using UnityEngine;

public class DamagingCube : MonoBehaviour, IDamageable
{
    private float _currentHealth;

    private void Awake()
    {
        _currentHealth = 1000f;
    }

    public void Damage(float damages)
    {
        _currentHealth -= damages;
    }
}
