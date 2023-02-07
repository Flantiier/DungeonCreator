using UnityEngine;

namespace _Scripts.Characters.Animations.StateMachines
{
	public class KnockbackState : CharacterStateMachine
	{
		[Header("Knockback properties")]
		[SerializeField] private AnimationCurve curve;

		protected override void StateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{
			Character.PlayerSM.CurrentState = Characters.StateMachines.PlayerStateMachine.PlayerStates.Knocked;
			Character.PlayerSM.CanAttack = false;
			Character.PlayerSM.CanDodge = false;

			Character.ResetCharacterVelocity();
        }

		protected override void StateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{
			float speed = curve.Evaluate(stateInfo.normalizedTime);
			Character.UpdateCharacterSpeed(speed);
			Character.MoveBackwards();
		}

		protected override void StateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{
			Character.PlayerSM.CanDodge = true;
        }
    }
}
