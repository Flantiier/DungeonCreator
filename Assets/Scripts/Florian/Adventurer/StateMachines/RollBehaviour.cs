using UnityEngine;

public class RollBehaviour : StateMachineBehaviour
{
    private AnimationCurve _curve;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        PlayerController player = AdvStaticAnim.GetPlayer(animator);

        //Set the player state
        player.PlayerStateMachine.CurrentState = PlayerStateMachine.PlayerStates.Roll;
        //Create a new curve
        _curve = new AnimationCurve(player.DodgeCurve.keys);
        //Set the first key value to curretn speed value
        _curve.MoveKey(0, new Keyframe(_curve.keys[0].time, player.CurrentSpeed));

        player.ResetVelocity() ;
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        PlayerController player = AdvStaticAnim.GetPlayer(animator);

        float speed = _curve.Evaluate(Mathf.Repeat(stateInfo.normalizedTime, 1f));
        player.HandleDodgeMovement(speed);
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        AdvStaticAnim.GetPlayer(animator).PlayerStateMachine.CurrentState = PlayerStateMachine.PlayerStates.Walk;
    }
}
