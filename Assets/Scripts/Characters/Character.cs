using System;
using UnityEngine;
using UnityEngine.InputSystem;
using Photon.Pun;
using _Scripts.Characters.StateMachines;

namespace _Scripts.Characters
{
    [RequireComponent(typeof(PlayerInput))]
    [RequireComponent(typeof(CharacterController))]
    [RequireComponent(typeof(PhotonView))]
    [RequireComponent(typeof(PhotonTransformView))]
    public class Character : MonoBehaviour
    {
        #region Variables

        #region References
        [Header("Character references")]
        [SerializeField] protected AdventurerDatas characterDatas;
        [SerializeField] protected AdventurerCameraSetup cameraPrefab;
        [SerializeField] private Transform mesh;
        [SerializeField] private Transform lookAt;
        [SerializeField] public Transform orientation;
        protected AdventurerCameraSetup _myCamera;
        protected PlayerInput _inputs;
        private CharacterController _cc;
        protected Animator _animator;
        #endregion

        #region Motion
        [Header("Character properties")]
        [SerializeField] private float walkSpeed = 3f;
        [SerializeField] private float runSpeed = 7f;
        [SerializeField] private float dodgeSpeed = 10f;
        [SerializeField, Range(0f, 0.2f)] private float inputsSmoothing = 0.1f;
        [SerializeField, Range(0f, 0.2f)] private float speedSmoothing = 0.15f;
        [SerializeField, Range(0f, 0.2f)] private float meshRotation = 0.1f;

        private Vector2 _currentInputs;
        private Vector2 _smoothInputsRef;
        private float _speedSmoothingRef;
        private float _meshTurnRef;
        #endregion

        #region Physics

        [Header("Ground infos")]
        [SerializeField] private LayerMask walkableMask;
        [SerializeField] private float maxGroundDistance = 1f;
        private bool _lowGround;

        [Header("Gravity properties")]
        [SerializeField] private float appliedGravity = 5f;
        [SerializeField, Range(0f, 0.1f)] private float fallSmoothing = 0.05f;
        private float _airTime;

        #endregion

        #endregion

        #region Properties

        public PhotonView PView { get; private set; }
        public bool PViewIsMine { get; private set; }
        public GroundStateMachine GroundStateMachine { get; private set; }
        public PlayerStateMachine PlayerStateMachine { get; private set; }
        public Transform Mesh => mesh;
        public Vector2 InputsVector { get; private set; }
        public Vector3 Movement { get; set; }
        public Vector2 OverrideDir { get; set; }
        public float CurrentSpeed { get; set; }
        public float DodgeSpeed => dodgeSpeed;
        public float AirTime => _airTime;

        #endregion

        #region Builts_In

        public virtual void Awake()
        {
            PhotonView view = GetComponent<PhotonView>();
            PViewIsMine = view.IsMine;

            if (!PViewIsMine)
                return;

            PView = view;
            _inputs = GetComponent<PlayerInput>();
            _cc = GetComponent<CharacterController>();
            _animator = mesh.GetComponent<Animator>();
            GroundStateMachine = new GroundStateMachine();
            PlayerStateMachine = new PlayerStateMachine();

            InstantiateCamera();
        }

        public virtual void OnEnable()
        {
            if (!PViewIsMine)
                return;

            SubscribeToInputs();
        }

        public virtual void OnDisable()
        {
            if (!PViewIsMine)
                return;

            UnsubscribeToInputs();
        }

        public virtual void OnDestroy()
        {
            if (!PViewIsMine)
                return;

            if (_myCamera)
                PhotonNetwork.Destroy(_myCamera.gameObject);
        }

        public virtual void Update()
        {
            if (!PViewIsMine)
                return;

            HandleGroundStateMachine();
            SetOrientation();
            UpdateAnimations();
        }

        #endregion

        #region Methods

        #region Inputs
        /// <summary>
        /// Subscribe Player actions to methods
        /// </summary>
        protected virtual void SubscribeToInputs()
        {
            _inputs.ActivateInput();

            _inputs.actions["Move"].performed += ctx => InputsVector = ctx.ReadValue<Vector2>();
            _inputs.actions["Move"].canceled += ctx => InputsVector = Vector2.zero;

            _inputs.actions["Roll"].started += StartRoll;

            _inputs.actions["Attack"].started += StartAttack;
        }

        /// <summary>
        /// Unsubscribe Player actions to methods
        /// </summary>
        protected virtual void UnsubscribeToInputs()
        {
            _inputs.DeactivateInput();

            _inputs.actions["Move"].performed -= ctx => InputsVector = ctx.ReadValue<Vector2>();
            _inputs.actions["Move"].canceled -= ctx => InputsVector = Vector2.zero;

            _inputs.actions["Roll"].started -= StartRoll;

            _inputs.actions["Attack"].started -= StartAttack;
        }
        #endregion

        #region Camera

        /// <summary>
        /// Instantiate a camera for the player
        /// </summary>
        private void InstantiateCamera()
        {
            if (!cameraPrefab)
            {
                Debug.LogError("Missing camera prefab");
                return;
            }

            AdventurerCameraSetup instance = PhotonNetwork.Instantiate(cameraPrefab.name, transform.position, Quaternion.identity).GetComponent<AdventurerCameraSetup>();
            instance.SetCameraInfos(lookAt);
            _myCamera = instance;
        }

        #endregion

        #region StateMachines Methods

        /// <summary>
        /// Handle the GroundStateMachine
        /// </summary>
        private void HandleGroundStateMachine()
        {
            GroundStateMachine.CurrentStatement = _cc.isGrounded || _lowGround ? GroundStateMachine.GroundStatements.Grounded : GroundStateMachine.GroundStatements.Falling;

            switch (GroundStateMachine.CurrentStatement)
            {
                case GroundStateMachine.GroundStatements.Grounded:

                    _lowGround = LowGroundDetect();
                    HandlePlayerStateMachine();
                    break;

                case GroundStateMachine.GroundStatements.Falling:

                    HandleFall();
                    break;
            }

            _cc.Move(Movement * Time.deltaTime);
        }

        /// <summary>
        /// Handle the PlayerStateMachine
        /// </summary>
        private void HandlePlayerStateMachine()
        {
            switch (PlayerStateMachine.CurrentState)
            {
                case PlayerStateMachine.PlayerStates.Walk:

                    if (!GroundStateMachine.IsLanding)
                    {
                        HandleMotion();
                    }
                    break;
            }
        }

        /// <summary>
        /// Set player animations
        /// </summary>
       protected virtual void UpdateAnimations()
        {
            if (!_animator)
                return;

            _animator.SetFloat("CurrentState", _animator.GetCurrentAnimatorStateInfo(0).normalizedTime);
            _animator.SetBool("IsGrounded", GroundStateMachine.IsThisState(GroundStateMachine.GroundStatements.Grounded) || _lowGround);
            _animator.SetFloat("Inputs", InputsVector.magnitude);

            float current = _animator.GetFloat("Motion");
            float target = RunCondition() ? 2f : CurrentSpeed >= walkSpeed ? 1f : InputsVector.magnitude >= 0.1f ? InputsVector.magnitude : 0f;
            float value = Mathf.Lerp(current, target, 0.1f);
            _animator.SetFloat("Motion", value);
        }

        #endregion

        #region Motion

        /// <summary>
        /// Handle player motion
        /// </summary>
        private void HandleMotion()
        {
            _currentInputs = Vector2.SmoothDamp(_currentInputs, InputsVector, ref _smoothInputsRef, inputsSmoothing);

            UpdateSpeed(GetMovementSpeed());

            Vector3 movement = (orientation.forward * _currentInputs.y + orientation.right * _currentInputs.x) * CurrentSpeed;
            movement.y = -appliedGravity;
            Movement = movement;

            RotatePlayer();
        }

        /// <summary>
        /// Handle the player movement during an animation
        /// </summary>
        /// <param name="speed"> Incoming motion speed </param>
        public void HandleDodgeMovement(float speed)
        {
            UpdateSpeed(speed);

            Movement = GetOverrideVector(CurrentSpeed);
        }

        /// <summary>
        /// Handle the player movement durinng an animation
        /// </summary>
        /// <param name="speed"> Combat motion speed </param>
        public void HandleCombatMovement(float speed)
        {
            UpdateSpeed(speed);

            Movement = GetOverrideVector(speed);
        }

        /// <summary>
        /// Return the override vector multiplied by the speed
        /// </summary>
        /// <param name="speed"> Motion speed </param>
        private Vector3 GetOverrideVector(float speed)
        {
            return new Vector3(OverrideDir.x, -appliedGravity, OverrideDir.y) * speed;
        }

        /// <summary>
        /// Lerping the motion speed to a target speed
        /// </summary>
        /// <param name="targetSpeed"> Targeting speed </param>
        private void UpdateSpeed(float targetSpeed)
        {
            CurrentSpeed = Mathf.SmoothDamp(CurrentSpeed, targetSpeed, ref _speedSmoothingRef, speedSmoothing);
        }

        /// <summary>
        /// Return the targeted motion speed
        /// </summary>
        public float GetMovementSpeed()
        {
            if (RunCondition())
                return runSpeed;
            else if (InputsVector.magnitude >= 0.1f)
                return walkSpeed;

            return 0f;
        }

        /// <summary>
        /// Conditions if the player can run
        /// </summary>
        private bool RunCondition()
        {
            return InputsVector.magnitude >= 0.8f && _inputs.actions["Run"].IsPressed();
        }

        /// <summary>
        /// Reset player momentum
        /// </summary>
        public void ResetVelocity()
        {
            _currentInputs = Vector2.zero;
            CurrentSpeed = 0f;
            Movement = new Vector3(0f, Movement.y, 0f);
        }

        #region Rotations

        /// <summary>
        /// Setting player orientation
        /// </summary>
        private void SetOrientation()
        {
            orientation.rotation = Quaternion.Euler(0f, _myCamera.MainCam.transform.eulerAngles.y, 0f);
        }

        /// <summary>
        /// Handle mesh rotation
        /// </summary>
        private void RotatePlayer()
        {
            if (!mesh)
                return;

            if (InputsVector.magnitude >= 0.1f)
            {
                float angle = Mathf.Atan2(InputsVector.x, InputsVector.y) * Mathf.Rad2Deg + orientation.eulerAngles.y;
                float smoothAngle = Mathf.SmoothDampAngle(mesh.eulerAngles.y, angle, ref _meshTurnRef, meshRotation);
                mesh.rotation = Quaternion.Euler(0f, smoothAngle, 0f);
            }
        }

        /// <summary>
        /// Return the direction of the mesh
        /// </summary>
        public Vector2 MeshDirection()
        {
            return new Vector2(mesh.forward.x, mesh.forward.z);
        }

        #endregion

        #endregion

        #region Dodge
        /// <summary>
        /// Roll action callback
        /// </summary>
        private void StartRoll(InputAction.CallbackContext _)
        {
            if (!DodgeCondition())
                return;
                        
            OverrideDir = MeshDirection();
            _animator.SetTrigger("Roll");
        }

        /// <summary>
        /// Conidition to be able to dodge
        /// </summary>
        private bool DodgeCondition()
        {
            return IsAttacking() && PlayerStateMachine.CurrentState != PlayerStateMachine.PlayerStates.Roll 
                        && GroundStateMachine.CurrentStatement == GroundStateMachine.GroundStatements.Grounded;
        }
        #endregion

        #region Combat

        /// <summary>
        /// Attack action callback
        /// </summary>
        private void StartAttack(InputAction.CallbackContext _)
        {
            if (!AttackCondition())
                return;

            _animator.SetTrigger("Attack");
        }

        /// <summary>
        /// Condition to be able to attack
        /// </summary>
        private bool AttackCondition()
        {
            return PlayerStateMachine.CanAttack && !PlayerStateMachine.IsThisState(PlayerStateMachine.PlayerStates.Roll)
                && GroundStateMachine.IsThisState(GroundStateMachine.GroundStatements.Grounded);
        }

        /// <summary>
        /// Indicates of the player is currently attacking
        /// </summary>
        private bool IsAttacking()
        {
            if (!PlayerStateMachine.IsThisState(PlayerStateMachine.PlayerStates.Attack))
                return true;

            if (_animator.GetFloat("CurrentState") < 0.7f)
                return false;

            return true;
        }

        #endregion

        #region Physics
        /// <summary>
        /// Handle fall movement
        /// </summary>
        private void HandleFall()
        {
            //Increase air time
            _airTime += Time.deltaTime;

            //Inputs
            Vector3 movement = Vector3.Slerp(Movement, Vector3.zero, fallSmoothing / 10f);
            movement.y = -appliedGravity * _airTime;
            Movement = movement;
        }

        /// <summary>
        /// Reset player airTime
        /// </summary>
        public void ResetAirTime()
        {
            _airTime = 0f;
        }

        /// <summary>
        /// Shooting a raycast to detect if there's a low ground
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

#region GroundSM_Class

namespace _Scripts.Characters.StateMachines
{
    [Serializable]
    public class GroundStateMachine
    {
        public enum GroundStatements { Grounded, Falling }
        public GroundStatements CurrentStatement { get; set; }
        public bool IsLanding { get; set; }

        /// <summary>
        /// Return if the target state is the same as the current
        /// </summary>
        /// <param name="targetState"> Target State </param>
        public bool IsThisState(GroundStatements targetState)
        {
            return CurrentStatement == targetState;
        }
    }
}

#endregion

#region PlayerSM_Class

namespace _Scripts.Characters.StateMachines
{
    [Serializable]
    public class PlayerStateMachine
    {
        public enum PlayerStates { Walk, Roll, Attack }
        public PlayerStates CurrentState { get; set; }
        public bool CanAttack { get; set; }

        /// <summary>
        /// Return if the target state is the same as the current
        /// </summary>
        /// <param name="targetState"> Target State </param>
        public bool IsThisState(PlayerStates targetState)
        {
            return CurrentState == targetState;
        }
    }
}

#endregion
