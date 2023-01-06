using UnityEngine;
using UnityEngine.InputSystem;
using _Scripts.Interfaces;

namespace _Scripts.Characters.Adventurers
{
    public class Bowman : Character
    {
        #region Variables
        [Header("Defuse properties")]
        [SerializeField] private float defuseDistance = 3f;
        [SerializeField] private LayerMask defuseLayers;

        private IDefusable _currentTrap;
        #endregion

        #region Properties
        public bool IsDefusing { get; set; }
        public float TargetDefuseTime { get; set; }
        public float CurrentDefuseTime { get; set; }
        #endregion

        #region Inherited Methods

        #region Inputs Methods
        protected override void SubscribeInputActions()
        {
            base.SubscribeInputActions();

            _inputs.Gameplay.Skill.started += SkillAction;
        }

        protected override void UnsubscribeInputActions()
        {
            base.UnsubscribeInputActions();

            _inputs.Gameplay.Skill.started -= SkillAction;
        }
        #endregion

        protected override void UpdateAnimations()
        {
            base.UpdateAnimations();

            Animator.SetBool("Defusing", IsDefusing);
            UpdateAnimationLayers();
        }

        protected override void HandleCharacterStateMachine()
        {
            if (IsDefusing)
                return;

            base.HandleCharacterStateMachine();
        }

        protected override bool SkillConditions()
        {
            if (IsDefusing)
                return false;

            return base.SkillConditions();
        }
        #endregion

        #region Methods
        private void SkillAction(InputAction.CallbackContext _)
        {
            if (!SkillConditions())
                return;

            if (!Physics.Raycast(MainCamTransform.position, MainCamTransform.forward, out RaycastHit hit, _tpsCamera.CameraDistance + defuseDistance, defuseLayers))
                return;

            if (!hit.collider.TryGetComponent(out IDefusable trap) || trap.IsDisabled)
                return;

            _currentTrap = trap;
            TargetDefuseTime = trap.DefuseDuration;
            RPCAnimatorTrigger(Photon.Pun.RpcTarget.All, "SkillEnabled", true);
        }

        /// <summary>
        /// Defuse the last trap
        /// </summary>
        public void DefuseTrap()
        {
            if (_currentTrap == null)
                return;

            _currentTrap.HasBeenDefused();
            SkillUsed();
        }
        #endregion
    }
}
