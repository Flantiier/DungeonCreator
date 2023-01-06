using UnityEngine;
using _Scripts.Characters.Adventurers;

namespace _Scripts.Characters.Animations.StateMachines
{
	public class WarriorSkillState : NetworkStateMachine
	{
        #region Variables
        private Warrior _warrior;
        #endregion

        #region Inherited Methods
        protected override void StateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            _warrior = animator.GetComponentInParent<Warrior>();
            _warrior.Shield.EnableShield(true);
        }

        protected override void StateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            _warrior.Shield.EnableShield(false);
        }
        #endregion
    }
}
