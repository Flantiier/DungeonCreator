using UnityEngine;

public class SwappableWeapon : NetworkMonoBehaviour
{
    #region
    [Header("Weapon info")]
    [SerializeField] private Transform holdingPoint;
    [SerializeField] private Transform sheathPoint;
    #endregion

    #region Builts_In
    public override void Awake()
    {
        SetView(gameObject);
    }
    #endregion

    #region Methods
    /// <summary>
    /// Set the weapon's position and rotation to the holdingPoint position and rotation
    /// </summary>
    [ContextMenu("Hold weapon")]
    public void HoldWeapon()
    {
        SwapPoint(holdingPoint);
    }

    /// <summary>
    /// Set the weapon's position and rotation to the sheathPoint position and rotation
    /// </summary>
    [ContextMenu("Sheath weapon")]
    public void SheathWeapon()
    {
        SwapPoint(sheathPoint);
    }

    /// <summary>
    /// Set the position and rotation of this transform to the targetTransform values
    /// </summary>
    /// <param name="targetPoint"> Target transform </param>
    private void SwapPoint(Transform targetPoint)
    {
        transform.SetParent(targetPoint);

        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
    }
    #endregion
}
