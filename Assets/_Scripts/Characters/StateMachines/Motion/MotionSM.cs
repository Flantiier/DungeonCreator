using UnityEngine;
using _Scripts.Characters.StateMachines;

namespace _Scripts.Characters.Animations.StateMachines
{
    public class MotionSM : CharacterStateMachine
    {
        protected override void StateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            Character.PlayerSM.CurrentState = PlayerStateMachine.PlayerStates.Walk;
            Character.PlayerSM.CanAttack = true;
        }

        protected override void StateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (!Character.RunConditions())
                return;

            Character.UsingStamina(Character.OverallDatas.staminaToRun * Time.deltaTime);
        }
    }
}