using UnityEngine;

namespace _Scripts.Characters.Animations.StateMachines
{
	public class AimSMBehaviour : NetworkStateMachine
	{
		protected override void OnEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{
			CharacterAnimation.GetPlayer(animator).PlayerStateMachine.EnableLayers = true;
		}

		protected override void OnExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{
            CharacterAnimation.GetPlayer(animator).PlayerStateMachine.EnableLayers = false;
        }
	}
}
