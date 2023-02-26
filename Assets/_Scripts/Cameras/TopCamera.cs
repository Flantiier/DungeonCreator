using UnityEngine;
using Cinemachine;
using _ScriptableObjects.Cinemachine;

namespace _Scripts.Cameras
{
	public class TopCamera : GameplayCamera
	{
        [SerializeField] private TopCameraProperties cameraProperties;

        protected override void SetCameraProperties()
        {
            if (!cameraProperties)
                return;

            cameraProperties.transposer.SetTranposer(vCam.GetCinemachineComponent<CinemachineTransposer>());
            cameraProperties.composer.SetComposer(vCam.GetCinemachineComponent<CinemachineComposer>());
        }
    }
}
