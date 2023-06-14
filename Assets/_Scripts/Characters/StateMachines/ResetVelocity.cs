using UnityEngine;

namespace _Scripts.Characters.Animations.StateMachines
{
    public class ResetVelocity : CharacterStateMachine
    {
        protected override void StateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            base.StateEnter(animator, stateInfo, layerIndex);
            Character.ResetCharacterVelocity();
        }
    }
}
