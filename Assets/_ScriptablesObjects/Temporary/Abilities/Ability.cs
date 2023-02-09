using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "New Ability", menuName = "Scriptables/Ability")]
public class Ability : ScriptableObject
{
    public AnimationClip clip;
    public float reloadTime = 5f;
    public bool Used { get; private set; }

    public IEnumerator StartReloadTimer()
    {
        Used = true;
        yield return new WaitForSecondsRealtime(reloadTime);
        Used = false;
    }
}
