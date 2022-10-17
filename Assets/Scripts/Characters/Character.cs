using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using Photon.Pun;
using _Scripts.Characters.Cameras;
using _Scripts.Characters.StateMachines;
using _Scripts.Interfaces;

namespace _Scripts.Characters
{
    [RequireComponent(typeof(PlayerInput))]
    [RequireComponent(typeof(CharacterController))]
    [RequireComponent(typeof(PhotonView))]
    [RequireComponent(typeof(PhotonTransformView))]
    public class Character : MonoBehaviour, IPlayerDamageable
    {
        #region Variables

        #region References

        [Header("Character references")]
        [SerializeField] protected AdventurerDatas characterDatas;
        [SerializeField] protected AdvSimpleCamera cameraPrefab;
        [SerializeField] private Transform mesh;
        [SerializeField] private Transform lookAt;
        [SerializeField] private Transform orientation;
        protected AdvSimpleCamera _myCamera;
        protected PlayerInput _inputs;
        private CharacterController _cc;
        protected Animator _animator;

        #endregion

        #region Character

        [Header("Character stats")]
        [SerializeField] private float healthRecup = 2f;
        [SerializeField] private float staminaRecup = 2f;
        [SerializeField] private float healthRecupTime = 3f;
        [SerializeField] private float staminaToRun = 1f;
        [SerializeField] private float staminaToDodge = 20f;

        private Coroutine _healthRecupCoroutine;
        private bool _gainHealth = true;


        [Header("Character motion")]
        [SerializeField] private float walkSpeed = 3f;
        [SerializeField] private float runSpeed = 7f;
        [SerializeField] private float aimWalkSpeed = 1f;
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
        public PlayerInput Inputs => _inputs;
        public Transform Mesh => mesh;
        public Transform Orientation => orientation;
        public Vector2 InputsVector { get; private set; }
        public Vector3 Movement { get; set; }
        public Vector2 OverrideDir { get; set; }
        public float CurrentSpeed { get; set; }
        public float CurrentHealth { get; set; }
        public float CurrentStamina { get; set; }
        public bool UsingStamina { get; set; }
        public float StaminaToRun => staminaToRun;
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
            InitializeCharacter();

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
            HandleCombat();
            SetOrientation();
            UpdateAnimations();

            HandleHealthRecup();
            HandleStaminaRecup();
        }

        #endregion

        #region Methods

        /// <summary>
        /// Initiliazing player stats
        /// </summary>
        protected void InitializeCharacter()
        {
            CurrentHealth = characterDatas.health;
            CurrentStamina = characterDatas.stamina;
            UsingStamina = true;
        }

        #region Health

        /// <summary>
        /// Reduce health by the amount of damages
        /// </summary>
        /// <param name="damages"> incoming damages </param>
        public void DamagePlayer(float damages)
        {
            if (!PViewIsMine)
                return;

            CurrentHealth -= damages;
            CurrentHealth = Mathf.Clamp(CurrentHealth, 0f, characterDatas.health);

            if (CurrentHealth <= 0)
            {
                Debug.Log("PlayerIsDead");
            }
            else
            {
                if (_healthRecupCoroutine != null)
                    StopCoroutine(_healthRecupCoroutine);

                _healthRecupCoroutine = StartCoroutine("DamageTempo");
            }
        }

        /// <summary>
        /// Handle health recuperation
        /// </summary>
        protected void HandleHealthRecup()
        {
            if (!_gainHealth)
                return;

            if (CurrentHealth < characterDatas.health)
                CurrentHealth += healthRecup * Time.deltaTime;

            CurrentHealth = Mathf.Clamp(CurrentHealth, 0f, characterDatas.health);
        }

        /// <summary>
        /// Wait time before recup health
        /// </summary>
        public IEnumerator DamageTempo()
        {
            _gainHealth = false;

            yield return new WaitForSecondsRealtime(healthRecupTime);

            _gainHealth = true;
            _healthRecupCoroutine = null;
        }

        #endregion

        #region Stamina

        /// <summary>
        /// Handle stamina recuperation
        /// </summary>
        protected void HandleStaminaRecup()
        {
            UsingStamina = PlayerStateMachine.IsThisState(PlayerStateMachine.PlayerStates.Roll) || (RunCondition());

            if (!UsingStamina && CurrentStamina < characterDatas.stamina)
                CurrentStamina += staminaRecup * Time.deltaTime;

            CurrentStamina = Mathf.Clamp(CurrentStamina, 0f, characterDatas.stamina);
        }

        /// <summary>
        /// using stamina
        /// </summary>
        /// <param name="amount"> amount of stamina used </param>
        public void UseStamina(float amount)
        {
            if (!PViewIsMine)
                return;

            CurrentStamina -= amount;
        }

        #endregion

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

            AdvSimpleCamera instance = PhotonNetwork.Instantiate(cameraPrefab.name, transform.position, Quaternion.identity).GetComponent<AdvSimpleCamera>();
            instance.SetCamera(lookAt);

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
            _animator.SetFloat("DirX", _currentInputs.x);
            _animator.SetFloat("DirY", _currentInputs.y);

            float current = _animator.GetFloat("Motion");
            float target = RunCondition() ? 2f : CurrentSpeed >= walkSpeed ? 1f : InputsVector.magnitude >= 0.1f ? InputsVector.magnitude : 0f;
            float final = Mathf.Lerp(current, target, 0.1f);
            _animator.SetFloat("Motion", final);

            _animator.SetBool("Aiming", PlayerStateMachine.IsAiming);

            float weight = PlayerStateMachine.EnableLayers ? 1f : 0f;
            SetLowerBodyWeight(Mathf.Lerp(_animator.GetLayerWeight(1), weight, 0.05f));
        }

        /// <summary>
        /// Method to set layer waight
        /// </summary>
        /// <param name="value"> target value </param>
        public void SetLowerBodyWeight(float value)
        {
            _animator.SetLayerWeight(1, value);
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
        protected Vector3 GetOverrideVector(float speed)
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
            if (PlayerStateMachine.IsAiming)
                return aimWalkSpeed;
            else if (RunCondition())
                return runSpeed;
            else if (InputsVector.magnitude >= 0.1f)
                return walkSpeed;

            return 0f;
        }

        /// <summary>
        /// Conditions if the player can run
        /// </summary>
        protected virtual bool RunCondition()
        {
            return CurrentStamina > 0f && GroundStateMachine.IsThisState(GroundStateMachine.GroundStatements.Grounded) && PlayerStateMachine.IsThisState(PlayerStateMachine.PlayerStates.Walk)
                    && !PlayerStateMachine.IsAiming && InputsVector.magnitude >= 0.8f && _inputs.actions["Run"].IsPressed();
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
        protected void SetOrientation()
        {
            orientation.rotation = Quaternion.Euler(0f, _myCamera.MainCam.transform.eulerAngles.y, 0f);
        }

        /// <summary>
        /// Handle mesh rotation
        /// </summary>
        protected virtual void RotatePlayer()
        {
            if (!mesh || PlayerStateMachine.EnableLayers)
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

        /// <summary>
        /// Turn the player in inputs direction
        /// </summary>
        public void TurnPlayer()
        {
            Vector2 inputs = InputsVector;
            Vector3 dir;

            if (inputs.magnitude <= 0)
                dir = Mesh.forward;
            else
                dir = Orientation.forward * inputs.y + Orientation.right * inputs.x;

            Mesh.rotation = Quaternion.LookRotation(dir, transform.up);

            OverrideDir = MeshDirection();
        }

        #endregion

        #endregion

        #region Dodge
        /// <summary>
        /// Roll action callback
        /// </summary>
        private void StartRoll(InputAction.CallbackContext _)
        {
            if (!DodgeCondition() && CurrentStamina < staminaToDodge)
                return;

            UseStamina(staminaToDodge);
            TurnPlayer();
            _animator.SetTrigger("Roll");
        }

        /// <summary>
        /// Conidition to be able to dodge
        /// </summary>
        private bool DodgeCondition()
        {
            return PlayerStateMachine.CanDodge && PlayerStateMachine.CurrentState != PlayerStateMachine.PlayerStates.Roll
                        && GroundStateMachine.CurrentStatement == GroundStateMachine.GroundStatements.Grounded;
        }
        #endregion

        #region Combat

        /// <summary>
        /// Handle combat methods
        /// </summary>
        protected virtual void HandleCombat()
        {
            if (AimCondition() && _inputs.actions["Aim"].IsPressed())
                PlayerStateMachine.IsAiming = true;
            else
                PlayerStateMachine.IsAiming = false;
        }

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
        protected virtual bool AttackCondition()
        {
            return PlayerStateMachine.CanAttack && !PlayerStateMachine.IsThisState(PlayerStateMachine.PlayerStates.Roll)
                && GroundStateMachine.IsThisState(GroundStateMachine.GroundStatements.Grounded);
        }

        /// <summary>
        /// Handle player rotation during aiming
        /// </summary>
        public void AimMeshRotation()
        {
            if (PlayerStateMachine.IsThisState(PlayerStateMachine.PlayerStates.Attack))
                return;

            mesh.rotation = Quaternion.LookRotation(orientation.forward, Vector3.up);
        }

        /// <summary>
        /// Conditions to be able to aim
        /// </summary>
        protected virtual bool AimCondition()
        {
            return GroundStateMachine.IsThisState(GroundStateMachine.GroundStatements.Grounded);
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
        public bool CanDodge { get; set; }
        public bool IsAiming { get; set; }
        public bool EnableLayers { get; set; }

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
