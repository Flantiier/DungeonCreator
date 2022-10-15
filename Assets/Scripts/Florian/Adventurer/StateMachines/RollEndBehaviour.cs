using UnityEngine;
using _Scripts.Characters.StateMachines;

namespace _Scripts.Characters.Animations.StateMachines
{
    public class RollEndBehaviour : NetworkStateMachine
    {
        protected override void OnEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            CharacterAnimation.GetPlayer(animator).PlayerStateMachine.CurrentState = PlayerStateMachine.PlayerStates.Walk;
        }
    }
}
