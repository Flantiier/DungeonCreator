using _Scripts.Interfaces;
using UnityEngine;

public class KnockbackBox : MonoBehaviour
{
    public void OnTriggerEnter(Collider other)
    {
        if (!other.TryGetComponent(out IPlayerDamageable player))
            return;

        player.HardDamages(1f, transform.position);
    }
}
