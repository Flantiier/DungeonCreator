using UnityEngine;
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

            base.OnTriggerEnter(other);
        }
        #endregion

        #region Methods
        public override void ThrowProjectile(Vector3 direction)
        {
            base.ThrowProjectile(direction);
            Destroy(gameObject, destructTime);
        }

        public override void OverrideThrowForce(Vector3 direction, float force)
        {
            base.OverrideThrowForce(direction, force);
            Destroy(gameObject, destructTime);
        }
        #endregion
    }
}