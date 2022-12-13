using UnityEngine;
using UnityEngine.InputSystem;

namespace _Scripts.Characters
{
	public class Wizard : Character
	{
        #region Inherited Methods

        #region Inputs
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

            UpdateAnimationLayers();
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
