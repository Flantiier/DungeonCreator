using UnityEngine;
using Photon.Pun;
using _Scripts.Interfaces;
using _Scripts.Characters;

namespace _Scripts.GameplayFeatures.IA
{
    public class Enemy : Entity, IDamageable
    {
        #region Methods
        protected void InitializeHealth(float health)
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
