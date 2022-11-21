using UnityEngine;
using Photon.Pun;
using _Scripts.Interfaces;
using _Scripts.Characters;

namespace _Scripts.IA
{
    public class Enemy : Entity, IDamageable
    {
        #region Variables
        [Header("Enemy properties")]
        [SerializeField] protected float health = 50f;

        protected Animator _animator;
        #endregion

        #region Builts_In
        public virtual void Awake()
        {
            _animator = GetComponentInChildren<Animator>();
        }

        public virtual void OnEnable()
        {
            if (!ViewIsMine())
                return;

            InitializeEnemy();
        }
        #endregion

        #region Methods
        /// <summary>
        /// Initializing script variables
        /// </summary>
        protected virtual void InitializeEnemy()
        {
            CurrentHealth = health;
        }

        #region RPC Methods
        /// <summary>
        /// Sending a RPC to trigger a feedback over the network
        /// </summary>
        [PunRPC]
        public void DeathRPC()
        {
            _animator.SetTrigger("Death");
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

        #region Health Methods
        protected override void HandleHealth(float damages)
        {
            CurrentHealth -= damages;
            Mathf.Clamp(CurrentHealth, 0, Mathf.Infinity);

            View.RPC("HealthRPC", RpcTarget.Others, CurrentHealth);
            View.RPC("HitFeedbackRPC", RpcTarget.All);

            if (CurrentHealth > 0)
                return;

            HandleEntityDeath();
        }

        protected override void HandleEntityDeath()
        {
            View.RPC("DeathRPC", RpcTarget.All);
            DestroyWithDelay(View, 3f);
        }
        #endregion

        #endregion

        #region Interfaces Implementation
        public void Damage(float damages)
        {
            if (CurrentHealth <= 0)
                return;

            Debug.Log($"Took {damages} damages");
            HandleHealth(damages);
        }
        #endregion
    }
}
