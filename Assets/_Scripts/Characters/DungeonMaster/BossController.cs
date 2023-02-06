using UnityEngine;
using InputsMaps;
using Photon.Pun;
using Sirenix.OdinInspector;
using _ScriptableObjects.DM;

namespace _Scripts.Characters.DungeonMaster
{
    public class BossController : TPS_Character
    {
        #region Variables        
        public enum BossState { Walk, Attack }

        [FoldoutGroup("Stats")]
        [Required, SerializeField] private BossDatas datas;

        private BossInputs _inputs;
        #endregion

        #region Properties
        public BossState StateMachine { get; private set; }
        #endregion

        #region Builts_In
        public override void Awake()
        {
            base.Awake();

            if (!ViewIsMine())
                return;

            _inputs = new BossInputs();
            StateMachine = new BossState();
        }

        protected override void Update()
        {
            if (!ViewIsMine())
                return;

            base.Update();
            UpdateAnimations();
        }
        #endregion

        #region Methods
        private void UpdateAnimations()
        {
            if (!Animator)
                return;

            Animator.SetFloat("motion", Inputs.magnitude);
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
            _inputs.Gameplay.Motion.performed += ctx => Inputs = ctx.ReadValue<Vector2>();
            _inputs.Gameplay.Motion.canceled += _ => Inputs = Vector2.zero;
        }

        /// <summary>
        /// Unsubscribe Player actions to methods
        /// </summary>
        protected override void UnsubscribeInputActions()
        {
            _inputs.Gameplay.Motion.performed -= ctx => Inputs = ctx.ReadValue<Vector2>();
            _inputs.Gameplay.Motion.canceled -= _ => Inputs = Vector2.zero;
        }
        #endregion

        #region Motion Methods
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

        #endregion
    }
}