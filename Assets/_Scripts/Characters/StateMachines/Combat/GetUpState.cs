using UnityEngine;

namespace _Scripts.Characters.Animations.StateMachines
{
	public class GetUpState : CharacterStateMachine
	{
		protected override void StateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{
			Character.PlayerSM.CurrentState = Characters.StateMachines.PlayerStateMachine.PlayerStates.Walk;
		}
	}
}
