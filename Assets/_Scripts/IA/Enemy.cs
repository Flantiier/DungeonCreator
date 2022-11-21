using UnityEngine;
using Photon.Pun;
using _Scripts.NetworkScript;
using _Scripts.Interfaces;

namespace _Scripts.Characters.Temporary
{
    public class Enemy : NetworkMonoBehaviour, IDamageable
    {
        #region Variables
        [Header("Enemy properties")]
        [SerializeField] protected float health;

        protected Animator _animator;
        #endregion

        #region Properties
        public float CurrentHealth { get; private set; }
        #endregion

        #region Builts_In
        public virtual void Awake()
        {
            _animator = GetComponentInChildren<Animator>();
        }
        #endregion

        #region Methods
        /// <summary>
        /// Initializing script variables
        /// </summary>
        protected virtual void InitializeMethod()
        {
            CurrentHealth = health;
        }

        #region Health
        /// <summary>
        /// Sending a RPC to trigger a feedback over the network
        /// </summary>
        [PunRPC]
        public void HitFeedbackRPC()
        {
            _animator.SetTrigger("Hit");
        }
        #endregion

        #endregion

        #region Interfaces Implementation
        public void Damage(float damages)
        {
            Debug.Log($"Took {damages} damages");
            View.RPC("HitFeedbackRPC", RpcTarget.All);
        }
        #endregion
    }
}
