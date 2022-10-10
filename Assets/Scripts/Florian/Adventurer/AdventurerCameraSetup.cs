using UnityEngine;
using Cinemachine;

public class AdventurerCameraSetup : MonoBehaviour
{
    public Camera mainCam;
    public CinemachineVirtualCamera mainVCam;
    public CinemachineVirtualCamera aimVCam;

    public void SetLookAt(Transform target)
    {
        //Main
        mainVCam.Follow = target;
        mainVCam.LookAt = target;
        //Aiù
        aimVCam.Follow = target;
        aimVCam.LookAt = target;
    }
}
