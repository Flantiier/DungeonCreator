using UnityEngine;
using _Scripts.Characters.StateMachines;

namespace _Scripts.Characters.Animations.StateMachines
{
    public class RollBehaviour : NetworkStateMachine
    {
        #region Variables
        private AnimationCurve _dodgeCurve;
        #endregion

        #region Inherited Methods
        protected override void StateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            Character player = CharacterAnimation.GetPlayer(animator);

            player.PlayerSM.CurrentState = PlayerStateMachine.PlayerStates.Roll;
            player.UseStamina(player.OverallDatas.staminaToDodge);

            DynamicDodgeCurve(player);
        }

        protected override void StateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            Character player = CharacterAnimation.GetPlayer(animator);

            float dodgeSpeed = _dodgeCurve.Evaluate(Mathf.Repeat(stateInfo.normalizedTime, 1f));
            player.UpdateSpeed(dodgeSpeed);
        }
        #endregion

        #region Methods
        /// <summary>
        /// Set dynamically the dodge curve keys
        /// </summary>
        private void DynamicDodgeCurve(Character player)
        {
            float startVel = player.CurrentSpeed;

            _dodgeCurve = player.OverallDatas.dodgeCurve;
            _dodgeCurve.MoveKey(0, new Keyframe(_dodgeCurve.keys[0].time, startVel));
            _dodgeCurve.MoveKey(1, new Keyframe(_dodgeCurve.keys[1].time, player.OverallDatas.dodgeSpeed));
            _dodgeCurve.MoveKey(2, new Keyframe(_dodgeCurve.keys[2].time, startVel));
        }
        #endregion
    }
}
