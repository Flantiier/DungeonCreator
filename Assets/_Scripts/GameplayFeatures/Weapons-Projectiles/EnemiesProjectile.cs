using UnityEngine;
using Photon.Pun;
using _Scripts.Interfaces;

namespace _Scripts.GameplayFeatures.Projectiles
{
    public class EnemiesProjectile : Projectile
    {
        #region Builts_In
        protected override void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out IPlayerDamageable damageable))
                damageable.DealDamage(damages);

            Destroy(gameObject);
        }
        #endregion

        #region Methods
        public override void ThrowProjectile(Vector3 direction)
        {
            base.ThrowProjectile(direction);
            StartCoroutine(DelayedDestroy());
        }

        public override void OverrideThrowForce(Vector3 direction, float force)
        {
            StartCoroutine(DelayedDestroy());
            base.OverrideThrowForce(direction, force);
        }
        #endregion
    }
}