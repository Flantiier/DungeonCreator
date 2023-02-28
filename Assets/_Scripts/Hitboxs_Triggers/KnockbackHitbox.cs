using _Scripts.Interfaces;
using UnityEngine;

namespace _Scripts.Hitboxs_Triggers.Hitboxs
{
    public class KnockbackHitbox : Hitbox
    {
        public void OnTriggerEnter(Collider other)
        {
            if (!other.TryGetComponent(out IPlayerDamageable player))
                return;

            player.KnockbackDamages(1f, transform.position);
        }
    }
}
