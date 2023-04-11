using System.Collections;
using UnityEngine;
using _Scripts.Interfaces;

public class TestTrapDefuse : MonoBehaviour, IDefusable
{
    #region Variables
    [Header("Defusable trap properties")]
    [SerializeField] private float defuseDuration = 10f;
    [SerializeField] private float disabledTime = 5f;

    private bool _isDisabled;
    #endregion

    #region Properties
    public float DefuseDuration { get => defuseDuration; }
    public bool IsDisabled { get => _isDisabled; set => _isDisabled = value; }
    #endregion

    #region Methods
    public void DefuseTrap()
    {
        StartCoroutine(DisabledCooldownRoutine());
    }

    /// <summary>
    /// Trap disabled before goes back to normal
    /// </summary>
    private IEnumerator DisabledCooldownRoutine()
    {
        IsDisabled = true;
        Debug.Log("Disabled");

        yield return new WaitForSeconds(disabledTime);

        Debug.Log("Enabled");
        IsDisabled = false;

    }
    #endregion
}
