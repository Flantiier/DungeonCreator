using UnityEngine;

namespace _Scripts.Characters.Animations.StateMachines
{
    public class FallingState : CharacterStateMachine
    {
        #region Inherited Methods
        protected override void StateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            animator.SetBool("IsFalling", true);
        }

        protected override void StateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            Character.ResetAirTime();
            animator.SetBool("IsFalling", false);
        }
        #endregion
    }
}
