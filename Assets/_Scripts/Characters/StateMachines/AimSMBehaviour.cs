using UnityEngine;

namespace _Scripts.Characters.Animations.StateMachines
{
	public class AimSMBehaviour : NetworkStateMachine
	{
        protected override void StateMachineEnter(Animator animator, int stateMachinePathHash)
        {
            animator.GetComponent<CharacterAnimator>().SwapWeapons(1);
            Debug.Log("start");

            Character player = CharacterAnimation.GetPlayer(animator);

            player.PlayerSM.EnableLayers = true;
        }

        protected override void StateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            Character player = CharacterAnimation.GetPlayer(animator);

            player.AimRotation();
        }

        protected override void StateMachineExit(Animator animator, int stateMachinePathHash)
        {
            animator.GetComponent<CharacterAnimator>().SwapWeapons(0);
            Debug.Log("ecit");

            Character player = CharacterAnimation.GetPlayer(animator);

            player.PlayerSM.EnableLayers = false;
        }
	}
}
