using UnityEngine;
using _Scripts.NetworkScript;
using _Scripts.Interfaces;

public class Shield : NetworkMonoBehaviour
{
    #region Variables
    [Header("Shield properties")]
    [SerializeField] private float maxHealth = 20f;

    private Collider _collider;
    private bool _isDestroyed = false;
    #endregion

    #region Properties
    public float CurrentDurability { get; private set; }
    #endregion

    #region Builts_In
    private void Awake()
    {
        _collider = GetComponent<Collider>();

        EnableShield("false");
    }
    #endregion

    #region Methods
    public void HandleShield()
    {

    }

    /// <summary>
    /// Enable or Disable the collider
    /// </summary>
    public void EnableShield(string state)
    {
        switch (state)
        {
            case "true":
                _collider.enabled = true;
                break;

            case "false":
                _collider.enabled = false;
                break;
        }
    }
    #endregion
}
