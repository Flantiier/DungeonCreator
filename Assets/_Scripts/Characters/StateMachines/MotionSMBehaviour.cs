using UnityEngine;
using _Scripts.Characters.StateMachines;

namespace _Scripts.Characters.Animations.StateMachines
{
    public class MotionSMBehaviour : NetworkStateMachine
    {
        protected override void StateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            Character player = CharacterAnimation.GetPlayer(animator);

            player.PlayerSM.CurrentState = PlayerStateMachine.PlayerStates.Walk;
            player.PlayerSM.CanAttack = true;
        }

        protected override void StateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            Character player = CharacterAnimation.GetPlayer(animator);

            if (player.PlayerSM.IsRunning)
                player.UseStamina( player.OverallDatas.staminaToRun * Time.deltaTime);
        }
    }
}