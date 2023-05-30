using UnityEngine;

namespace _Scripts.Characters.Animations.StateMachines
{
	public class WizardSkillState : CharacterStateMachine
	{
		#region Inherited Methods
		protected override void StateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{
			Character.PlayerSM.CanMove = false;
			Character.PlayerSM.SkillUsed = true;
			Character.PlayerSM.CanAttack = false;
			Character.PlayerSM.CanDodge = false;

			Character.ResetCharacterVelocity();
        }

		protected override void StateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{
			Character.PlayerSM.CanMove = true;
            Character.PlayerSM.CanAttack = true;
            Character.PlayerSM.CanDodge = true;
        }
        #endregion
    }
}
