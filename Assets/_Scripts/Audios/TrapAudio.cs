using _Scripts.NetworkScript;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class TrapAudio : NetworkMonoBehaviour
{
    [SerializeField] private float localDistance = 0.1f;
    [SerializeField] private float otherDistance = 1f;

    private AudioSource _audioSource;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
        _audioSource.minDistance = ViewIsMine() ? localDistance : otherDistance;
    }
}
