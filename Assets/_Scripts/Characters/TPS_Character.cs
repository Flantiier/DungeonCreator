using System;
using UnityEngine;
using Photon.Pun;
using Sirenix.OdinInspector;
using _Scripts.Cameras;
using _Scripts.Characters.StateMachines;
using static _Scripts.Characters.StateMachines.GroundStateMachine;
using _Scripts.Managers;

namespace _Scripts.Characters
{
    [RequireComponent(typeof(CharacterController))]
    [RequireComponent(typeof(PhotonTransformView))]
    public class TPS_Character : Entity
    {
        #region Variables

        #region References
        [TitleGroup("References/Character References")]
        [SerializeField] protected Transform orientation;
        [TitleGroup("References/Character References")]
        [SerializeField] protected Transform mesh;
        [TitleGroup("References/Character References")]
        [SerializeField] protected Transform lookAt;
        [TitleGroup("References/Character References")]
        [SerializeField] private TpsCamera tpsCameraPrefab;

        protected CharacterController _cc;
        protected CharacterAudio _audioSource;
        protected TpsCamera _camera;
        #endregion

        #region Physics
        [FoldoutGroup("Physics")]
        [SerializeField] private LayerMask walkableMask;
        [FoldoutGroup("Physics")]
        [Range(0.25f, 2f), GUIColor(0.3f, 2, 0.3f)]
        [SerializeField] private float maxGroundDistance = 1f;
        [FoldoutGroup("Physics")]
        [Range(1f, 10f), GUIColor(3, 0.5f, 1)]
        [SerializeField] private float appliedGravity = 5f;
        [FoldoutGroup("Physics")]
        [Range(0f, 0.1f), GUIColor(2, 2, 2)]
        [SerializeField] private float fallSmoothing = 0.05f;

        protected float _airTime;
        #endregion

        #region Variables
        [FoldoutGroup("Feedback")]
        [SerializeField] private int hitIndex = 1;
        [FoldoutGroup("Feedback")]
        [SerializeField] private float hitDelay = 0.8f;

        private float _lastHitFeedback;
        #endregion

        protected Vector2 _currentInputs;
        protected Vector3 _movement;
        protected Vector2 _smoothInputsRef;
        protected float _smoothSpeedRef;
        protected float _smoothMeshTurnRef;
        #endregion

        #region Properties
        public GroundStateMachine GroundSM { get; protected set; }
        public Transform MainCamera => _camera.CameraTransform;
        public Transform Orientation => orientation;
        public Vector2 Inputs { get; protected set; }
        public float CurrentSpeed { get; set; }
        #endregion

        #region Builts_In
        public virtual void Awake()
        {
            _cc = GetComponent<CharacterController>();
            GroundSM = new GroundStateMachine();
            _audioSource = GetComponentInChildren<CharacterAudio>();

            if (!ViewIsMine())
                return;

            InstantiateTPSCamera();
        }

        public override void OnEnable()
        {
            if (!ViewIsMine())
                return;

            EnableInputs(true);
            SubscribeInputActions();
            GameUIManager.OnMenuOpen += EnableInputs;
            GameUIManager.OnMenuOpen += _camera.EnableInputProvider;
        }

        public override void OnDisable()
        {
            if (!ViewIsMine())
                return;

            EnableInputs(false);
            UnsubscribeInputActions();
            GameUIManager.OnMenuOpen -= EnableInputs;
            GameUIManager.OnMenuOpen -= _camera.EnableInputProvider;
        }

        protected virtual void Update()
        {
            if (!ViewIsMine())
                return;

            SetOrientation();
            HandleGroundStateMachine();
            UpdateAnimations();
        }
        #endregion

        #region Methods

        #region Camera Methods
        /// <summary>
        /// Instantiate a camera for the player
        /// </summary>
        protected virtual void InstantiateTPSCamera()
        {
            if (!tpsCameraPrefab)
            {
                Debug.LogError("Missing camera prefab");
                return;
            }

            _camera = Instantiate(tpsCameraPrefab, transform.position, Quaternion.identity);
            _camera.SetLookAt(lookAt);
        }
        #endregion

        #region Health Methods
        protected override void HandleEntityHealth(float damages)
        {
            CurrentHealth = ClampedHealth(damages, 0f, Mathf.Infinity);
            RPCCall("HealthRPC", RpcTarget.OthersBuffered, CurrentHealth);

            if (CurrentHealth <= 0)
                HandleEntityDeath();
        }

        protected override void HandleEntityDeath()
        {
            EnableInputs(false);
            ResetCharacterVelocity();

            RPCAnimatorTrigger(RpcTarget.All, "Dead", true);
        }
        #endregion

        #region Inputs Methods
        /// <summary>
        /// Enable and disable inputs based on the given parameter
        /// </summary>
        protected virtual void EnableInputs(bool enabled) { }

        /// <summary>
        /// Subscribe to the given action events to update inputs vector
        /// </summary>
        protected virtual void SubscribeInputActions() { }

        /// <summary>
        /// Unsubscribe to the given action events to update inputs vector
        /// </summary>
        protected virtual void UnsubscribeInputActions() { }

        /// <summary>
        /// Smoothly setting the current inputs value
        /// </summary>
        /// <param name="inputs"> Incoming inputs </param>
        /// <param name="smoothing"> Smoothing value </param>
        protected void SmoothingInputs(Vector2 inputs, float smoothing)
        {
            _currentInputs = Vector2.SmoothDamp(_currentInputs, inputs, ref _smoothInputsRef, smoothing);
        }
        #endregion

        #region StateMachines Methods
        /// <summary>
        /// Handle the character behaviour on/off the ground
        /// </summary>
        protected void HandleGroundStateMachine()
        {
            GroundSM.CurrentState = _cc.isGrounded || LowGroundDetect() ? GroundStatements.Grounded : GroundStatements.Falling;

            switch (GroundSM.CurrentState)
            {
                case GroundStatements.Grounded:
                    HandleCharacterStateMachine();
                    break;

                case GroundStatements.Falling:
                    HandleFall();
                    break;
            }

            _cc.Move(_movement * Time.deltaTime);
        }

        /// <summary>
        /// Handle the character behaviour in each state
        /// </summary>
        protected virtual void HandleCharacterStateMachine() { }

        /// <summary>
        /// Update animator parameters
        /// </summary>
        protected virtual void UpdateAnimations() { }

        #endregion

        #region Motion Methods
        /// <summary>
        /// Move the character based on user inputs
        /// </summary>
        protected virtual void HandleCharacterMotion()
        {
            Vector3 movement = (orientation.forward * _currentInputs.y + orientation.right * _currentInputs.x) * CurrentSpeed;
            movement.y = -appliedGravity;

            _movement = movement;
        }

        /// <summary>
        /// Move the character based on the mesh orientation
        /// </summary>
        public void MoveForwards()
        {
            _movement = new Vector3(mesh.forward.x, -appliedGravity, mesh.forward.z) * CurrentSpeed;
        }

        /// <summary>
        /// Move the character based on the mesh orientation
        /// </summary>
        public void MoveBackwards()
        {
            _movement = new Vector3(-mesh.forward.x, -appliedGravity, -mesh.forward.z) * CurrentSpeed;
        }

        /// <summary>
        /// Smoothly updating character motion speed
        /// </summary>
        public virtual void UpdateCharacterSpeed(float speed)
        {
            CurrentSpeed = Mathf.SmoothDamp(CurrentSpeed, speed, ref _smoothSpeedRef, 0.1f);
        }

        /// <summary>
        /// Reset player momentum
        /// </summary>
        public void ResetCharacterVelocity()
        {
            _currentInputs = Vector2.zero;
            _movement = new Vector3(0f, _movement.y, 0f);
            _cc.SimpleMove(_movement);
        }
        #endregion

        #region Rotations Methods
        /// <summary>
        /// Setting the orientation transform to look towards mainCamera forward
        /// </summary>
        protected void SetOrientation()
        {
            orientation.rotation = Quaternion.Euler(0f, MainCamera.eulerAngles.y, 0f);
        }

        /// <summary>
        /// Setting the mesh rotation based on an orientation
        /// </summary>
        /// <param name="orientation"> New orientation </param>
        public void SetMeshOrientation(Vector3 orientation)
        {
            mesh.rotation = Quaternion.LookRotation(orientation, Vector3.up);
        }

        /// <summary>
        /// Setting mesh rotations based on current inputs
        /// </summary>
        protected virtual void HandleCharacterRotation(float smooth)
        {
            if (!mesh)
                return;

            if (Inputs.magnitude >= 0.1f)
            {
                float angle = Mathf.Atan2(Inputs.x, Inputs.y) * Mathf.Rad2Deg + orientation.eulerAngles.y;
                float smoothAngle = Mathf.SmoothDampAngle(mesh.eulerAngles.y, angle, ref _smoothMeshTurnRef, smooth);
                mesh.rotation = Quaternion.Euler(0f, smoothAngle, 0f);
            }
        }

        /// <summary>
        /// Setting the mesh rotation to a flatten plan vector
        /// </summary>
        public virtual void LookTowardsOrientation()
        {
            SetMeshOrientation(orientation.forward);
        }
        #endregion

        #region Physics Methods
        /// <summary>
        /// Simulates a fluid fall over the time spend in air
        /// </summary>
        private void HandleFall()
        {
            _airTime += Time.deltaTime;

            Vector3 movement = Vector3.Slerp(_movement, Vector3.zero, fallSmoothing / 10f);
            movement.y = -appliedGravity * _airTime;

            _movement = movement;
        }

        /// <summary>
        /// Shooting a raycast to detect if there's a low ground under the player
        /// </summary>
        public bool LowGroundDetect()
        {
            Ray ray = new Ray(transform.position + new Vector3(0f, 0.25f, 0f), -transform.up);
            Debug.DrawRay(ray.origin, ray.direction * maxGroundDistance);

            if (Physics.Raycast(ray, maxGroundDistance, walkableMask))
                return true;

            return false;
        }
        #endregion

        #region Audio Methods
        protected virtual void PlayHitSound()
        {
            if (Time.time <= _lastHitFeedback + hitDelay)
                return;

            _audioSource.PlayClip(hitIndex);
            _lastHitFeedback = Time.time;
        }
        #endregion

        #endregion
    }
}

#region GroundStateMachine_Class

namespace _Scripts.Characters.StateMachines
{
    [Serializable]
    public class GroundStateMachine
    {
        #region Properties
        public enum GroundStatements { Grounded, Falling }
        public GroundStatements CurrentState { get; set; }
        public bool IsLanding { get; set; }
        #endregion

        #region Methods
        public GroundStateMachine()
        {
            CurrentState = GroundStatements.Grounded;
        }

        /// <summary>
        /// Indicates if the given state is the same as the current one
        /// </summary>
        /// <param name="targetState"> State to check </param>
        public bool IsStateOf(GroundStatements targetState)
        {
            return CurrentState == targetState;
        }
        #endregion
    }
}

#endregion