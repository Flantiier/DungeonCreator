using UnityEngine;

namespace _Scripts.Characters.Animations.StateMachines
{
	public class BowmanSkillState : NetworkStateMachine
	{
		#region Variables
		private Bowman _bowman;
        #endregion

        #region Inherited Methods
        protected override void StateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{
			_bowman = MyCharacter.GetComponent<Bowman>();
			_bowman.IsDefusing = true;

			MyCharacter.ResetVelocity();
			MyCharacter.PlayerSM.CanAttack = false;
		}

		protected override void StateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{
			_bowman.IsDefusing = false;
			MyCharacter.PlayerSM.CanAttack = true;
		}
		#endregion
	}
}
