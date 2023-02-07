using UnityEngine;

namespace _Scripts.Characters.Animations.StateMachines
{
    public class CharacterStateMachine : NetworkStateMachine
    {
        #region Variables
        protected Character Character { get; private set; }
        #endregion

        #region Builts_In
        public new void OnStateMachineEnter(Animator animator, int stateMachinePathHash)
        {
            if (!ViewIsMine(animator))
                return;

            GetCharacter(animator);
            StateMachineEnter(animator, stateMachinePathHash);
        }

        public new void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (!ViewIsMine(animator))
                return;

            GetCharacter(animator);
            StateEnter(animator, stateInfo, layerIndex);
        }
        #endregion

        #region Inherited Methods
        /// <summary>
        /// Get the player character refrences
        /// </summary>
        protected void GetCharacter(Animator animator)
        {
            if (Character)
                return;

            Character = animator.GetComponent<CharacterAnimator>().Character;
        }
        #endregion
    }
}
