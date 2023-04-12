using UnityEngine;

namespace _Scripts.GameplayFeatures.IA.StateMachines
{
	public class EnemyAimStateMachine : EnemyStateMachine
	{
        [SerializeField] private float shootTime = 0.9f;
        private bool _hasShot;

        public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (!enemy.ViewIsMine())
                return;

            ShootTrigger(stateInfo.normalizedTime);
        }

        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            _hasShot = false;

            if (!enemy.ViewIsMine())
                return;

            enemy.RPCCall("TriggerRPC", Photon.Pun.RpcTarget.All, "Shoot", false);
        }

        private void ShootTrigger(float normalizedTime)
        {
            if (_hasShot || normalizedTime <= shootTime)
                return;

            _hasShot = true;
            enemy.RPCCall("TriggerRPC", Photon.Pun.RpcTarget.All, "Shoot", true);
        }
    }
}
