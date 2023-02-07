using UnityEngine;
using _Scripts.Characters.DungeonMaster;

namespace _Scripts.Characters.Animations.StateMachines.Boss
{
	public class BossStateMachine : NetworkStateMachine
	{
		#region Variables
		protected BossAnimator BossAnimator { get; private set; }
        protected BossController Boss => BossAnimator.Boss;
        #endregion

        #region Builts_In
        public new void OnStateMachineEnter(Animator animator, int stateMachinePathHash)
        {
            if (!ViewIsMine(animator))
                return;

            GetBossAnimator(animator);
            StateMachineEnter(animator, stateMachinePathHash);
        }

        public new void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (!ViewIsMine(animator))
                return;

            GetBossAnimator(animator);
            StateEnter(animator, stateInfo, layerIndex);
        }
        #endregion

        #region Methods
        /// <summary>
        /// Get the Boss animator script
        /// </summary>
        private void GetBossAnimator(Animator animator)
		{
			if (BossAnimator)
				return;

			BossAnimator = animator.GetComponent<BossAnimator>();
		}
		#endregion
	}
}
