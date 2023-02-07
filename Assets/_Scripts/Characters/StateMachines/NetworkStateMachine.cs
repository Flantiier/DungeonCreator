using UnityEngine;
using Photon.Pun;

namespace _Scripts.Characters.Animations.StateMachines
{
    public class NetworkStateMachine : StateMachineBehaviour
    {
        #region Builts_In
        public override void OnStateMachineEnter(Animator animator, int stateMachinePathHash)
        {
            if (!ViewIsMine(animator))
                return;

            StateMachineEnter(animator, stateMachinePathHash);
        }

        public override void OnStateMachineExit(Animator animator, int stateMachinePathHash)
        {
            if (!ViewIsMine(animator))
                return;

            StateMachineExit(animator, stateMachinePathHash);
        }

        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (!ViewIsMine(animator))
                return;

            StateEnter(animator, stateInfo, layerIndex);
        }

        public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (!ViewIsMine(animator))
                return;

            StateUpdate(animator, stateInfo, layerIndex);
        }

        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (!ViewIsMine(animator))
                return;

            StateExit(animator, stateInfo, layerIndex);
        }
        #endregion

        #region Inherited Methods
        /// <summary>
        /// Indicate if the photonView is the local one
        /// </summary>
        protected bool ViewIsMine(Animator animator)
        {
            return animator.TryGetComponent(out PhotonView view) ? view.IsMine : false;
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