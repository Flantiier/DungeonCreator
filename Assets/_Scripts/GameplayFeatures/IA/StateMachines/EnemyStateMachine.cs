using UnityEngine;

namespace _Scripts.GameplayFeatures.IA.StateMachines
{
    public class EnemyStateMachine : StateMachineBehaviour
    {
        protected ChasingEnemy GetEnemy(Animator animator)
        {
            return animator.GetComponent<ChasingEnemy>();
        }
    }
}