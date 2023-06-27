using System.Collections;
using UnityEngine;

public class RandomSound : MonoBehaviour
{
    [SerializeField] private float minTime = 120f, maxTime = 300f;
    [SerializeField] private AudioClip[] clips;

    private AudioSource _audioSource;

    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        StartCoroutine(PlaySoundDelayed());
    }

    private IEnumerator PlaySoundDelayed()
    {
        int index = Random.Range(0, clips.Length);
        float time = Random.Range(minTime, maxTime);
        yield return new WaitForSecondsRealtime(time);

        _audioSource.PlayOneShot(clips[index]);
        StartCoroutine(PlaySoundDelayed());
    }
}
