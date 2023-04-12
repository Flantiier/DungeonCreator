using UnityEngine;
using _Scripts.GameplayFeatures.Weapons;

namespace _Scripts.Hitboxs_Triggers.Hitboxs
{
    public class EnemyWeaponHitbox : EnemyHitbox
    {
        public override void OnTriggerEnter(Collider other)
        {
            if(other.TryGetComponent(out Shield shield))
            {
                shield.DealDamage(Damages);
                Collider.enabled = false;
                return;
            }

            base.OnTriggerEnter(other);
        }
    }
}

