using _Scripts.Interfaces;
using UnityEngine;

public class DamagingBox : MonoBehaviour
{
    [SerializeField] private float damages;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.TryGetComponent(out IPlayerDamageable player))
            return;

        player.DamagePlayer(damages);
    }
}
