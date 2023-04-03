using System;
using UnityEngine;
using _Scripts.NetworkScript;
using _Scripts.Interfaces;

namespace _Scripts.GameplayFeatures.Weapons
{
    public class Shield : NetworkMonoBehaviour, IPlayerDamageable
    {
        #region Variables
        [Header("Shield properties")]
        [SerializeField] private float maxHealth = 20f;
        public event Action OnShieldDestroyed;

        private Collider _collider;
        #endregion

        #region Properties
        public float CurrentHealth { get; private set; }
        #endregion

        #region Builts_In
        private void Awake()
        {
            _collider = GetComponent<Collider>();
            _collider.enabled = false;

            if (!ViewIsMine())
                return;

            InitializeShield();
        }
        #endregion

        #region Methods
        /// <summary>
        /// Set its health and enable state
        /// </summary>
        public void InitializeShield()
        {
            CurrentHealth = maxHealth;
        }

        /// <summary>
        /// Enable or Disable the collider
        /// </summary>
        public void EnableShield(bool state)
        {
            if (!ViewIsMine())
                return;

            _collider.enabled = state;
        }
        #endregion

        #region Interfaces Methods
        public void DealDamage(float damages)
        {
            if (!ViewIsMine())
                return;

            CurrentHealth -= damages;
            Mathf.Clamp(CurrentHealth, 0f, maxHealth);

            if (CurrentHealth > 0)
                return;

            EnableShield(false);
            OnShieldDestroyed?.Invoke();
        }

        public void KnockbackDamages(float damages, Vector3 hitPoint) { }
        #endregion
    }
}
