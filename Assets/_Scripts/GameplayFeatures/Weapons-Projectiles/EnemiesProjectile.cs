using UnityEngine;
using Photon.Pun;
using _Scripts.Interfaces;

namespace _Scripts.GameplayFeatures.Projectiles
{
    public class EnemiesProjectile : Projectile
    {
        public void Update()
        {
            if (!ViewIsMine())
                return;

            RPCCall("SyncRigidbody", RpcTarget.Others, _rb.position);
        }

        [PunRPC]
        protected void SyncRigidbody(Vector3 position)
        {
            _rb.position = position;
        }

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