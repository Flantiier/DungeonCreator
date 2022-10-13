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
        player.DodgeCurve.MoveKey(0, new Keyframe(_curve.keys[0].time, player.CurrentSpeed));

        //Set the speed at the end of the dodge
        float endSpeed = player.DodgeSpeed();
        player.DodgeCurve.MoveKey(2, new Keyframe(_curve.keys[2].time, endSpeed));
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        PlayerController player = AdvStaticAnim.GetPlayer(animator);

        //Determines the player speed by reading a curve
        float dodgeSpeed = player.DodgeCurve.Evaluate(Mathf.Repeat(stateInfo.normalizedTime, 1f));
        //Apply the readed speed to the player movement
        player.HandleDodgeMovement(dodgeSpeed);
    }
}
