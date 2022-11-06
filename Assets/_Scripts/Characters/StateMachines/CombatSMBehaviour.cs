using UnityEngine;

namespace _Scripts.Characters.Animations.StateMachines
{
	public class CombatSMBehaviour : NetworkStateMachine
	{
		protected override void StateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{
			CharacterAnimation.GetPlayer(animator).PlayerStateMachine.CanAttack = true;
		}
	}
}
