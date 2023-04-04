using UnityEngine;
using UnityEngine.Rendering;

namespace _Scripts.GameplayFeatures.IA.StateMachines
{
	public class EnemyAimStateMachine : StateMachineBehaviour
	{
        [SerializeField] private float shootTime = 0.9f;
        private bool _hasShot;

        public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            ShootTrigger(animator, stateInfo.normalizedTime);
        }

        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            _hasShot = false;
            animator.ResetTrigger("Shoot");
        }

        private void ShootTrigger(Animator animator, float normalizedTime)
        {
            if (_hasShot || normalizedTime <= shootTime)
                return;

            _hasShot = true;
            animator.SetTrigger("Shoot");
        }
    }
}
