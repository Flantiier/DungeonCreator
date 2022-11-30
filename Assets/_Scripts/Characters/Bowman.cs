using UnityEngine;
using UnityEngine.InputSystem;

namespace _Scripts.Characters
{
	public class Bowman : Character
	{
        #region Variables
        [Header("Defuse properties")]
        [SerializeField] private float distance;
        #endregion

        #region Properties
        public bool IsDefusing { get; set; }
        #endregion

        #region Inherited Methods

        #region Inputs
        protected override void SubscribeToInputs()
        {
            base.SubscribeToInputs();

            _inputs.Gameplay.Skill.started += SkillAction;
        }

        protected override void UnsubscribeToInputs()
        {
            base.UnsubscribeToInputs();

            _inputs.Gameplay.Skill.started -= SkillAction;
        }
        #endregion

        protected override void UpdateAnimations()
        {
            base.UpdateAnimations();

            UpdateAnimationLayers();
        }

        protected override void HandlePlayerStateMachine()
        {
            if (IsDefusing)
                return;

            base.HandlePlayerStateMachine();
        }
        #endregion

        #region Methods
        private void SkillAction(InputAction.CallbackContext _)
        {
            if (!SkillConditions())
                return;

            RPCAnimatorTrigger(Photon.Pun.RpcTarget.All, "SkillEnabled", true);
        }
        #endregion
    }
}
