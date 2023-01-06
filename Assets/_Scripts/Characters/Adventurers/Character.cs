using System;
using System.Collections;
using Photon.Pun;
using UnityEngine;
using InputsMaps;
using UnityEngine.InputSystem;
using _Scripts.Utilities.Florian;
using _Scripts.Managers;
using _Scripts.Interfaces;
using _Scripts.Characters.StateMachines;
using _ScriptableObjects.Adventurers;
using _ScriptableObjects.Afflictions;
using _Scripts.UI.Interfaces;

namespace _Scripts.Characters
{
    public class Character : TPS_Character, ITrapDamageable, IPlayerDamageable, IPlayerAfflicted
    {
        #region Variables

        #region References
        [Header("Stats references")]
        [SerializeField] protected CharactersOverallDatas overallDatas;
        [SerializeField] protected AdventurerDatas characterDatas;

        [Header("UI references")]
        [SerializeField] private PlayerHUD hud;

        protected AdventurerInputs _inputs;
        #endregion

        #region Character
        public static event Action OnCharacterDeath;

        private Coroutine _healthRecupRoutinee;
        private Coroutine _afflictionRoutine;

        protected Coroutine _skillCoroutine;
        public event Action OnSkillUsed;
        public event Action OnSkillRecovered;
        #endregion

        #endregion

        #region Properties
        public CharactersOverallDatas OverallDatas => overallDatas;
        public AdventurerDatas CharacterDatas => characterDatas;
        public PlayerStateMachine PlayerSM { get; private set; }
        public AfflictionStatus CurrentAffliction { get; set; }
        public Transform LookAt { get; private set; }
        public float CurrentStamina { get; set; }
        public float AirTime => _airTime;
        #endregion

        #region Builts_In
        public override void Awake()
        {
            base.Awake();

            if (!ViewIsMine())
                return;

            _inputs = new AdventurerInputs();
            InstantiateHUD();
        }

        public override void OnEnable()
        {
            if (!ViewIsMine())
                return;

            base.OnEnable();
            InitializeCharacter();
            GameUIManager.Instance.OnOptionsMenuChanged += ctx => InputsEnabled(!ctx);
        }

        public override void OnDisable()
        {
            if (!ViewIsMine())
                return;

            base.OnDisable();
            GameUIManager.Instance.OnOptionsMenuChanged -= ctx => InputsEnabled(!ctx);
        }

        public virtual void Update()
        {
            if (!ViewIsMine())
                return;

            SetOrientation();
            HandleGroundStateMachine();
            HandleStaminaRecuperation();
            UpdateAnimations();
        }
        #endregion

        #region Temporary
        private void InstantiateHUD()
        {
            if (!hud)
                return;

            PlayerHUD HUD = Instantiate(hud);
            HUD.SetTargetCharacter(this);
        }
        #endregion

        #region Methods
        /// <summary>
        /// Teleport the player to a point
        /// </summary>
        public void GetTeleported(Vector3 position)
        {
            InputsEnabled(false);
            ResetCharacterVelocity();

            transform.position = position;
            InitializeCharacter();
        }

        /// <summary>
        /// Reset the player
        /// </summary>
        protected virtual void InitializeCharacter()
        {
            InputsEnabled(true);

            GroundSM = new GroundStateMachine();
            PlayerSM = new PlayerStateMachine();

            CurrentHealth = characterDatas.health;
            CurrentStamina = characterDatas.stamina;
            CurrentAffliction = null;

            RPCCall("ResetAnimatorRPC", RpcTarget.All);
        }

        #region Inputs
        protected override void InputsEnabled(bool state)
        {
            if (state)
                _inputs.Enable();
            else
                _inputs.Disable();
        }

        /// <summary>
        /// Subscribe Player actions to methods
        /// </summary>
        protected override void SubscribeInputActions()
        {
            _inputs.Gameplay.Move.performed += ctx => Inputs = ctx.ReadValue<Vector2>();
            _inputs.Gameplay.Move.canceled += ctx => Inputs = Vector2.zero;
            _inputs.Gameplay.Roll.started += HandleDodge;

            _inputs.Gameplay.MainAttack.started += HandleMainAttack;
            _inputs.Gameplay.SecondAttack.started += HandleSecondAttack;
            _inputs.Gameplay.MainAttack.performed += ctx => PlayerSM.HoldAttack = ctx.ReadValueAsButton();
            _inputs.Gameplay.MainAttack.canceled += ctx => PlayerSM.HoldAttack = ctx.ReadValueAsButton();
        }

        /// <summary>
        /// Unsubscribe Player actions to methods
        /// </summary>
        protected override void UnsubscribeInputActions()
        {
            _inputs.Disable();

            _inputs.Gameplay.Move.performed -= ctx => Inputs = ctx.ReadValue<Vector2>();
            _inputs.Gameplay.Move.canceled -= ctx => Inputs = Vector2.zero;
            _inputs.Gameplay.Roll.started -= HandleDodge;

            _inputs.Gameplay.MainAttack.started -= HandleMainAttack;
            _inputs.Gameplay.SecondAttack.started -= HandleSecondAttack;
            _inputs.Gameplay.MainAttack.performed -= ctx => PlayerSM.HoldAttack = ctx.ReadValueAsButton();
            _inputs.Gameplay.MainAttack.canceled -= ctx => PlayerSM.HoldAttack = ctx.ReadValueAsButton();
        }
        #endregion

        #region Interfaces Implementations
        public void SoftDamages(float damages)
        {
            HandleEntityHealth(damages);
        }

        public void HardDamages(float damages, Vector3 hitPoint)
        {
            HandleEntityHealth(damages);
            HardHit(hitPoint);
        }

        public void TrapDamages(float damages)
        {
            HandleEntityHealth(damages);
        }

        public void TouchedByAffliction(AfflictionStatus status)
        {
            if (CurrentAffliction != null)
                return;

            StartAfflictionEffect(status);
        }
        #endregion

        #region Health Methods
        protected override void HandleEntityHealth(float damages)
        {
            if (!ViewIsMine() || PlayerSM.IsStateOf(PlayerStateMachine.PlayerStates.Knocked) || PlayerSM.IsStateOf(PlayerStateMachine.PlayerStates.Dead))
                return;

            CurrentHealth = ClampedHealth(damages, 0f, Mathf.Infinity);
            RPCCall("HealthRPC", RpcTarget.Others, CurrentHealth);

            if (CurrentHealth > 0)
            {
                if (_healthRecupRoutinee != null)
                    StopCoroutine(_healthRecupRoutinee);

                _healthRecupRoutinee = StartCoroutine(HealthRecuperation());
                return;
            }

            HandleEntityDeath();
        }

        protected override void HandleEntityDeath()
        {
            base.HandleEntityDeath();
            InvokeDeathEvent();

            if (_healthRecupRoutinee != null)
                StopCoroutine(_healthRecupRoutinee);

            if (_skillCoroutine != null)
                StopCoroutine(_skillCoroutine);

            if (_afflictionRoutine != null)
                StopCoroutine(_afflictionRoutine);

            if (CurrentAffliction != null)
                CurrentAffliction = null;
        }

        /// <summary>
        /// Loop until the life is full, stopped if the character takes a hit
        /// </summary>
        private IEnumerator HealthRecuperation()
        {
            yield return new WaitForSecondsRealtime(overallDatas.healthRecupTime);

            while (CurrentHealth < characterDatas.health)
            {
                CurrentHealth += overallDatas.healthRecup * Time.deltaTime;
                yield return null;
            }

            CurrentHealth = ClampedHealth(0f, 0f, characterDatas.health);
            _healthRecupRoutinee = null;
        }

        /// <summary>
        /// Invoking the player death event
        /// </summary>
        private void InvokeDeathEvent()
        {
            PlayerSM.CurrentState = PlayerStateMachine.PlayerStates.Dead;
            OnCharacterDeath?.Invoke();
        }
        #endregion

        #region Stamina Methods
        /// <summary>
        /// Handle stamina recuperation
        /// </summary>
        protected void HandleStaminaRecuperation()
        {
            PlayerSM.UsingStamina = PlayerSM.IsStateOf(PlayerStateMachine.PlayerStates.Roll) || RunConditions();

            if (!PlayerSM.UsingStamina || CurrentStamina > characterDatas.stamina)
                return;

            CurrentStamina += overallDatas.staminaRecup * Time.deltaTime;
            CurrentStamina = Mathf.Clamp(CurrentStamina, 0f, characterDatas.stamina);
        }

        /// <summary>
        /// using stamina
        /// </summary>
        /// <param name="amount"> amount of stamina used </param>
        public void UsingStamina(float amount)
        {
            if (!ViewIsMine())
                return;

            CurrentStamina -= amount;
        }
        #endregion

        #region Affliction Methods
        /// <summary>
        /// Apply a new affliction on the player if there isn't one
        /// </summary>
        /// <param name="status"></param>
        private void StartAfflictionEffect(AfflictionStatus status)
        {
            CurrentAffliction = status;
            Debug.LogWarning(status);
            _afflictionRoutine = StartCoroutine("AfflictionRoutine");
        }

        /// <summary>
        /// Affliction effect duration
        /// </summary>
        /// <returns></returns>
        private IEnumerator AfflictionRoutine()
        {
            float time = 0f;
            float duration = CurrentAffliction.Duration;

            while (time < duration)
            {
                time += Time.deltaTime;
                CurrentAffliction.UpdateEffect(this);
                yield return null;
            }

            CurrentAffliction = null;
            _afflictionRoutine = null;
        }
        #endregion

        #region StateMachines Methods
        /// <summary>
        /// Handle the PlayerStateMachine
        /// </summary>
        protected override void HandleCharacterStateMachine()
        {
            switch (PlayerSM.CurrentState)
            {
                case PlayerStateMachine.PlayerStates.Walk:

                    if (GroundSM.IsLanding)
                        return;

                    SmoothingInputs(Inputs, inputSmoothing);
                    UpdateCharacterSpeed(GetMovementSpeed());
                    HandleCharacterMotion();
                    HandleCharacterRotation();
                    break;
            }
        }
        #endregion

        #region Animation Methods
        /// <summary>
        /// Set player animations
        /// </summary>
        protected virtual void UpdateAnimations()
        {
            if (!Animator)
                return;

            Animator.SetFloat("CurrentStateTime", Animator.GetCurrentAnimatorStateInfo(0).normalizedTime);
            Animator.SetBool("IsGrounded", GroundSM.IsStateOf(GroundStateMachine.GroundStatements.Grounded));

            Animator.SetFloat("Inputs", Inputs.magnitude);
            Animator.SetFloat("DirX", _currentInputs.x);
            Animator.SetFloat("DirY", _currentInputs.y);

            float current = Animator.GetFloat("Motion");
            float target = RunConditions() && CurrentStamina >= 0.1f ? 2f : CurrentSpeed >= overallDatas.walkSpeed ? 1f : Inputs.magnitude >= 0.1f ? Inputs.magnitude : 0f;
            float final = Mathf.Lerp(current, target, 0.1f);
            Animator.SetFloat("Motion", final);

            Animator.SetBool("HoldMainAttack", PlayerSM.HoldAttack && PlayerSM.CanAttack);
        }

        /// <summary>
        /// Updating layers waight during update
        /// </summary>
        protected void UpdateAnimationLayers()
        {
            float targetWeight = PlayerSM.EnableLayers ? 1f : 0f;
            float currentWeight = Animator.GetLayerWeight(1);
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
            Animator.SetLayerWeight(1, value);
        }

        /// <summary>
        /// By taking a strong hit, the hit creates a knockback
        /// </summary>
        private void HardHit(Vector3 hitPoint)
        {
            if (!GroundSM.IsStateOf(GroundStateMachine.GroundStatements.Grounded))
                return;

            Vector3 dir = (hitPoint - transform.position).normalized;
            dir.y = 0f;

            SetMeshOrientation(dir);
            RPCAnimatorTrigger(RpcTarget.All, "Knockback", true);
        }
        #endregion

        #region Motion Methods
        /// <summary>
        /// Conditions if the player can run
        /// </summary>
        public virtual bool RunConditions()
        {
            if (!GroundSM.IsStateOf(GroundStateMachine.GroundStatements.Grounded) && !PlayerSM.IsStateOf(PlayerStateMachine.PlayerStates.Walk))
                return false;

            if (PlayerSM.EnableLayers)
                return false;

            return Inputs.magnitude >= 0.8 && _inputs.Gameplay.Run.IsPressed();
        }

        /// <summary>
        /// Return the current target motion speed
        /// </summary>
        public float GetMovementSpeed()
        {
            if (RunConditions() && CurrentStamina >= 0.1f)
                return overallDatas.runSpeed;
            else if (Inputs.magnitude >= 0.1f)
                return overallDatas.walkSpeed;

            return 0f;
        }

        /// <summary>
        /// Setting mesh rotations based on current inputs
        /// </summary>
        protected override void HandleCharacterRotation()
        {
            if (!mesh)
                return;

            if (PlayerSM.EnableLayers)
            {
                LookTowardsOrientation();
                return;
            }

            if (Inputs.magnitude >= 0.1f)
            {
                float angle = Mathf.Atan2(Inputs.x, Inputs.y) * Mathf.Rad2Deg + orientation.eulerAngles.y;
                float smoothAngle = Mathf.SmoothDampAngle(mesh.eulerAngles.y, angle, ref _smoothMeshTurnRef, overallDatas.rotationSmoothing);
                mesh.rotation = Quaternion.Euler(0f, smoothAngle, 0f);
            }
        }
        #endregion

        #region Dodge Methods
        /// <summary>
        /// Roll action callback
        /// </summary>
        private void HandleDodge(InputAction.CallbackContext _)
        {
            if (!DodgeCondition())
                return;

            RPCAnimatorTrigger(RpcTarget.All, "Roll", true);
        }

        /// <summary>
        /// Setting player orientation based on inputs to set the dodge direction
        /// </summary>
        public void SetOrientationToDodge()
        {
            Vector3 newOrientation = Inputs.magnitude <= 0 ? mesh.forward : orientation.forward * Inputs.y + orientation.right * Inputs.x;
            SetMeshOrientation(newOrientation);
        }

        /// <summary>
        /// Condition to be able to dodge
        /// </summary>
        private bool DodgeCondition()
        {
            if (!PlayerSM.CanDodge || !GroundSM.IsStateOf(GroundStateMachine.GroundStatements.Grounded))
                return false;

            if (CurrentStamina < overallDatas.staminaToDodge)
                return false;

            return !PlayerSM.IsStateOf(PlayerStateMachine.PlayerStates.Roll);
        }
        #endregion

        #region Combat Methods
        /// <summary>
        /// Main attack callback
        /// </summary>
        private void HandleMainAttack(InputAction.CallbackContext _)
        {
            if (!AttackConditions())
                return;

            PlayerSM.WaitAttack = StartCoroutine("AttackWaitRoutine");
            RPCAnimatorTrigger(RpcTarget.All, "MainAttack", true);
        }

        /// <summary>
        /// Second attack callback
        /// </summary>
        private void HandleSecondAttack(InputAction.CallbackContext _)
        {
            if (!AttackConditions())
                return;

            PlayerSM.WaitAttack = StartCoroutine("AttackWaitRoutine");
            RPCAnimatorTrigger(RpcTarget.All, "SecondAttack", true);
        }

        /// <summary>
        /// Condition to be able to attack
        /// </summary>
        protected virtual bool AttackConditions()
        {
            if (!GroundSM.IsStateOf(GroundStateMachine.GroundStatements.Grounded) && !PlayerSM.IsStateOf(PlayerStateMachine.PlayerStates.Walk))
                return false;

            if (PlayerSM.WaitAttack != null)
                return false;

            return PlayerSM.CanAttack && !PlayerSM.IsStateOf(PlayerStateMachine.PlayerStates.Roll);
        }

        /// <summary>
        /// Safety to combat spamming
        /// </summary>
        protected IEnumerator AttackWaitRoutine()
        {
            yield return new WaitForSecondsRealtime(0.1f);

            PlayerSM.WaitAttack = null;
        }
        #endregion

        #region Character Skill Methods
        /// <summary>
        /// Start skill cooldown
        /// </summary>
        public void SkillUsed()
        {
            if (!ViewIsMine())
                return;

            OnSkillUsed?.Invoke();
            _skillCoroutine = StartCoroutine(SkillCooldownRoutine());
        }

        /// <summary>
        /// Start the cooldown to recover the character skill
        /// </summary>
        private IEnumerator SkillCooldownRoutine()
        {
            yield return new WaitForSecondsRealtime(characterDatas.skillCooldown);

            _skillCoroutine = null;
            OnSkillRecovered?.Invoke();
        }

        /// <summary>
        /// Skill to be able to use his skill
        /// </summary>
        /// <returns></returns>
        protected virtual bool SkillConditions()
        {
            if (_skillCoroutine != null || !GroundSM.IsStateOf(GroundStateMachine.GroundStatements.Grounded) || PlayerSM.IsStateOf(PlayerStateMachine.PlayerStates.Attack))
                return false;

            return !PlayerSM.IsStateOf(PlayerStateMachine.PlayerStates.Roll);
        }
        #endregion

        #region Physics Methods
        /// <summary>
        /// Reset player airTime
        /// </summary>
        public void ResetAirTime()
        {
            _airTime = 0f;
        }
        #endregion

        #endregion
    }
}

#region PlayerSM_Class

namespace _Scripts.Characters.StateMachines
{
    [Serializable]
    public class PlayerStateMachine
    {
        #region Properties
        public enum PlayerStates { Walk, Roll, Attack, Knocked, OffBalanced, Dead }
        public PlayerStates CurrentState { get; set; }
        public bool UsingStamina { get; set; }
        public bool CanDodge { get; set; }
        public bool CanAttack { get; set; }
        public Coroutine WaitAttack { get; set; }
        public bool HoldAttack { get; set; }
        public bool EnableLayers { get; set; }
        #endregion

        #region Methods
        public PlayerStateMachine()
        {
            CurrentState = PlayerStates.Walk;
            CanAttack = true;
            CanDodge = true;
            WaitAttack = null;
        }

        /// <summary>
        /// Return if the target state is the same as the current
        /// </summary>
        /// <param name="targetState"> Target State </param>
        public bool IsStateOf(PlayerStates targetState)
        {
            return CurrentState == targetState;
        }
        #endregion
    }
}

#endregion
