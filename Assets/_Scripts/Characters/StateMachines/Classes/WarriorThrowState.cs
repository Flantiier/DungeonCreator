using UnityEngine;

namespace _Scripts.Characters.Animations.StateMachines
{
	public class WarriorThrowState : CharacterMeleeAttack
	{
		#region Variables
		private WarriorAnimator _warrior;
        #endregion

        #region Methods
        protected override void StateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{
			base.StateEnter(animator, stateInfo, layerIndex);

			_warrior = animator.GetComponent<WarriorAnimator>();
			_warrior.EnableSword(false);
			Character.LookTowardsOrientation();
		}

		protected override void StateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
			base.StateExit(animator, stateInfo, layerIndex);
            _warrior.EnableSword(true);
        }
		#endregion
	}
}
