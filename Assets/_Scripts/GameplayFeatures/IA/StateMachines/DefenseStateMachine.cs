using UnityEngine;

namespace _Scripts.GameplayFeatures.IA.StateMachines
{
    public class DefenseStateMachine : EnemyStateMachine
    {
        [SerializeField] private int attackAmount = 1;

        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            base.OnStateEnter(animator, stateInfo, layerIndex);

            if (!enemy.ViewIsMine())
                return;

            animator.SetInteger("AttackIndex", Random.Range(0, attackAmount));
            enemy.RPCCall("TriggerRPC", Photon.Pun.RpcTarget.All, "Defend", false);
        }
    }
}