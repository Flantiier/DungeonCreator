using UnityEngine;

public class RollEndBehaviour : StateMachineBehaviour
{
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        AdvStaticAnim.GetPlayer(animator).PlayerStateMachine.CurrentState = PlayerStateMachine.PlayerStates.Walk;
    }
}
