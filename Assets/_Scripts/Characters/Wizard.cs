using UnityEngine;
using UnityEngine.InputSystem;

namespace _Scripts.Characters
{
	public class Wizard : Character
	{
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
