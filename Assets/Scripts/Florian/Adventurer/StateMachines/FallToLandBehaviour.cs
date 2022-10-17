using UnityEngine;

namespace _Scripts.Characters.Animations.StateMachines
{
    public class FallToLandBehaviour : NetworkStateMachine
    {
        protected override void OnEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            Character player = CharacterAnimation.GetPlayer(animator);

            if (player.AirTime >= 1f)
            {
                player.GroundStateMachine.IsLanding = true;
                animator.SetBool("Landing", true);
                player.ResetVelocity();
            }
        }

        protected override void OnExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            CharacterAnimation.GetPlayer(animator).GroundStateMachine.IsLanding = false;
            animator.SetBool("Landing", false);
        }
    }
}