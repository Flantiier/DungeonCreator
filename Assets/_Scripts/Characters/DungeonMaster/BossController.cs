using System;
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

namespace _Scripts.Characters.DungeonMaster
{
    public class BossController : TPS_Character, IDamageable
    {
        #region Variables        
        public enum BossState { Walk, Attack }

        [FoldoutGroup("Stats")]
        [Required, SerializeField] private BossProperties datas;

        private BossInputs _inputs;
        //Events
        public static event Action OnBossDefeated;
        #endregion

        #region Properties
        public BossState StateMachine { get; private set; }
        public bool CanAttack { get; set; } = true;
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

        public override void OnEnable() 
        {
            SubscribeInputActions();
        }

        public override void OnDisable()
        {
            UnsubscribeInputActions();
            _inputs.Disable();
        }
        #endregion

        #region Methods
        public void EnableBoss()
        {
            _camera.gameObject.SetActive(true);
            _inputs.Enable();
        }

        #region Health
        protected override void HandleEntityDeath()
        {
            base.HandleEntityDeath();
            //Call the event of the boss is defeated
            OnBossDefeated?.Invoke();
        }

        public void Damage(float damages)
        {
            if (CurrentHealth <= 0)
                return;

            HandleEntityHealth(damages);
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

            Animator.SetFloat("motion", Inputs.magnitude);
        }
        #endregion

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
            CurrentSpeed = datas.walkSpeed;
            base.HandleCharacterMotion();
        }

        private float GetMovementSpeed()
        {
            if (Inputs.magnitude >= 0.1f)
                return datas.walkSpeed;

            return 0f;
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
        #endregion

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
        public IEnumerator AbilityRoutine(float cooldown)
        {
            Available = false;
            yield return new WaitForSecondsRealtime(cooldown);
            Available = true;
        }
    }
}
#endregion