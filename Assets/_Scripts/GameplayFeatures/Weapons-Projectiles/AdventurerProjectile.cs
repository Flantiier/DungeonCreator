using UnityEngine;
using Photon.Pun;
using _Scripts.Interfaces;

namespace _Scripts.GameplayFeatures.Projectiles
{
    public class AdventurerProjectile : Projectile
    {
        #region Buitls_In
        protected override void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out IDamageable damageable))
                damageable.Damage(damages);

            if(ViewIsMine())
                PhotonNetwork.Destroy(View);
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
            base.OverrideThrowForce(direction, force);
            StartCoroutine(DelayedDestroy());
        }
        #endregion
    }
}
