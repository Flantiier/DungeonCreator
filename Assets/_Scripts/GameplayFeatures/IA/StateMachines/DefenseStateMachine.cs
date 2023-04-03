using UnityEngine;

namespace _Scripts.GameplayFeatures.IA.StateMachines
{
    public class DefenseStateMachine : StateMachineBehaviour
    {
        [SerializeField] private int attackAmount = 1;

        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            animator.SetInteger("AttackIndex", Random.Range(0, attackAmount));
            animator.ResetTrigger("Defend");
        }
    }
}