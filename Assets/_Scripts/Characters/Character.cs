using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using Photon.Pun;
using _Scripts.Characters.Cameras;
using _Scripts.Characters.StateMachines;
using _Scripts.Interfaces;
using _Scripts.Utilities.Florian;
using _SciptablesObjects.Adventurer;
using _Scripts.NetworkScript;
using UnityEditor.Build;

namespace _Scripts.Characters
{
    [RequireComponent(typeof(PlayerInput))]
    [RequireComponent(typeof(CharacterController))]
    [RequireComponent(typeof(PhotonView))]
    [RequireComponent(typeof(PhotonTransformView))]
    public class Character : NetworkMonoBehaviour, IPlayerDamageable
    {
        #region Variables

        #region References
        [Header("Stats references")]
        [SerializeField] protected CharactersOverallDatas overallDatas;
        [SerializeField] protected AdventurerDatas characterDatas;

        [Header("Character references")]
        [SerializeField] private Transform mesh;
        [SerializeField] private Transform lookAt;
        [SerializeField] private Transform orientation;

        [Header("Other references")]
        [SerializeField] protected CameraSetup cameraPrefab;
        [SerializeField] protected PlayerHUD playerHUD;

        protected PlayerInput _inputs;
        private CharacterController _cc;
        protected Animator _animator;
        protected TpsCameraHandler _tpsCamera;
        #endregion

        #region Character
        private Vector2 _currentInputs;
        private Vector2 _smoothInputsRef;
        private float _speedSmoothingRef;
        private float _meshTurnRef;

        private bool _gainHealth = true;
        private Coroutine _healthRecupCoroutine;
        private Coroutine _recenteringCoroutine;
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
        public GroundStateMachine GroundSM { get; private set; }
        public PlayerStateMachine PlayerSM { get; private set; }
        public CharactersOverallDatas OverallDatas => overallDatas;
        public AdventurerDatas CharacterDatas => characterDatas;
        public Transform MainCamTransform => _tpsCamera.MainCam.transform;
        public Transform Orientation => orientation;
        public Vector2 InputsVector { get; private set; }
        public Vector3 Movement { get; set; }
        public bool DisableInputs { get; set; }
        public float CurrentSpeed { get; set; }
        public float CurrentHealth { get; set; }
        public float CurrentStamina { get; set; }
        public float AirTime => _airTime;
        #endregion

        #region Builts_In
        public virtual void Awake()
        {
            _animator = mesh.GetComponent<Animator>();

            if (!ViewIsMine())
                return;

            _inputs = GetComponent<PlayerInput>();
            _cc = GetComponent<CharacterController>();

            InstantiateCamera();
            InstantiateHUD();
        }

        public virtual void OnEnable()
        {
            if (!ViewIsMine())
                return;

            InitializeCharacter();
            SubscribeToInputs();
        }

        public virtual void OnDisable()
        {
            if (!ViewIsMine())
                return;

            UnsubscribeToInputs();
        }

        public virtual void OnDestroy()
        {
            if (!ViewIsMine())
                return;

            if (_tpsCamera.MainCam)
                PhotonNetwork.Destroy(_tpsCamera.MainCam.gameObject);
        }

        public virtual void Update()
        {
            if (!ViewIsMine() || PlayerSM.IsThisState(PlayerStateMachine.PlayerStates.Dead))
                return;

            HandleGroundStateMachine();
            SetOrientation();
            UpdateAnimations();

            HandleHealthRecup();
            HandleStaminaRecup();
        }
        #endregion

        #region Methods
        /// <summary>
        /// Reset the player
        /// </summary>
        protected virtual void InitializeCharacter()
        {
            DisableInputs = false;

            GroundSM = new GroundStateMachine();
            PlayerSM = new PlayerStateMachine();

            CurrentHealth = characterDatas.health;
            CurrentStamina = characterDatas.stamina;
        }

        #region Health
        /// <summary>
        /// Reduce health by the amount of damages
        /// </summary>
        /// <param name="damages"> incoming damages </param>
        public void DamagePlayer(float damages)
        {
            if (!ViewIsMine())
                return;

            CurrentHealth -= damages;
            View.RPC("HealthRPC", RpcTarget.Others, CurrentHealth);

            if (_healthRecupCoroutine != null)
                StopCoroutine(_healthRecupCoroutine);

            if (CurrentHealth <= 0)
                HandleCharacterDeath();
            else
                _healthRecupCoroutine = StartCoroutine("DamageTempo");
        }

        /// <summary>
        /// Character death method
        /// </summary>
        [ContextMenu("Instant Death")]
        protected void HandleCharacterDeath()
        {
            Debug.Log("Player dead");

            PlayerSM.InvokeDeathEvent();
            CurrentHealth = 0f;

            View.RPC("CharacterDeathRPC", RpcTarget.All);
        }

        /// <summary>
        /// Handle health recuperation
        /// </summary>
        protected void HandleHealthRecup()
        {
            if (!_gainHealth)
                return;

            if (CurrentHealth < characterDatas.health)
                CurrentHealth += overallDatas.healthRecup * Time.deltaTime;

            CurrentHealth = Mathf.Clamp(CurrentHealth, 0f, characterDatas.health);
        }

        /// <summary>
        /// Wait time before recup health
        /// </summary>
        public IEnumerator DamageTempo()
        {
            _gainHealth = false;

            yield return new WaitForSecondsRealtime(overallDatas.healthRecupTime);

            _gainHealth = true;
            _healthRecupCoroutine = null;
        }

        /// <summary>
        /// Send health over network
        /// </summary>
        /// <param name="healthAmount"> Current health amount </param>
        [PunRPC]
        public void HealthRPC(float healthAmount)
        {
            CurrentHealth = healthAmount;
        }

        /// <summary>
        /// Send death over the network
        /// </summary>
        [PunRPC]
        public void CharacterDeathRPC()
        {
            _animator.SetTrigger("Dead");
        }
        #endregion

        #region Stamina
        /// <summary>
        /// Handle stamina recuperation
        /// </summary>
        protected void HandleStaminaRecup()
        {
            PlayerSM.UsingStamina = PlayerSM.IsThisState(PlayerStateMachine.PlayerStates.Roll) || RunCondition();

            if (!PlayerSM.UsingStamina && CurrentStamina < characterDatas.stamina)
                CurrentStamina += overallDatas.staminaRecup * Time.deltaTime;

            CurrentStamina = Mathf.Clamp(CurrentStamina, 0f, characterDatas.stamina);
        }

        /// <summary>
        /// using stamina
        /// </summary>
        /// <param name="amount"> amount of stamina used </param>
        public void UseStamina(float amount)
        {
            if (!ViewIsMine())
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

            _inputs.actions["Roll"].started += HandleDodge;

            _inputs.actions["MainAttack"].started += HandleMainAttack;
            _inputs.actions["SecondAttack"].started += HandleSecondAttack;
            _inputs.actions["MainAttack"].performed += ctx => PlayerSM.HoldAttack = ctx.ReadValueAsButton();
            _inputs.actions["MainAttack"].canceled += ctx => PlayerSM.HoldAttack = ctx.ReadValueAsButton();

            _inputs.actions["Recenter"].started += RecenterTpsCamera;

            PlayerSM.OnPlayerDeath += UnsubscribeToInputs;
        }

        /// <summary>
        /// Unsubscribe Player actions to methods
        /// </summary>
        protected virtual void UnsubscribeToInputs()
        {
            _inputs.DeactivateInput();

            _inputs.actions["Move"].performed -= ctx => InputsVector = ctx.ReadValue<Vector2>();
            _inputs.actions["Move"].canceled -= ctx => InputsVector = Vector2.zero;

            _inputs.actions["Roll"].started -= HandleDodge;

            _inputs.actions["MainAttack"].started -= HandleMainAttack;
            _inputs.actions["SecondAttack"].started -= HandleSecondAttack;
            _inputs.actions["MainAttack"].performed -= ctx => PlayerSM.HoldAttack = ctx.ReadValueAsButton();
            _inputs.actions["MainAttack"].canceled -= ctx => PlayerSM.HoldAttack = ctx.ReadValueAsButton();

            _inputs.actions["Recenter"].started -= RecenterTpsCamera;
        }
        #endregion

        #region Camera/UI
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

            TpsCameraHandler instance = PhotonNetwork.Instantiate(cameraPrefab.name, transform.position, Quaternion.identity).GetComponent<TpsCameraHandler>();
            instance.SetLookAtTarget(lookAt);

            _tpsCamera = instance;
        }

        /// <summary>
        /// Instantiate and set the player HUD
        /// </summary>
        private void InstantiateHUD()
        {
            PlayerHUD hud = Instantiate(playerHUD);
            hud.SetHUD(this);
        }

        /// <summary>
        /// Recentering camera behind the look at
        /// </summary>
        private void RecenterTpsCamera(InputAction.CallbackContext _)
        {
            if (_recenteringCoroutine != null || DisableInputs)
                return;

            _recenteringCoroutine = StartCoroutine("RecenterCoroutine");
        }

        /// <summary>
        /// Recentering coroutine
        /// </summary>
        private IEnumerator RecenterCoroutine()
        {
            lookAt.localRotation = Quaternion.Euler(0f, mesh.eulerAngles.y, 0f);
            _tpsCamera.EnableRecentering(true);

            while (!PersonnalUtilities.MathFunctions.ApproximationRange(MainCamTransform.localEulerAngles.y, lookAt.localEulerAngles.y, 1f))
                yield return new WaitForSecondsRealtime(0.05f);

            _tpsCamera.EnableRecentering(false);
            _recenteringCoroutine = null;
        }
        #endregion

        #region StateMachines Methods
        /// <summary>
        /// Handle the GroundStateMachine
        /// </summary>
        private void HandleGroundStateMachine()
        {
            GroundSM.CurrentStatement = _cc.isGrounded || _lowGround ? GroundStateMachine.GroundStatements.Grounded : GroundStateMachine.GroundStatements.Falling;
            PlayerSM.IsRunning = RunCondition();

            switch (GroundSM.CurrentStatement)
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
            switch (PlayerSM.CurrentState)
            {
                case PlayerStateMachine.PlayerStates.Walk:

                    if (!GroundSM.IsLanding)
                        HandleMotion();
                    break;

                case PlayerStateMachine.PlayerStates.Roll:
                    HandleMotionFromAnimations();
                    break;

                case PlayerStateMachine.PlayerStates.Attack:
                    HandleMotionFromAnimations();
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

            _animator.SetFloat("CurrentStateTime", _animator.GetCurrentAnimatorStateInfo(0).normalizedTime);
            _animator.SetBool("IsGrounded", GroundSM.IsThisState(GroundStateMachine.GroundStatements.Grounded) || _lowGround);

            _animator.SetFloat("Inputs", InputsVector.magnitude);
            _animator.SetFloat("DirX", _currentInputs.x);
            _animator.SetFloat("DirY", _currentInputs.y);

            float current = _animator.GetFloat("Motion");
            float target = RunCondition() && CurrentStamina >= 0.1f ? 2f : CurrentSpeed >= overallDatas.walkSpeed ? 1f : InputsVector.magnitude >= 0.1f ? InputsVector.magnitude : 0f;
            float final = Mathf.Lerp(current, target, 0.1f);
            _animator.SetFloat("Motion", final);

            _animator.SetBool("HoldMainAttack", PlayerSM.HoldAttack);
        }

        /// <summary>
        /// Updating layers waight during update
        /// </summary>
        protected void UpdateAnimationLayers()
        {
            float targetWeight = PlayerSM.EnableLayers ? 1f : 0f;
            float currentWeight = _animator.GetLayerWeight(1);
            float updatedWeight = Mathf.Lerp(currentWeight, targetWeight, 0.05f);

            if (PersonnalUtilities.MathFunctions.ApproximationRange(updatedWeight, 0f, 0.05f))
                updatedWeight = 0f;
            else if (PersonnalUtilities.MathFunctions.ApproximationRange(updatedWeight, 1f, 0.05f))
                updatedWeight = 1f;

            SetLowerBodyWeight(updatedWeight);
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
            _currentInputs = Vector2.SmoothDamp(_currentInputs, InputsVector, ref _smoothInputsRef, overallDatas.inputSmoothing);

            UpdateSpeed(GetMovementSpeed());

            Vector3 movement = (orientation.forward * _currentInputs.y + orientation.right * _currentInputs.x) * CurrentSpeed;
            movement.y = -appliedGravity;
            Movement = movement;

            HandlePlayerRotation();
        }

        /// <summary>
        /// Handle the player movement during an animation
        /// </summary>
        public void HandleMotionFromAnimations()
        {
            Movement = GetMeshForward(CurrentSpeed);
        }


        /// <summary>
        /// Conditions if the player can run
        /// </summary>
        protected virtual bool RunCondition()
        {
            if (!GroundSM.IsThisState(GroundStateMachine.GroundStatements.Grounded) && !PlayerSM.IsThisState(PlayerStateMachine.PlayerStates.Walk))
                return false;

            if (PlayerSM.UsingSkill)
                return false;

            return _inputs.actions["Run"].IsPressed() && InputsVector.magnitude >= 0.8f;
        }

        /// <summary>
        /// Lerping the motion speed to a target speed
        /// </summary>
        /// <param name="targetSpeed"> Targeting speed </param>
        public void UpdateSpeed(float targetSpeed)
        {
            CurrentSpeed = Mathf.SmoothDamp(CurrentSpeed, targetSpeed, ref _speedSmoothingRef, overallDatas.speedSmoothing);
        }

        /// <summary>
        /// Return the targeted motion speed
        /// </summary>
        public float GetMovementSpeed()
        {
            if (RunCondition() && CurrentStamina >= 0.1f)
                return overallDatas.runSpeed;
            else if (InputsVector.magnitude >= 0.1f)
                return overallDatas.walkSpeed;

            return 0f;
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

        /// <summary>
        /// Return the override vector multiplied by the speed
        /// </summary>
        /// <param name="speed"> Motion speed </param>
        protected Vector3 GetMeshForward(float speed)
        {
            return new Vector3(mesh.forward.x, -appliedGravity, mesh.forward.z) * speed;
        }

        #region Dodge
        /// <summary>
        /// Roll action callback
        /// </summary>
        private void HandleDodge(InputAction.CallbackContext _)
        {
            if (!DodgeCondition() || DisableInputs)
                return;

            _animator.SetTrigger("Roll");
        }

        /// <summary>
        /// Setting player orientation based on inputs to set the dodge direction
        /// </summary>
        public void SetOrientationToDodge()
        {
            Vector3 newOrientation = InputsVector.magnitude <= 0 ? mesh.forward : orientation.forward * InputsVector.y + orientation.right * InputsVector.x;
            SetPlayerMeshOrientation(newOrientation);
        }

        /// <summary>
        /// Conidition to be able to dodge
        /// </summary>
        private bool DodgeCondition()
        {
            if (!PlayerSM.CanDodge || !GroundSM.IsThisState(GroundStateMachine.GroundStatements.Grounded))
                return false;

            if (CurrentStamina < overallDatas.staminaToDodge)
                return false;

            return !PlayerSM.IsThisState(PlayerStateMachine.PlayerStates.Roll);
        }
        #endregion

        #region Rotations
        /// <summary>
        /// Setting player orientation
        /// </summary>
        protected void SetOrientation()
        {
            orientation.rotation = Quaternion.Euler(0f, _tpsCamera.MainCam.transform.eulerAngles.y, 0f);
        }

        /// <summary>
        /// Handle mesh rotation
        /// </summary>
        protected virtual void HandlePlayerRotation()
        {
            if (!mesh && !PlayerSM.EnableLayers)
                return;

            if (InputsVector.magnitude >= 0.1f)
            {
                float angle = Mathf.Atan2(InputsVector.x, InputsVector.y) * Mathf.Rad2Deg + orientation.eulerAngles.y;
                float smoothAngle = Mathf.SmoothDampAngle(mesh.eulerAngles.y, angle, ref _meshTurnRef, overallDatas.rotationSmoothing);
                mesh.rotation = Quaternion.Euler(0f, smoothAngle, 0f);
            }
        }

        /// <summary>
        /// Handle player rotation during aiming
        /// </summary>
        public void RotateMeshToOrientation()
        {
            if (PlayerSM.IsThisState(PlayerStateMachine.PlayerStates.Attack) || PlayerSM.IsThisState(PlayerStateMachine.PlayerStates.Roll))
                return;

            SetPlayerMeshOrientation(orientation.forward);
        }

        /// <summary>
        /// Turn the player in inputs direction
        /// </summary>
        public void SetPlayerMeshOrientation(Vector3 orientation)
        {
            mesh.rotation = Quaternion.LookRotation(orientation, Vector3.up);
        }
        #endregion

        #endregion

        #region Combat
        /// <summary>
        /// Main attack callback
        /// </summary>
        private void HandleMainAttack(InputAction.CallbackContext _)
        {
            if (!AttackConditions())
                return;

            _animator.SetTrigger("MainAttack");
        }

        /// <summary>
        /// Second attack callback
        /// </summary>
        private void HandleSecondAttack(InputAction.CallbackContext _)
        {
            if (!AttackConditions())
                return;

            _animator.SetTrigger("SecondAttack");
        }

        /// <summary>
        /// Condition to be able to attack
        /// </summary>
        protected virtual bool AttackConditions()
        {
            if (!GroundSM.IsThisState(GroundStateMachine.GroundStatements.Grounded) && !PlayerSM.IsThisState(PlayerStateMachine.PlayerStates.Walk))
                return false;

            return PlayerSM.CanAttack && !PlayerSM.IsThisState(PlayerStateMachine.PlayerStates.Roll);
        }
        #endregion

        #region Character Skill
        /// <summary>
        /// Skill to be able to use his skill
        /// </summary>
        /// <returns></returns>
        protected virtual bool SkillConditions()
        {
            if (!GroundSM.IsThisState(GroundStateMachine.GroundStatements.Grounded) || PlayerSM.IsThisState(PlayerStateMachine.PlayerStates.Attack))
                return false;

            return !PlayerSM.IsThisState(PlayerStateMachine.PlayerStates.Roll);
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
        #region Properties
        public enum GroundStatements { Grounded, Falling }
        public GroundStatements CurrentStatement { get; set; }
        public bool IsLanding { get; set; }
        #endregion

        #region Methods
        public GroundStateMachine()
        {
            CurrentStatement = GroundStatements.Grounded;
        }

        /// <summary>
        /// Return if the target state is the same as the current
        /// </summary>
        /// <param name="targetState"> Target State </param>
        public bool IsThisState(GroundStatements targetState)
        {
            return CurrentStatement == targetState;
        }
        #endregion
    }
}

#endregion

#region PlayerSM_Class

namespace _Scripts.Characters.StateMachines
{
    [Serializable]
    public class PlayerStateMachine
    {
        #region Properties
        public enum PlayerStates { Walk, Roll, Attack, Dead }
        public PlayerStates CurrentState { get; set; }
        public bool IsRunning { get; set; }
        public bool UsingStamina { get; set; }
        public bool CanDodge { get; set; }
        public bool CanAttack { get; set; }
        public bool HoldAttack { get; set; }
        public bool EnableLayers { get; set; }
        public bool UsingSkill { get; set; }
        #endregion

        #region Events
        public event PlayerDeathDelegate OnPlayerDeath;
        public delegate void PlayerDeathDelegate();
        #endregion

        #region Methods
        public PlayerStateMachine()
        {
            CurrentState = PlayerStates.Walk;
            CanAttack = true;
            CanDodge = true;
        }

        /// <summary>
        /// Return if the target state is the same as the current
        /// </summary>
        /// <param name="targetState"> Target State </param>
        public bool IsThisState(PlayerStates targetState)
        {
            return CurrentState == targetState;
        }

        /// <summary>
        /// Invoking the player death event
        /// </summary>
        public void InvokeDeathEvent()
        {
            CurrentState = PlayerStates.Dead;
            OnPlayerDeath?.Invoke();
        }
        #endregion
    }
}

#endregion
