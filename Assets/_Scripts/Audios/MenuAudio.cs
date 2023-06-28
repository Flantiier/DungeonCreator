using UnityEngine;

public class MenuAudio : PersistentSingleton<MenuAudio>
{
    private AudioSource _audioSource;

    public override void Awake()
    {
        base.Awake();
        _audioSource = GetComponent<AudioSource>();
    }

    public void PlaySound(AudioClip clip)
    {
        _audioSource.PlayOneShot(clip);
    }

    public void DestroyInstance()
    {
        Destroy(gameObject);
    }
}
