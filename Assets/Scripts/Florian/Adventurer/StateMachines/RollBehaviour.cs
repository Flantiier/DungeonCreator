using UnityEngine;

public class RollBehaviour : StateMachineBehaviour
{
    //Curv to read dodge vel
    [SerializeField] private AnimationCurve curve;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        PlayerController player = AdvStaticAnim.GetPlayer(animator);

        //Set the player state
        player.PlayerStateMachine.CurrentState = PlayerStateMachine.PlayerStates.Roll;

        //Create a new curve
        float startVel = player.CurrentSpeed;
        //Set the momentum curve
        curve = new AnimationCurve(curve.keys);
        curve.MoveKey(0, new Keyframe(curve.keys[0].time, startVel));
        curve.MoveKey(1, new Keyframe(curve.keys[1].time, player.dodgeSpeed));
        curve.MoveKey(2, new Keyframe(curve.keys[2].time, startVel));
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        PlayerController player = AdvStaticAnim.GetPlayer(animator);

        //Determines the player speed by reading a curve
        float dodgeSpeed = curve.Evaluate(Mathf.Repeat(stateInfo.normalizedTime, 1f));
        //Apply the readed speed to the player movement
        player.HandleDodgeMovement(dodgeSpeed);
    }
}
