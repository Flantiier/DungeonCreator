using UnityEngine;
using Sirenix.OdinInspector;
using Photon.Pun;
using _Scripts.Interfaces;

namespace _Scripts.GameplayFeatures.Traps
{
    public class DestructibleTrap : TrapClass1, IDamageable
    {
        #region Variables
        [TitleGroup("Properties")]
        [SerializeField] protected float health = 50f;

        public float CurrentHealth { get; protected set; }
        #endregion

        #region Builts_In
        public override void OnEnable()
        {
            if (!ViewIsMine())
                return;

            SetTrapHealth(health);
        }
        #endregion

        #region Interfaces Implementation
        public void Damage(float damages)
        {
            if (!ViewIsMine())
                return;

            HandleDamages(damages);
        }
        #endregion

        #region Methods
        /// <summary>
        /// Calls the rpc to set the health value to teh given value
        /// </summary>
        protected virtual void SetTrapHealth(float amount)
        {
            RPCCall("SetTrapHealthRPC", RpcTarget.AllViaServer, amount);
        }

        /// <summary>
        /// Set the health to the given value
        /// </summary>
        [PunRPC]
        public void SetTrapHealthRPC(float healthAmount)
        {
            CurrentHealth = healthAmount;
        }

        /// <summary>
        /// Deal damage to this trap
        /// </summary>
        protected virtual void HandleDamages(float damages)
        {
            CurrentHealth = CurrentHealth - damages <= 0 ? 0f : CurrentHealth - damages;

            if (CurrentHealth <= 0)
            {
                HandleTrapDestruction();
                return;
            }

            SetTrapHealth(CurrentHealth);

        }

        /// <summary>
        /// Called when the current health is at 0
        /// </summary>
        protected virtual void HandleTrapDestruction() { }
        #endregion
    }
}