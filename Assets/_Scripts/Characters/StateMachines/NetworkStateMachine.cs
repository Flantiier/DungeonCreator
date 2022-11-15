using UnityEngine;

namespace _Scripts.Characters.Animations.StateMachines
{
    public class NetworkStateMachine : StateMachineBehaviour
    {
        #region Variables
        protected Character MyCharacter { get; set; }
        #endregion

        #region Builts_In
        public override void OnStateMachineEnter(Animator animator, int stateMachinePathHash)
        {
            if (!animator.GetComponent<CharacterAnimator>().ViewIsMine())
                return;

            StateMachineEnter(animator, stateMachinePathHash);
        }

        public override void OnStateMachineExit(Animator animator, int stateMachinePathHash)
        {
            if (!animator.GetComponent<CharacterAnimator>().ViewIsMine())
                return;

            StateMachineExit(animator, stateMachinePathHash);
        }

        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (!animator.GetComponent<CharacterAnimator>().ViewIsMine())
                return;

            GetCharacter(animator);
            StateEnter(animator, stateInfo, layerIndex);
        }
        public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (!animator.GetComponent<CharacterAnimator>().ViewIsMine())
                return;

            StateUpdate(animator, stateInfo, layerIndex);
        }

        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (!animator.GetComponent<CharacterAnimator>().ViewIsMine())
                return;

            StateExit(animator, stateInfo, layerIndex);
        }
        #endregion

        #region Inherited Methods
        /// <summary>
        /// Get the player character refrences
        /// </summary>
        protected void GetCharacter(Animator animator)
        {
            MyCharacter = animator.GetComponent<CharacterAnimator>().Character;
        }

        /// <summary>
        /// Executed during OnStateEnter of the StateMachine on local player
        /// </summary>
        protected virtual void StateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) { }

        /// <summary>
        /// Executed during OnStateUpdate of the StateMachine on local player
        /// </summary>
        protected virtual void StateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) { }

        /// <summary>
        /// Executed during OnStateExit of the StateMachine on local player
        /// </summary>
        protected virtual void StateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) { }

        /// <summary>
        /// Executed during OnStateMachineEnter of the StateMachine on local player
        /// </summary>
        protected virtual void StateMachineEnter(Animator animator, int stateMachinePathHash) { }

        /// <summary>
        /// Executed during OnStateMachineExit of the StateMachine on local player
        /// </summary>
        protected virtual void StateMachineExit(Animator animator, int stateMachinePathHash) { }
        #endregion
    }
}
