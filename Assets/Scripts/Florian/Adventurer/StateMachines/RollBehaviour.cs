using UnityEngine;
using _Scripts.Characters.StateMachines;

namespace _Scripts.Characters.Animations.StateMachines
{
    public class RollBehaviour : NetworkStateMachine
    {
        [SerializeField] private AnimationCurve dodgeCurve;

        protected override void OnEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            Character player = CharacterAnimation.GetPlayer(animator);

            player.PlayerStateMachine.CurrentState = PlayerStateMachine.PlayerStates.Roll;

            float startVel = player.CurrentSpeed;

            dodgeCurve = new AnimationCurve(dodgeCurve.keys);
            dodgeCurve.MoveKey(0, new Keyframe(dodgeCurve.keys[0].time, startVel));
            dodgeCurve.MoveKey(1, new Keyframe(dodgeCurve.keys[1].time, player.DodgeSpeed));
            dodgeCurve.MoveKey(2, new Keyframe(dodgeCurve.keys[2].time, startVel));
        }

        protected override void OnUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            Character player = CharacterAnimation.GetPlayer(animator);

            float dodgeSpeed = dodgeCurve.Evaluate(Mathf.Repeat(stateInfo.normalizedTime, 1f));
            player.HandleDodgeMovement(dodgeSpeed);
        }
    }
}
