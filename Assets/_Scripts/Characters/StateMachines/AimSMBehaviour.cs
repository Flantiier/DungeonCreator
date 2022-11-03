using UnityEngine;

namespace _Scripts.Characters.Animations.StateMachines
{
	public class AimSMBehaviour : NetworkStateMachine
	{
		protected override void OnEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{
			Character player = CharacterAnimation.GetPlayer(animator);

            player.PlayerStateMachine.EnableLayers = true;
		}

		protected override void OnUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{
			Character player = CharacterAnimation.GetPlayer(animator);

			player.AimRotation();
        }

		protected override void OnExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{
			Character player = CharacterAnimation.GetPlayer(animator);

            player.PlayerStateMachine.EnableLayers = false;
        }
	}
}
