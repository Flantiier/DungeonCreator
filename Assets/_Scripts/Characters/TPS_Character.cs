using System;
using UnityEngine;
using Photon.Pun;
using _Scripts.Characters.Cameras;
using _Scripts.Characters.StateMachines;
using static _Scripts.Characters.StateMachines.GroundStateMachine;

namespace _Scripts.Characters
{
    [RequireComponent(typeof(CharacterController))]
    [RequireComponent(typeof(PhotonView))]
    [RequireComponent(typeof(PhotonTransformView))]
    public class TPS_Character : Entity
    {
        #region Variables

        #region References
        [Header("Character references")]
        [SerializeField] protected Transform orientation;
        [SerializeField] protected Transform mesh;
        [SerializeField] protected Transform lookAt;
        [SerializeField] private TpsCameraProfile cameraPrefab;

        protected CharacterController _cc;
        protected TpsCameraProfile _tpsCamera;
        #endregion

        #region Motion
        [Header("Motion properties")]
        [SerializeField, Range(0f, 0.2f)] protected float inputSmoothing = 0.1f;
        [SerializeField, Range(0f, 0.2f)] protected float speedSmoothing = 0.15f;
        [SerializeField, Range(0f, 0.2f)] protected float rotationSmoothing = 0.1f;

        protected Vector2 _currentInputs;
        protected Vector3 _movement;
        protected Vector2 _smoothInputsRef;
        protected float _smoothSpeedRef;
        protected float _smoothMeshTurnRef;
        #endregion

        #region Physics
        [Header("Ground infos")]
        [SerializeField] private LayerMask walkableMask;
        [SerializeField] private float maxGroundDistance = 1f;

        [Header("Gravity properties")]
        [SerializeField] private float appliedGravity = 5f;
        [SerializeField, Range(0f, 0.1f)] private float fallSmoothing = 0.05f;
        protected float _airTime;
        #endregion

        #endregion

        #region Properties
        public GroundStateMachine GroundSM { get; protected set; }
        public Transform MainCamTransform => _tpsCamera.MainCam.transform;
        public Transform Orientation => orientation;
        public Vector2 Inputs { get; protected set; }
        public float CurrentSpeed { get; set; }
        #endregion

        #region Builts_In
        public virtual void Awake()
        {
            _cc = GetComponent<CharacterController>();
            GroundSM = new GroundStateMachine();

            if (!ViewIsMine())
                return;

            InstantiateCamera();
        }
        public override void OnEnable()
        {
            if (!ViewIsMine())
                return;

            InputsEnabled(true);
            SubscribeInputActions();
        }

        public override void OnDisable()
        {
            if (!ViewIsMine())
                return;

            InputsEnabled(false);
            UnsubscribeInputActions();
        }

        public virtual void OnDestroy()
        {
            if (!ViewIsMine())
                return;

            if (_tpsCamera.MainCam)
                PhotonNetwork.Destroy(_tpsCamera.MainCam.gameObject);
        }
        #endregion

        #region Methods

        #region Camera Methods
        /// <summary>
        /// Instantiate a camera for the player
        /// </summary>
        protected virtual void InstantiateCamera()
        {
            if (!cameraPrefab)
            {
                Debug.LogError("Missing camera prefab");
                return;
            }

            TpsCameraProfile instance = PhotonNetwork.Instantiate(cameraPrefab.name, transform.position, Quaternion.identity).GetComponent<TpsCameraProfile>();
            instance.SetLookAtTarget(lookAt);

            _tpsCamera = instance;
        }
        #endregion

        #region Health Methods
        protected override void HandleEntityHealth(float damages)
        {
            CurrentHealth = ClampedHealth(damages, 0f, Mathf.Infinity);
            RPCCall("HealthRPC", RpcTarget.Others, CurrentHealth);

            if (CurrentHealth <= 0)
                HandleEntityDeath();
        }

        protected override void HandleEntityDeath()
        {
            InputsEnabled(false);
            ResetCharacterVelocity();

            RPCAnimatorTrigger(RpcTarget.All, "Dead", true);
        }
        #endregion

        #region Inputs Methods
        /// <summary>
        /// Enable and disable inputs based on the given parameter
        /// </summary>
        protected virtual void InputsEnabled(bool enabled) { }

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
        public void MoveInMeshForward()
        {
            _movement = GetMeshForward(CurrentSpeed);
        }

        /// <summary>
        /// Mesh forward direction multiplied by a motion speed
        /// </summary>
        /// <param name="speed"> Character motion speed </param>
        protected Vector3 GetMeshForward(float speed)
        {
            return new Vector3(mesh.forward.x, -appliedGravity, mesh.forward.z) * speed;
        }

        /// <summary>
        /// Smoothly updating character motion speed
        /// </summary>
        public void UpdateCharacterSpeed(float speed)
        {
            CurrentSpeed = Mathf.SmoothDamp(CurrentSpeed, speed, ref _smoothSpeedRef, speedSmoothing);
        }

        /// <summary>
        /// Reset player momentum
        /// </summary>
        public void ResetCharacterVelocity()
        {
            _currentInputs = Vector2.zero;
            _movement = new Vector3(0f, _movement.y, 0f);
        }

        /// <summary>
        /// Run is enable when all the conditions are fullfilled
        /// </summary>
        protected virtual bool RunConditions()
        {
            if (!GroundSM.IsStateOf(GroundStatements.Grounded))
                return false;

            return Inputs.magnitude >= 0.8f;
        }
        #endregion

        #region Rotations Methods
        /// <summary>
        /// Setting the orientation transform to look towards mainCamera forward
        /// </summary>
        protected void SetOrientation()
        {
            orientation.rotation = Quaternion.Euler(0f, MainCamTransform.eulerAngles.y, 0f);
        }

        /// <summary>
        /// Setting the mesh rotation based on an orientation
        /// </summary>
        /// <param name="orientation"> New orientation </param>
        public void SetPlayerMeshOrientation(Vector3 orientation)
        {
            mesh.rotation = Quaternion.LookRotation(orientation, Vector3.up);
        }

        /// <summary>
        /// Setting mesh rotations based on current inputs
        /// </summary>
        protected virtual void HandleCharacterRotation()
        {
            if (!mesh)
                return;

            if (Inputs.magnitude >= 0.1f)
            {
                float angle = Mathf.Atan2(Inputs.x, Inputs.y) * Mathf.Rad2Deg + orientation.eulerAngles.y;
                float smoothAngle = Mathf.SmoothDampAngle(mesh.eulerAngles.y, angle, ref _smoothMeshTurnRef, rotationSmoothing);
                mesh.rotation = Quaternion.Euler(0f, smoothAngle, 0f);
            }
        }

        /// <summary>
        /// Setting the mesh rotation to a flatten plan vector
        /// </summary>
        public virtual void LookTowardsOrientation()
        {
            SetPlayerMeshOrientation(orientation.forward);
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