using UnityEngine;

public class UISoundTrigger : MonoBehaviour
{
    [SerializeField] protected AudioClip[] clips;

    public void PlaySoundFromClip(AudioClip clip)
    {
        PersistentAudioSource.Instance.PlaySound(clip);
    }

    public void PlaySoundFromArray(int index)
    {
        if (index < 0 || index >= clips.Length)
            return;

        PersistentAudioSource.Instance.PlaySound(clips[index]);
    }
}
