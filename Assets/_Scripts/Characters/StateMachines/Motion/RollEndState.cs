using UnityEngine;
using _Scripts.Characters.StateMachines;

namespace _Scripts.Characters.Animations.StateMachines
{
    public class RollEndState : CharacterStateMachine
    {
        protected override void StateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            Character.PlayerSM.CurrentState = PlayerStateMachine.PlayerStates.Walk;
        }
    }
}
