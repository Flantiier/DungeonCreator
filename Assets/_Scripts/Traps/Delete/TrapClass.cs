using System.Collections;
using UnityEngine;

public  class TrapClass : MonoBehaviour
{
    #region Trap Variables
    public enum TrapType
    {
        Ground,
        Wall,
        Both
    }

    //protected TrapHitbox hitbox;
    [SerializeField] protected float trapDamage;
    [SerializeField] protected float trapCooldown;
    [SerializeField] protected TrapType trapType;

    private bool _Used;
    #endregion

    #region Trap Methods
    public IEnumerator StartCooldown()
    {
        _Used = true;
        ActivateTrap();

        yield return new WaitForSecondsRealtime(trapCooldown);

        DeactivateTrap();
        _Used = false;
    }

    public virtual void ActivateTrap() { }
    public virtual void DeactivateTrap() { }
    #endregion
}
