using UnityEngine;
using Cinemachine;

public class AdventurerCameraSetup : MonoBehaviour
{
    public Camera mainCam;
    public CinemachineFreeLook mainVCam;
    public CinemachineFreeLook aimVCam;

    public void SetLookAt(Transform target)
    {
        //Main
        mainVCam.Follow = target;
        mainVCam.LookAt = target;
        //Aim
        //aimVCam.Follow = target;
        //aimVCam.LookAt = target;
    }
}
