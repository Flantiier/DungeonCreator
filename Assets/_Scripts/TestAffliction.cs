using UnityEngine;
using _Scripts.Interfaces;
using _ScriptableObjects.Afflictions;

public class TestAffliction : MonoBehaviour
{
    [SerializeField] private AfflictionStatus affliction;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.TryGetComponent(out IPlayerAfflicted player))
            return;

        player.TouchedByAffliction(affliction);
    }
}