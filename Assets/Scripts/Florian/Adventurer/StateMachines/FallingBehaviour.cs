using UnityEngine;

namespace _Scripts.Characters.Animations.StateMachines
{
    public class FallingBehaviour : NetworkStateMachine
    {
        protected override void OnEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            animator.SetBool("IsFalling", true);
        }

        protected override void OnExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            CharacterAnimation.GetPlayer(animator).ResetAirTime();
            animator.SetBool("IsFalling", false);
        }
    }
}
