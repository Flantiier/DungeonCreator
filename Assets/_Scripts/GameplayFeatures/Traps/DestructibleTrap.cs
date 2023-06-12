﻿using UnityEngine;
using Photon.Pun;
using Sirenix.OdinInspector;
using _Scripts.Interfaces;

namespace _Scripts.GameplayFeatures.Traps
{
    public class DestructibleTrap : TrapClass1, IDamageable
    {
        #region Variables/Properties
        [FoldoutGroup("Dissolve properties")]
        [SerializeField] protected float dissolveSpeed = 0.04f;
        private const string DISSOLVE_PARAM = "_dissolve";
        protected bool _isVisible = false;

        public float CurrentHealth { get; protected set; }
        #endregion

        #region Builts_In
        public override void OnEnable()
        {
            base.OnEnable();

            if (ViewIsMine() || !_sharedMaterial.HasFloat(DISSOLVE_PARAM))
                return;

            _sharedMaterial.SetFloat(DISSOLVE_PARAM, 0f);
        }

        protected virtual void Update()
        {
            if (ViewIsMine())
                return;

            SetDissolveAmount();
        }
        #endregion

        #region Interfaces Implementation
        public void Damage(float damages)
        {
            Debug.Log($"{gameObject} got damaged by {damages} damages");
            HandleDamages(damages);
        }
        #endregion

        #region Invisibility Methods
        /// <summary>
        /// Set the dissolve property of the material
        /// </summary>
        protected virtual void SetDissolveAmount()
        {
            if (!_sharedMaterial.HasFloat(DISSOLVE_PARAM))
                return;

            float amount = _sharedMaterial.GetFloat(DISSOLVE_PARAM);

            if (_isVisible && amount < 1)
                amount = Mathf.Lerp(amount, 1f, dissolveSpeed);
            else if (!_isVisible && amount > 0)
                amount = Mathf.Lerp(amount, 0f, dissolveSpeed);

            _sharedMaterial.SetFloat(DISSOLVE_PARAM, amount);
        }

        /// <summary>
        /// Enable or disable the dissolve
        /// </summary>
        protected void SetVisbility(bool enabled)
        {
            _isVisible = enabled;
        }
        #endregion

        #region Health Methods
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