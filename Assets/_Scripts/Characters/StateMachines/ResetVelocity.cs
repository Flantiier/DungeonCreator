using UnityEngine;

namespace _Scripts.Characters.Animations.StateMachines
{
    public class ResetVelocity : CharacterStateMachine
    {
        private bool _resetVel;

        protected override void StateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            _resetVel = false;
        }

        protected override void StateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (_resetVel)
                return;

            if (Character.Velocity.magnitude != 0f && stateInfo.normalizedTime > 0.05f)
            {
                Character.ResetCharacterVelocity();
                _resetVel = true;
            }
        }
    }
}
