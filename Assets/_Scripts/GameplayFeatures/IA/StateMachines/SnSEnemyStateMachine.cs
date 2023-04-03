using UnityEngine;

namespace _Scripts.GameplayFeatures.IA.StateMachines
{
    public class SnSEnemyStateMachine : EnemyAttackState
    {
        private SwordAndShieldEnemy classEnemy;
        private bool shouldDefend;

        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            classEnemy = animator.GetComponent<SwordAndShieldEnemy>();
            base.OnStateEnter(animator, stateInfo, layerIndex);
        }

        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            base.OnStateExit(animator, stateInfo, layerIndex);

            animator.ResetTrigger("Defend");
            animator.ResetTrigger("Combo");
            shouldDefend = false;
        }

        protected override void HandleComboTrigger(Animator animator, float normalizedTime)
        {
            if (_combo || normalizedTime < attackDuration)
                return;

            if (!enemy.TargetNear)
                return;

            if (classEnemy.ShouldDefend())
            {
                animator.SetTrigger("Defend");
                return;
            }

            //Combo
            _combo = true;
            animator.SetTrigger("Combo");

        }
    }
}