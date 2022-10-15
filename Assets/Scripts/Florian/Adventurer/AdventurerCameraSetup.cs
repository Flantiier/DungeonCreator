using UnityEngine;
using Cinemachine;
using _Scripts.Characters;
using Photon.Pun;
using Cinemachine.Editor;

public class AdventurerCameraSetup : MonoBehaviour
{
    #region Variables

    [SerializeField] private Camera mainCam;
    [SerializeField] private CinemachineFreeLook mainFreelook;
    [SerializeField] private CinemachineFreeLook aimFreelook;

    private PhotonView _Pview;

    #endregion

    #region Properties

    public Camera MainCam => mainCam;
    public CinemachineFreeLook MainFreelook => mainFreelook;
    public CinemachineFreeLook AimFreelook => aimFreelook;

    #endregion

    #region Builts_In
    private void Awake()
    {
        _Pview = mainCam.GetComponent<PhotonView>();

        if (!_Pview.IsMine)
            Destroy(gameObject);
    }
    #endregion

    #region Methods
    public void SetCameraInfos(Transform target)
    {
        MainFreelook.Follow = target;
        MainFreelook.LookAt = target;

        AimFreelook.Follow = target;
        AimFreelook.LookAt = target;
    }
    #endregion
}
