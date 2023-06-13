using UnityEngine;
using _Scripts.Interfaces;

namespace _Scripts.GameplayFeatures.Projectiles
{
	public class AdventurerProjectile : Projectile
	{
        #region Inherited Methods
        protected override void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out IDamageable damageable))
                damageable.Damage(damages);

            DestroyObject(View);
        }
        #endregion
    }
}
