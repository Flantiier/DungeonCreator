using UnityEngine;
using Photon.Pun;
using Sirenix.OdinInspector;
using _Scripts.Interfaces;
using _Scripts.Characters;

namespace _Scripts.GameplayFeatures.IA
{
    public class Enemy : Entity, IDamageable
    {
        [TitleGroup("Enemy properties")]
        [SerializeField] private float health = 50f;

        #region Builts_In
        public override void OnEnable()
        {
            base.OnEnable();

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
            RPCCall("HealthRPC", RpcTarget.OthersBuffered, CurrentHealth);
        }

        #region Health Methods
        protected override void HandleEntityHealth(float damages)
        {
            CurrentHealth = ClampedHealth(damages, 0f, Mathf.Infinity);
            RPCCall("HealthRPC", RpcTarget.OthersBuffered, CurrentHealth);

            if (CurrentHealth > 0)
            {
                RPCAnimatorTrigger(RpcTarget.All, "Hit", true);
                return;
            }

            HandleEntityDeath();
        }

        protected override void HandleEntityDeath()
        {
            RPCAnimatorTrigger(RpcTarget.All, "Death", true);
            RPCDestroyWithDelay(View, 3f);
        }
        #endregion

        #endregion

        #region Interfaces Implementation
        public void Damage(float damages)
        {
            if (CurrentHealth <= 0)
                return;

            HandleEntityHealth(damages);
        }
        #endregion
    }
}
