using UnityEngine;

namespace _Scripts.Characters.Animations.StateMachines
{
	public class WizardSkillState : CharacterStateMachine
	{
		#region Inherited Methods
		protected override void StateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{
			Character.SkillUsed();
			Character.PlayerSM.CanAttack = false;
			Character.PlayerSM.CanDodge = false;
        }

		protected override void StateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{
            Character.PlayerSM.CanAttack = true;
            Character.PlayerSM.CanDodge = true;
        }
        #endregion
    }
}
