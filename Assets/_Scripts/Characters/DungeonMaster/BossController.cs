using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using InputsMaps;
using Photon.Pun;
using Sirenix.OdinInspector;
using _ScriptableObjects.Characters;
//
using _Scripts.Interfaces;
using _Scripts.Characters.StateMachines;
using _Scripts.Managers;

namespace _Scripts.Characters.DungeonMaster
{
    public class BossController : TPS_Character, IDamageable
    {
        #region Variables        
        public enum BossState { Walk, Attack }

        [FoldoutGroup("Stats")]
        [Required, SerializeField] private BossProperties datas;

        private BossInputs _inputs;
        private bool _isRunning;
        private bool _isEnabled = false;
        #endregion

        #region Properties
        public BossState StateMachine { get; private set; }
        public bool CanAttack { get; set; } = true;
        public BossProperties Datas => datas;
        public float Stamina { get; private set; }
        public Ability FirstAbility { get; private set; }
        public Ability SecondAbility { get; private set; }
        #endregion

        #region Builts_In
        public override void Awake()
        {
            base.Awake();

            if (!ViewIsMine())
                return;

            _inputs = new BossInputs();
            StateMachine = new BossState();

            //Init class
            FirstAbility = new Ability();
            SecondAbility = new Ability();
            //Camera
            _camera.gameObject.SetActive(false);
        }

        private void Start()
        {
            SetHealth();
            Stamina = datas.stamina;
        }

        public override void OnEnable()
        {
            if (!ViewIsMine())
                return;

            SubscribeInputActions();
            GameUIManager.OnMenuOpen += EnableInputs;
        }

        public override void OnDisable()
        {
            if (!ViewIsMine())
                return;

            GameUIManager.OnMenuOpen -= EnableInputs;
            UnsubscribeInputActions();
            _inputs.Disable();
        }

        protected override void Update()
        {
            if (!ViewIsMine() || CurrentHealth <= 0)
                return;

            _isRunning = _inputs.Gameplay.Run.IsPressed() && StateMachine == BossState.Walk;

            base.Update();
            HandleStaminaRecuperation();
        }
        #endregion

        #region Methods
        public void EnableBoss()
        {
            _camera.gameObject.SetActive(true);
            _inputs.Enable();
        }

        public void EnableBossDamage(bool enabled)
        {
            _isEnabled = enabled;
        }

        #region Health
        public void Damage(float damages)
        {
            if (CurrentHealth <= 0 || !_isEnabled)
                return;

            HandleEntityHealth(damages);
        }

        [ContextMenu("Death")]
        protected override void HandleEntityDeath()
        {
            if (!ViewIsMine())
                return;

            _inputs.Disable();
            RPCAnimatorTrigger(RpcTarget.AllBuffered, "Death", true); ;
        }

        /// <summary>
        /// Set character and send it over network
        /// </summary>
        private void SetHealth()
        {
            CurrentHealth = datas.health;
            RPCCall("HealthRPC", RpcTarget.OthersBuffered, CurrentHealth);
        }
        #endregion

        #region StateMachine Methods
        /// <summary>
        /// Set boss StateMachine State
        /// </summary>
        /// <param name="state"></param>
        public void SetBossState(BossState state)
        {
            StateMachine = state;
        }

        protected override void UpdateAnimations()
        {
            if (!Animator)
                return;

            float target = Inputs.magnitude >= 0.2f && (_isRunning && Stamina > 0) ? 2f : Inputs.magnitude;
            Animator.SetFloat("motion", Mathf.Lerp(Animator.GetFloat("motion"), target, 0.05f));
        }
        #endregion

        #region Inputs
        protected override void EnableInputs(bool state)
        {
            if (!ViewIsMine() || !_isEnabled)
                return;

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
            _inputs.Gameplay.Motion.performed += ctx => Inputs = ctx.ReadValue<Vector2>();
            _inputs.Gameplay.Motion.canceled += _ => Inputs = Vector2.zero;
            //Combat
            _inputs.Gameplay.Attack.started += HandleAttack;
            _inputs.Gameplay.Ability01.started += HandleFirstAbility;
            _inputs.Gameplay.Ability02.started += HandleSecondAbility;
        }

        /// <summary>
        /// Unsubscribe Player actions to methods
        /// </summary>
        protected override void UnsubscribeInputActions()
        {
            _inputs.Gameplay.Motion.performed -= ctx => Inputs = ctx.ReadValue<Vector2>();
            _inputs.Gameplay.Motion.canceled -= _ => Inputs = Vector2.zero;
            //Combat
            _inputs.Gameplay.Attack.started -= HandleAttack;
            _inputs.Gameplay.Ability01.started -= HandleFirstAbility;
            _inputs.Gameplay.Ability02.started -= HandleSecondAbility;
        }
        #endregion

        #region Motion
        protected override void HandleCharacterStateMachine()
        {
            switch (StateMachine)
            {
                case BossState.Walk:
                    SmoothingInputs(Inputs, datas.inputSmoothing);
                    UpdateCharacterSpeed(GetMovementSpeed());
                    HandleCharacterMotion();
                    HandleCharacterRotation(datas.rotationSmoothing);
                    break;
            }
        }

        protected override void HandleCharacterMotion()
        {
            CurrentSpeed = Mathf.Lerp(CurrentSpeed, GetMovementSpeed(), datas.speedSmoothing);
            base.HandleCharacterMotion();
        }

        private float GetMovementSpeed()
        {
            if (Inputs.magnitude >= 0.2f && (_isRunning && Stamina > 0))
                return datas.runSpeed;
            else if (Inputs.magnitude >= 0.1f)
                return datas.walkSpeed;

            return 0f;
        }
        #endregion

        #region Stamina Methods
        /// <summary>
        /// Handle stamina recuperation
        /// </summary>
        protected void HandleStaminaRecuperation()
        {
            if (_isRunning)
            {
                Stamina -= datas.usedStamina * Time.deltaTime;
                Stamina = Mathf.Clamp(Stamina, 0f, datas.stamina);
                return;
            }

            Stamina += datas.staminaRecup * Time.deltaTime;
            Stamina = Mathf.Clamp(Stamina, 0f, datas.stamina);
        }

        /// <summary>
        /// using stamina
        /// </summary>
        /// <param name="amount"> amount of stamina used </param>
        public void UsingStamina(float amount)
        {
            if (!ViewIsMine())
                return;

            Stamina -= amount;
        }
        #endregion

        #region Combat
        /// <summary>
        /// Attack trigger
        /// </summary>
        private void HandleAttack(InputAction.CallbackContext _)
        {
            if (!AttackConditions())
                return;

            RPCAnimatorTrigger(RpcTarget.All, "attack", true);
        }

        /// <summary>
        /// Indicates if the character can attack
        /// </summary>
        /// <returns></returns>
        private bool AttackConditions()
        {
            if (!GroundSM.IsStateOf(GroundStateMachine.GroundStatements.Grounded) && StateMachine == BossState.Walk)
                return false;

            return CanAttack;
        }
        #endregion

        #region Abilities
        /// <summary>
        /// First ability handler
        /// </summary>
        private void HandleFirstAbility(InputAction.CallbackContext _)
        {
            if (!AttackConditions() || !FirstAbility.Available)
                return;

            RPCAnimatorTrigger(RpcTarget.All, "ability01", true);
        }

        /// <summary>
        /// Second ability handler
        /// </summary>
        private void HandleSecondAbility(InputAction.CallbackContext _)
        {
            if (!AttackConditions() || !SecondAbility.Available)
                return;

            RPCAnimatorTrigger(RpcTarget.All, "ability02", true);
        }

        /// <summary>
        /// Start the cooldown of an ability
        /// </summary>
        public void AbilityUsed(int index)
        {
            if (index <= 0)
                StartCoroutine(FirstAbility.AbilityRoutine(datas.firstAbilityRecovery));
            else
                StartCoroutine(SecondAbility.AbilityRoutine(datas.secondAbilityRecovery));
        }
        #endregion*

        #endregion
    }
}

#region Ability_Class
namespace _Scripts.Characters
{
    [System.Serializable]
    public class Ability
    {
        public bool Available { get; private set; } = true;
        public float Cooldown { get; private set; }
        public IEnumerator AbilityRoutine(float cooldown)
        {
            Available = false;
            Cooldown = cooldown;

            while (Cooldown > 0)
            {
                Cooldown -= Time.deltaTime;
                yield return null;
            }

            Cooldown = 0f;
            Available = true;
        }
    }
}
#endregion