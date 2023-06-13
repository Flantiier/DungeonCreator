using UnityEngine;
using _Scripts.Interfaces;

namespace _Scripts.GameplayFeatures.Projectiles
{
	public class AdventurerProjectile : Projectile
	{
        protected override void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out IDamageable damageable))
                damageable.Damage(damages);

            base.OnTriggerEnter(other);
        }
    }
}
