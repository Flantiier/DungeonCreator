using UnityEngine;

namespace _Scripts.Weapons.Projectiles
{
	public class AdventurerProjectile : Projectile
	{
        #region Inherited Methods
        protected override void HandleCollision(Collider other)
        {
            if (other.TryGetComponent(out IDamageable damageable))
                damageable.Damage(damages);

            base.HandleCollision(other);
        }
        #endregion
    }
}
