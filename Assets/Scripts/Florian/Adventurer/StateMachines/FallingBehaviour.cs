using UnityEngine;

public class FallingBehaviour : StateMachineBehaviour
{
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool("IsFalling", true);
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        AdvStaticAnim.GetPlayer(animator).ResetAirTime();
        animator.SetBool("IsFalling", false);
    }
}
