using UnityEngine;
using _Scripts.Characters.StateMachines;

namespace _Scripts.Characters.Animations.StateMachines
{
    public class MotionSM : NetworkStateMachine
    {
        protected override void StateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            MyCharacter.PlayerSM.CurrentState = PlayerStateMachine.PlayerStates.Walk;
            MyCharacter.PlayerSM.CanAttack = true;
        }

        protected override void StateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (MyCharacter.PlayerSM.IsRunning)
                MyCharacter.UsingStamina(MyCharacter.OverallDatas.staminaToRun * Time.deltaTime);
        }
    }
}