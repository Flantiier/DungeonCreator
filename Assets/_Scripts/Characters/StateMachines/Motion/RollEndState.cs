using UnityEngine;
using _Scripts.Characters.StateMachines;

namespace _Scripts.Characters.Animations.StateMachines
{
    public class RollEndState : NetworkStateMachine
    {
        protected override void StateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            MyCharacter.PlayerSM.CurrentState = PlayerStateMachine.PlayerStates.Walk;
        }
    }
}
