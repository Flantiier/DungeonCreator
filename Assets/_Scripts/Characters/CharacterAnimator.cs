using UnityEngine;
using _Scripts.Characters;

public class CharacterAnimator : NetworkMonoBehaviour
{
    #region Variables
    [SerializeField] private CharacterHitbox[] hitboxs;
    [SerializeField] private SwappableWeapon mainWeapon;
    [SerializeField] private SwappableWeapon secondWeapon;
    #endregion

    #region Properties
    public Character Character { get; private set; }
    public CharacterHitbox[] Hitboxs => hitboxs;
    #endregion

    #region Builts_In
    public override void Awake()
    {
        SetView(transform.root.gameObject);

        if (!ViewIsMine())
            return;

        Character = GetComponentInParent<Character>();

        GetHitboxs();
    }
    #endregion

    #region AnimationsEvents_Methods
    /// <summary>
    /// Enable a hitbox in the colliderArray
    /// </summary>
    /// <param name="index"> hitbox's index </param>
    public void EnableCollider(int index)
    {
        if (!ViewIsMine())
            return;

        Hitboxs[index].gameObject.SetActive(true);
    }

    /// <summary>
    /// Disable a hitbox in the colliderArray
    /// </summary>
    /// <param name="index"> hitbox's index </param>
    public void DisableCollider(int index)
    {
        if (!ViewIsMine())
            return;

        Hitboxs[index].gameObject.SetActive(false);
    }

    /// <summary>
    /// Swap character weapons
    /// </summary>
    /// <param name="index"></param>
    public void SwapWeapons(int index)
    {
        switch (index)
        {
            case 1:
                mainWeapon.SheathWeapon();
                secondWeapon.HoldWeapon();
                break;

            case 0:
                mainWeapon.HoldWeapon();
                secondWeapon.SheathWeapon();
                break;
        }
    }
    #endregion

    #region Methods
    /// <summary>
    /// Searching all the hitboxs on the character
    /// </summary>
    [ContextMenu("Get Hitboxs")]
    private void GetHitboxs()
    {
        if (!ViewIsMine())
            return;

        hitboxs = GetComponentsInChildren<CharacterHitbox>();

        if (!Application.isPlaying)
            return;

        foreach (Hitbox hitbox in hitboxs)
        {
            if (!hitbox)
                continue;

            hitbox.gameObject.SetActive(false);
        }
    }
    #endregion
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
        return character.ViewIsMine();
    }
}
#endregion
