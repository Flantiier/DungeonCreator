using UnityEngine;

public class PersistentAudioSource : PersistentSingleton<PersistentAudioSource>
{
    #region Variables
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource effectsSource;

    public bool HasClip => musicSource.clip;
    public AudioClip CurrentMusic => musicSource.clip;
    #endregion

    #region Methods
    public void PlayMusic(AudioClip clip, bool loop, bool overrideClip)
    {
        if (!overrideClip)
            return;

        if (musicSource.isPlaying && musicSource.clip)
            musicSource.Stop();

        musicSource.clip = clip;
        musicSource.loop = loop;
        musicSource.Play();
    }

    public void PlaySound(AudioClip clip)
    {
        effectsSource.PlayOneShot(clip);
    }

    public void DisableAudioSource()
    {
        if (musicSource.isPlaying && musicSource.clip) 
        {
            musicSource.Stop();
            musicSource.clip = null;
        }
    }
    #endregion
}
