using UnityEngine;

namespace _Scripts.Characters.Animations.StateMachines
{
	public class ResetLayers : CharacterStateMachine
	{
		#region Methods
		protected override void StateMachineEnter(Animator animator, int stateMachinePathHash)
		{
			Character.PlayerSM.EnableLayers = false;
		}
		#endregion
	}
}
