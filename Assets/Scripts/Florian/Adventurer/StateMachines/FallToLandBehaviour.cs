using UnityEngine;

public class FallToLandBehaviour : StateMachineBehaviour
{
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        PlayerController player = GetPlayer(animator);

        player.IsLanding = true;
        player.ResetVelocity();
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        GetPlayer(animator).IsLanding = false;
    }

    private PlayerController GetPlayer(Animator animator)
    {
        return animator.GetComponent<PlayerAnimator>().Controller;
    }
}
