using UnityEngine;
using _Scripts.Hitboxs_Triggers.Hitboxs;
using _Scripts.Interfaces;

public class testhitbox : Hitbox
{
    public float damage = 50f;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.TryGetComponent(out IDamageable damageable))
            return;

        damageable.Damage(damage);
    }
}
