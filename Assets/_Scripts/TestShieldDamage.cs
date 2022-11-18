using _Scripts.Interfaces;
using System.Collections;
using UnityEngine;

public class TestShieldDamage : MonoBehaviour
{
    public float damages = 10f;
    private Collider _collider;

    private void Awake()
    {
        _collider = GetComponent<Collider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.TryGetComponent(out IPlayerDamageable damageable))
            return;

        damageable.TakeDamages(damages);
        StartCoroutine("Timing");
    }

    private IEnumerator Timing()
    {
        _collider.enabled = false;

        yield return new WaitForSecondsRealtime(1f);

        _collider.enabled = true;
    }
}
