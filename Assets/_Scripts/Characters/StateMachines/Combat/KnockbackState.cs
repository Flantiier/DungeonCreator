using UnityEngine;

namespace _Scripts.Characters.Animations.StateMachines
{
	public class KnockbackState : NetworkStateMachine
	{
		[Header("Knockback properties")]
		[SerializeField] private AnimationCurve curve;

		protected override void StateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{
			MyCharacter.PlayerSM.CurrentState = Characters.StateMachines.PlayerStateMachine.PlayerStates.Knocked;
			MyCharacter.PlayerSM.CanAttack = false;
			MyCharacter.PlayerSM.CanDodge = false;

			MyCharacter.ResetCharacterVelocity();
        }

		protected override void StateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{
			float speed = curve.Evaluate(stateInfo.normalizedTime);
			MyCharacter.UpdateCharacterSpeed(speed);
			MyCharacter.MoveBackwards();
		}

		protected override void StateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{
			MyCharacter.PlayerSM.CanDodge = true;
        }
    }
}
