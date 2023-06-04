using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using InputsMaps;
using Photon.Pun;
using Sirenix.OdinInspector;
using _Scripts.Interfaces;
using _Scripts.Characters.StateMachines;
using _ScriptableObjects.Characters;
using _ScriptableObjects.Afflictions;
using Cinemachine;
using _Scripts.Cameras;

namespace _Scripts.Characters
{
    public class Character : TPS_Character, ITrapDamageable, IPlayerDamageable, IPlayerAfflicted, IPlayerStunable
    {
        #region Variables

        #region References
        [FoldoutGroup("Stats")]
        [Required, SerializeField] protected CharactersProperties overallDatas;
        [FoldoutGroup("Stats")]
        [Required, SerializeField] protected AdventurerProperties characterDatas;

        [TitleGroup("Variables")]
        [SerializeField] protected FloatVariable skillCooldown;

        protected AdventurerInputs _inputs;
        #endregion

        #region Character
        public static event Action<Character> OnCharacterDeath;

        private Coroutine _healthRecupRoutine;
        protected Coroutine _skillRoutine;
        public event Action OnSkillUsed;
        public event Action OnSkillRecovered;
        #endregion

        #endregion

        #region Properties
        public CharactersProperties OverallDatas => overallDatas;
        public AdventurerProperties CharacterDatas => characterDatas;
        public PlayerStateMachine PlayerSM { get; private set; }
        public AfflictionStatus CurrentAffliction { get; set; }
        public Transform LookAt => lookAt;
        public TpsCamera Camera => _camera;
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
        }

        public override void OnEnable()
        {
            if (!ViewIsMine())
                return;

            base.OnEnable();
            InitializeCharacter();
        }

        protected override void Update()
        {
            if (!ViewIsMine())
                return;

            base.Update();
            HandleStaminaRecuperation();
        }
        #endregion

        #region Methods
        /// <summary>
        /// Teleport the player to a point
        /// </summary>
        public void TeleportPlayer(Transform teleportPoint)
        {
            gameObject.SetActive(false);
            transform.position = teleportPoint.position;
            transform.rotation = teleportPoint.rotation;
            gameObject.SetActive(true);

            _camera.VCam.Follow = LookAt;
            _camera.VCam.LookAt = LookAt;
        }

        public void TeleportPlayer(Vector3 teleportPoint)
        {
            gameObject.SetActive(false);
            transform.position = teleportPoint;
            gameObject.SetActive(true);

            _camera.VCam.Follow = LookAt;
            _camera.VCam.LookAt = LookAt;
        }

        /// <summary>
        /// Reset the player
        /// </summary>
        protected virtual void InitializeCharacter()
        {
            EnableInputs(true);

            GroundSM = new GroundStateMachine();
            PlayerSM = new PlayerStateMachine();

            CurrentHealth = characterDatas.health;
            CurrentStamina = characterDatas.stamina;
            CurrentAffliction = null;

            RPCCall("ResetAnimatorRPC", RpcTarget.All);
            RPCCall("HealthRPC", RpcTarget.Others, CurrentHealth);

            skillCooldown.value = 0f;
        }

        #region Inputs
        protected override void EnableInputs(bool state)
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
        public void DealDamage(float damages)
        {
            if (!ViewIsMine())
                return;

            HandleEntityHealth(damages);
        }

        public void KnockbackDamages(float damages, Vector3 hitPoint)
        {
            if (!ViewIsMine())
                return;

            HandleEntityHealth(damages);
            HardHit(hitPoint);
        }

        public void TrapDamages(float damages)
        {
            if (!ViewIsMine())
                return;

            HandleEntityHealth(damages);
        }

        public void TouchedByAffliction(AfflictionStatus status)
        {
            //Not my character
            if (!ViewIsMine() || CurrentAffliction)
                return;

            //Status
            Debug.LogWarning($"Affected by : {status}");
            CurrentAffliction = status;
            StartCoroutine("AfflictionRoutine");
        }

        public void Stunned(float duration)
        {
            if (!ViewIsMine() || !GroundSM.IsStateOf(GroundStateMachine.GroundStatements.Grounded))
                return;

            if (PlayerSM.IsStateOf(PlayerStateMachine.PlayerStates.Dead) || PlayerSM.IsStateOf(PlayerStateMachine.PlayerStates.Knocked))
                return;

            StartCoroutine(StunRoutine(duration));
        }
        #endregion

        #region Health Methods
        protected override void HandleEntityHealth(float damages)
        {
            if (PlayerSM.IsStateOf(PlayerStateMachine.PlayerStates.Knocked) || PlayerSM.IsStateOf(PlayerStateMachine.PlayerStates.Dead))
                return;

            CurrentHealth = ClampedHealth(damages, 0f, Mathf.Infinity);
            RPCCall("HealthRPC", RpcTarget.Others, CurrentHealth);

            if (CurrentHealth > 0)
            {
                if (_healthRecupRoutine != null)
                    StopCoroutine(_healthRecupRoutine);

                _healthRecupRoutine = StartCoroutine(HealthRecuperation());
                return;
            }

            HandleEntityDeath();
        }

        [ContextMenu("Death")]
        protected override void HandleEntityDeath()
        {
            base.HandleEntityDeath();
            InvokeDeathEvent();

            StopAllCoroutines();

            if (CurrentAffliction != null)
                CurrentAffliction = null;
        }

        /// <summary>
        /// Loop until the life is full, stopped if the character takes a hit
        /// </summary>
        private IEnumerator HealthRecuperation()
        {
            yield return new WaitForSecondsRealtime(overallDatas.healthRecupDelay);

            while (CurrentHealth < characterDatas.health)
            {
                CurrentHealth += overallDatas.healthRecup * Time.deltaTime;
                yield return null;
            }

            CurrentHealth = ClampedHealth(0f, 0f, characterDatas.health);
            _healthRecupRoutine = null;
        }

        /// <summary>
        /// Invoking the player death event
        /// </summary>
        private void InvokeDeathEvent()
        {
            PlayerSM.CurrentState = PlayerStateMachine.PlayerStates.Dead;
            OnCharacterDeath?.Invoke(this);
        }
        #endregion

        #region Stamina Methods
        /// <summary>
        /// Handle stamina recuperation
        /// </summary>
        protected void HandleStaminaRecuperation()
        {
            PlayerSM.UsingStamina = PlayerSM.IsStateOf(PlayerStateMachine.PlayerStates.Roll) || RunConditions();

            if (PlayerSM.UsingStamina || CurrentStamina > characterDatas.stamina)
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
        /// Affliction effect duration
        /// </summary>
        /// <returns></returns>
        private IEnumerator AfflictionRoutine()
        {
            float time = 0f;
            float duration = CurrentAffliction.duration;

            while (time < duration)
            {
                time += Time.deltaTime;
                CurrentAffliction.UpdateEffect(this);
                yield return null;
            }

            CurrentAffliction = null;
        }

        private IEnumerator StunRoutine(float duration)
        {
            RPCAnimatorTrigger(RpcTarget.All, "stunned", true);
            yield return new WaitForSecondsRealtime(duration);
            RPCAnimatorTrigger(RpcTarget.All, "resetStun", true);
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

                    SmoothingInputs(Inputs, overallDatas.inputSmoothing);
                    UpdateCharacterSpeed(GetMovementSpeed());

                    if (PlayerSM.CanMove)
                    {
                        HandleCharacterMotion();
                        HandleCharacterRotation(overallDatas.rotationSmoothing);
                    }
                    break;
            }
        }
        #endregion

        #region Animation Methods
        /// <summary>
        /// Set player animations
        /// </summary>
        protected override void UpdateAnimations()
        {
            if (!Animator || PlayerSM.IsStateOf(PlayerStateMachine.PlayerStates.Stunned))
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
            if (PlayerSM.IsStateOf(PlayerStateMachine.PlayerStates.Stunned))
                return;

            float targetWeight = PlayerSM.EnableLayers ? 1f : 0f;
            float currentWeight = Animator.GetLayerWeight(1);
            float updatedWeight = Mathf.Lerp(currentWeight, targetWeight, 0.05f);

            if (Utils.Utilities.Math.ApproximationRange(updatedWeight, 0f, 0.05f))
                updatedWeight = 0f;
            else if (Utils.Utilities.Math.ApproximationRange(updatedWeight, 1f, 0.05f))
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
        public virtual float GetMovementSpeed()
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
        protected override void HandleCharacterRotation(float smooth)
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
                float smoothAngle = Mathf.SmoothDampAngle(mesh.eulerAngles.y, angle, ref _smoothMeshTurnRef, smooth);
                mesh.rotation = Quaternion.Euler(0f, smoothAngle, 0f);
            }
        }

        public override void UpdateCharacterSpeed(float speed)
        {
            CurrentSpeed = Mathf.SmoothDamp(CurrentSpeed, speed, ref _smoothSpeedRef, overallDatas.speedSmoothing);
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
            if (!PlayerSM.CanDodge || !GroundSM.IsStateOf(GroundStateMachine.GroundStatements.Grounded) || PlayerSM.IsStateOf(PlayerStateMachine.PlayerStates.Stunned))
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

            if (PlayerSM.WaitAttack != null || PlayerSM.IsStateOf(PlayerStateMachine.PlayerStates.Stunned))
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
            _skillRoutine = StartCoroutine(SkillCooldownRoutine());
        }

        /// <summary>
        /// Start the cooldown to recover the character skill
        /// </summary>
        private IEnumerator SkillCooldownRoutine()
        {
            skillCooldown.value = characterDatas.skillCooldown;

            while (skillCooldown.value > 0)
            {
                skillCooldown.value -= Time.deltaTime;
                yield return null;
            }

            skillCooldown.value = 0f;
            _skillRoutine = null;
            PlayerSM.SkillUsed = false;
            //Skill recovered
            OnSkillRecovered?.Invoke();
        }

        /// <summary>
        /// Skill to be able to use his skill
        /// </summary>
        /// <returns></returns>
        protected virtual bool SkillConditions()
        {
            if (PlayerSM.SkillUsed || !GroundSM.IsStateOf(GroundStateMachine.GroundStatements.Grounded))
                return false;

            return PlayerSM.IsStateOf(PlayerStateMachine.PlayerStates.Walk);
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
        public enum PlayerStates { Walk, Roll, Attack, Knocked, Stunned, Dead }
        public PlayerStates CurrentState { get; set; }
        public bool UsingStamina { get; set; }
        public bool CanMove { get; set; }
        public bool CanDodge { get; set; }
        public bool CanAttack { get; set; }
        public Coroutine WaitAttack { get; set; }
        public bool HoldAttack { get; set; }
        public bool EnableLayers { get; set; }
        public bool SkillUsed { get; set; } = false;
        #endregion

        #region Methods
        public PlayerStateMachine()
        {
            CurrentState = PlayerStates.Walk;
            CanMove = true;
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
