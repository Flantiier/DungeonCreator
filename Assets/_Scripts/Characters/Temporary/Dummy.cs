using UnityEngine;
using _Scripts.NetworkScript;
using Photon.Pun;

namespace _Scripts.Characters.Temporary
{
	public class Dummy : NetworkMonoBehaviour, IDamageable
	{
		#region Variables
		private Animator _animator;
        #endregion

        #region Builts_In
        public virtual void Awake()
        {
            _animator = GetComponentInChildren<Animator>();
        }
        #endregion

        #region Methods
        public void Damage(float damages)
        {
            Debug.Log($"Took {damages} damages");
            View.RPC("HitFeedbackRPC", RpcTarget.All);
        }

        /// <summary>
        /// Sending a RPC to trigger a feedback over the network
        /// </summary>
        [PunRPC]
        public void HitFeedbackRPC()
        {
            _animator.SetTrigger("Hit");
        }
        #endregion
    }
}
