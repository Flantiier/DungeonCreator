using UnityEngine;
using _Scripts.Interfaces;

public class TestPlayerDamage : MonoBehaviour, IPlayerDamageable
{
    public float health = 100;
    public float currentHealth;

    public void Awake()
    {
        currentHealth = health;
    }

    public void DealDamage(float damages)
    {
        currentHealth -= damages;
        Debug.Log($"Hit {currentHealth}");
    }

    public void KnockbackDamages(float damages, Vector3 hitPoint)
    {
        Debug.Log("knockback");
    }
}
