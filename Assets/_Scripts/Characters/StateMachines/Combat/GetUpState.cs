using UnityEngine;

namespace _Scripts.Characters.Animations.StateMachines
{
	public class GetUpState : NetworkStateMachine
	{
		protected override void StateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{
			MyCharacter.PlayerSM.CurrentState = Characters.StateMachines.PlayerStateMachine.PlayerStates.Walk;
		}
	}
}
