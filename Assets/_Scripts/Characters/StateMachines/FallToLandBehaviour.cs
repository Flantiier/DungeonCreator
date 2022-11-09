using UnityEngine;

namespace _Scripts.Characters.Animations.StateMachines
{
    public class FallToLandBehaviour : NetworkStateMachine
    {
        protected override void StateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            Character player = CharacterAnimation.GetPlayer(animator);

            if (player.AirTime >= 1f)
            {
                player.GroundSM.IsLanding = true;
                animator.SetBool("Landing", true);
                player.ResetVelocity();
            }
        }

        protected override void StateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            CharacterAnimation.GetPlayer(animator).GroundSM.IsLanding = false;
            animator.SetBool("Landing", false);
        }
    }
}