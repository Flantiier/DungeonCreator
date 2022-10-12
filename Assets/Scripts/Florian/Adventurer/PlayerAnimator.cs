using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    protected PlayerController _controller;
    public PlayerController Controller => _controller;

    public virtual void Awake()
    {
        _controller = GetComponentInParent<PlayerController>();
    }
}

#region AdventurerStatic
public static class AdvStaticAnim
{
    public static PlayerController GetPlayer(Animator animator)
    {
        return animator.GetComponent<PlayerAnimator>().Controller;
    }
}
#endregion
