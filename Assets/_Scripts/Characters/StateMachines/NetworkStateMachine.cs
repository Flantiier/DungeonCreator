using UnityEngine;

namespace _Scripts.Characters.Animations.StateMachines
{
	public class NetworkStateMachine : StateMachineBehaviour
	{
        #region Builts_In

        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (!CharacterAnimation.IsMyPlayer(CharacterAnimation.GetPlayer(animator)))
                return;

            OnEnter(animator, stateInfo, layerIndex);
		}
		public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (!CharacterAnimation.IsMyPlayer(CharacterAnimation.GetPlayer(animator)))
                return;

            OnUpdate(animator, stateInfo, layerIndex);
        }

		public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (!CharacterAnimation.IsMyPlayer(CharacterAnimation.GetPlayer(animator)))
                return;

            OnExit(animator, stateInfo, layerIndex);
        }

        #endregion

        #region Inherited Methods

        /// <summary>
        /// Executed during OnStateEnter of the StateMachine on local player
        /// </summary>
        protected virtual void OnEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) { }

        /// <summary>
        /// Executed during OnStateUpdate of the StateMachine on local player
        /// </summary>
        protected virtual void OnUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) { }

        /// <summary>
        /// Executed during OnStateExit of the StateMachine on local player
        /// </summary>
        protected virtual void OnExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) { }

        #endregion
    }
}
