using UnityEngine;

public class MenuMusicPlayer : MonoBehaviour
{
    [SerializeField] private AudioClip clip;
    [SerializeField] private bool looping = true;

    private void Start()
    {
        PersistentAudioSource.Instance.PlayMusic(clip, looping, PersistentAudioSource.Instance.CurrentMusic != clip);
    }
}
