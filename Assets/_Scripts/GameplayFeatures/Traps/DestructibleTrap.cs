using UnityEngine;
using Photon.Pun;
using _Scripts.Interfaces;

namespace _Scripts.GameplayFeatures.Traps
{
    public class DestructibleTrap : TrapClass1, IDamageable
    {
        #region Properties
        public float CurrentHealth { get; protected set; }
        #endregion

        #region Interfaces Implementation
        public void Damage(float damages)
        {
            Debug.Log($"{gameObject} got damaged by {damages} damages");
            HandleDamages(damages);
        }
        #endregion

        #region Methods
        /// <summary>
        /// Calls the rpc to set the health value to teh given value
        /// </summary>
        protected virtual void SetTrapHealth(float amount)
        {
            Debug.Log($"Take Damages : {amount}");
            RPCCall("SetTrapHealthRPC", RpcTarget.AllViaServer, amount);
        }

        /// <summary>
        /// Set the health to the given value
        /// </summary>
        [PunRPC]
        public void SetTrapHealthRPC(float healthAmount)
        {
            CurrentHealth = healthAmount;

            if (CurrentHealth <= 0)
                HandleTrapDestruction();
        }

        /// <summary>
        /// Deal damage to this trap
        /// </summary>
        protected virtual void HandleDamages(float damages)
        {
            CurrentHealth = CurrentHealth - damages <= 0 ? 0f : CurrentHealth - damages;
            SetTrapHealth(CurrentHealth);
        }

        /// <summary>
        /// Called when the current health is at 0
        /// </summary>
        protected virtual void HandleTrapDestruction()
        {
            gameObject.SetActive(false);
        }
        #endregion
    }
}