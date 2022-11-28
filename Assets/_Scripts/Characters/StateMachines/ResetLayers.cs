using UnityEngine;

namespace _Scripts.Characters.Animations.StateMachines
{
	public class ResetLayers : NetworkStateMachine
	{
		#region Methods
		protected override void StateMachineEnter(Animator animator, int stateMachinePathHash)
		{
			MyCharacter.PlayerSM.EnableLayers = false;
		}
		#endregion
	}
}
