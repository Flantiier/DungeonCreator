using UnityEngine;

namespace _Scripts.GameplayFeatures.IA.StateMachines
{
    public class EnemyAttackState : EnemyStateMachine
    {
        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            animator.GetComponent<ChasingEnemy>().AttackWait = true;
            animator.applyRootMotion = true;
        }

        public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            animator.GetComponent<ChasingEnemy>().Stop();
        }

        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            animator.GetComponent<ChasingEnemy>().AttackWait = false;
            animator.applyRootMotion = false;
        }
    }
}
