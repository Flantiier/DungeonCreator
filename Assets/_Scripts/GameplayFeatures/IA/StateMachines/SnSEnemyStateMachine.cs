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
            if (!enemy.ViewIsMine())
                return;

            enemy.RPCCall("TriggerRPC", Photon.Pun.RpcTarget.All, "Defend", false);
            enemy.RPCCall("TriggerRPC", Photon.Pun.RpcTarget.All, "Combo", false);
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
                enemy.RPCCall("TriggerRPC", Photon.Pun.RpcTarget.All, "Defend", true);
                return;
            }

            //Combo
            _combo = true;
            enemy.RPCCall("TriggerRPC", Photon.Pun.RpcTarget.All, "Combo", true);

        }
    }
}