using UnityEngine;

public class FallToLandBehaviour : StateMachineBehaviour
{
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //Reset player velocity when touch the ground
        PlayerController player = AdvStaticAnim.GetPlayer(animator);

        //Player is falling a long time
        if (player.AirTime >= player.TimeToLand)
        {
            //Trigger landing animation
            player.IsLanding = true;
            animator.SetBool("Landing", true);
            player.ResetVelocity();
        }
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //Reset landing
        AdvStaticAnim.GetPlayer(animator).IsLanding = false;
        animator.SetBool("Landing", false);
    }
}