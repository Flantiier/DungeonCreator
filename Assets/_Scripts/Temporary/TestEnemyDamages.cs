using UnityEngine;
using _Scripts.Interfaces;

public class TestEnemyDamages : MonoBehaviour
{
    [SerializeField] private float damages = 25f;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.TryGetComponent(out IDamageable enemy))
            return;

        Debug.Log("Damage enemy");
        enemy.Damage(damages);
    }
}
