using UnityEngine;
using Photon.Pun;

public class NetworkCamera : MonoBehaviour
{
    #region Variables
    [Header("References")]
    [SerializeField, Tooltip("Referencing the playerCamera")]
    private GameObject camSetup;
    //PView Comp
    private PhotonView _view;
    #endregion

    #region Builts-In
    private void Awake()
    {
        //Get View comp
        _view = GetComponent<PhotonView>();

        //Not local
        if (!_view.IsMine)
            Destroy(camSetup);
    }
    #endregion
}
