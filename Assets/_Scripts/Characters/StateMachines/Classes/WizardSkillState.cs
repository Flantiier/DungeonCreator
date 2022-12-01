using UnityEngine;

namespace _Scripts.Characters.Animations.StateMachines
{
	public class WizardSkillState : NetworkStateMachine
	{
		#region Inherited Methods
		protected override void StateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{
			MyCharacter.InvokeSkillCooldown();
			MyCharacter.PlayerSM.CanAttack = false;
			MyCharacter.PlayerSM.CanDodge = false;
        }

		protected override void StateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{
            MyCharacter.PlayerSM.CanAttack = true;
            MyCharacter.PlayerSM.CanDodge = true;
        }
        #endregion
    }
}
