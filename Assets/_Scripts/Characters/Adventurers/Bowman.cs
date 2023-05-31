using UnityEngine;
using UnityEngine.InputSystem;
using _Scripts.Interfaces;
using _ScriptableObjects.Cinemachine;

namespace _Scripts.Characters.Adventurers
{
    public class Bowman : Character
    {
        #region Variables
        [Header("Defuse properties")]
        [SerializeField] private TpsCameraProperties cameraProperties;
        [SerializeField] private float defuseDistance = 3f;
        [SerializeField] private LayerMask defuseLayer;

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

            if (!Physics.Raycast(MainCamera.position, MainCamera.forward, out RaycastHit hit, 
                                    cameraProperties.framingTranposer.m_CameraDistance + defuseDistance, defuseLayer))
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

            _currentTrap.DefuseTrap();
            SkillUsed();
        }
        #endregion
    }
}
