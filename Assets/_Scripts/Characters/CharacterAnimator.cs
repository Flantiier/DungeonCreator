using UnityEngine;
using _Scripts.Characters;

public class CharacterAnimator : MonoBehaviour
{
    [SerializeField] private CharacterHitbox[] hitboxs;
    public Character Character { get; private set; }
    public CharacterHitbox[] Hitboxs => hitboxs;

    public virtual void Awake()
    {
        Character = GetComponentInParent<Character>();
        GetHitboxs();
    }

    public void EnableCollider(int index)
    {
        Hitboxs[index].gameObject.SetActive(true);
    }
    public void DisableCollider(int index)
    {
        Hitboxs[index].gameObject.SetActive(false);
    }

    [ContextMenu("Get Hitboxs")]
    private void GetHitboxs()
    {
        hitboxs = GetComponentsInChildren<CharacterHitbox>();
    }
}

#region AdventurerStatic
public static class CharacterAnimation
{
    /// <summary>
    /// Get character script from an animator's parent object
    /// </summary>
    /// <param name="animator"> Animator to get from  </param>
    public static Character GetPlayer(Animator animator)
    {
        return animator.GetComponent<CharacterAnimator>().Character;
    }

    /// <summary>
    /// Return if the character photon view is the local one
    /// </summary>
    /// <param name="character"> Character to check </param>
    public static bool IsMyPlayer(Character character)
    {
        if (character.PViewIsMine)
            return true;

        return false;
    }
}
#endregion
