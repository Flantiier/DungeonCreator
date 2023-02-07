using UnityEngine;

namespace _Scripts.Characters.Animations.StateMachines
{
    public class LandState : CharacterStateMachine
    {
        protected override void StateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (Character.AirTime >= 1f)
            {
                Character.GroundSM.IsLanding = true;
                animator.SetBool("Landing", true);
                Character.ResetCharacterVelocity();
            }
        }

        protected override void StateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            Character.GroundSM.IsLanding = false;
            animator.SetBool("Landing", false);
        }
    }
}