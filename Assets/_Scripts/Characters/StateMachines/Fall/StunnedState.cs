using UnityEngine;

namespace _Scripts.Characters.Animations.StateMachines
{
    public class StunnedState : CharacterStateMachine
    {
        protected override void StateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            Character.ResetCharacterVelocity();
            Character.PlayerSM.CurrentState = Characters.StateMachines.PlayerStateMachine.PlayerStates.Stunned;
        }
    }
}