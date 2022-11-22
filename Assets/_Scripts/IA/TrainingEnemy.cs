using System.Collections;
using UnityEngine;
using Photon.Pun;

namespace _Scripts.IA
{
	public class TrainingEnemy : Enemy
	{
        #region Inherited Method
        protected override void HandleEntityDeath()
		{
            RPCAnimatorTrigger(RpcTarget.All, "Death", true);
            RPCCall("ResetEnemyRPC", RpcTarget.MasterClient, 3f);
        }
        #endregion

        #region Methods
        [PunRPC]
        public void ResetEnemyRPC(float delay)
        {
            StartCoroutine(ResetCoroutine(delay));
        }

        private IEnumerator ResetCoroutine(float delay)
        {
            yield return new WaitForSecondsRealtime(delay);

            RPCDebugging(RpcTarget.All, $"Resetting {gameObject.name}");
            RPCCall("InitializeEnemy", RpcTarget.AllBuffered);
            RPCAnimatorTrigger(RpcTarget.AllBuffered, "Reset", true);
        }
        #endregion
    }
}