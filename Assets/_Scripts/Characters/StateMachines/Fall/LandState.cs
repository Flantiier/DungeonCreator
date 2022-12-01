using UnityEngine;

namespace _Scripts.Characters.Animations.StateMachines
{
    public class LandState : NetworkStateMachine
    {
        protected override void StateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (MyCharacter.AirTime >= 1f)
            {
                MyCharacter.GroundSM.IsLanding = true;
                animator.SetBool("Landing", true);
                MyCharacter.ResetVelocity();
            }
        }

        protected override void StateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            MyCharacter.GroundSM.IsLanding = false;
            animator.SetBool("Landing", false);
        }
    }
}