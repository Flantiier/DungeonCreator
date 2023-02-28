using UnityEngine;
using _Scripts.Interfaces;

namespace _Scripts.GameplayFeatures.Projectiles
{
    public class EnemiesProjectile : Projectile
    {
        #region Inherited Methods
        protected override void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out IPlayerDamageable damageable))
                damageable.DealDamage(damages);

            gameObject.SetActive(false);
        }
        #endregion
    }
}