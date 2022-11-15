using UnityEngine;

namespace _Scripts.Characters.Animations.StateMachines
{
    public class FallingBehaviour : NetworkStateMachine
    {
        protected override void StateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            animator.SetBool("IsFalling", true);
        }

        protected override void StateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            MyCharacter.ResetAirTime();
            animator.SetBool("IsFalling", false);
        }
    }
}
