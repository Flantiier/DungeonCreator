using UnityEngine;

namespace _Scripts.GameplayFeatures.IA.StateMachines
{
    public class EnemyStateMachine : StateMachineBehaviour
    {
        protected ChasingEnemy enemy;

        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            enemy = animator.GetComponent<ChasingEnemy>();
        }
    }
}
