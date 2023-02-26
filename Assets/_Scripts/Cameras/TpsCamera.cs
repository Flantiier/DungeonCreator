using UnityEngine;
using Cinemachine;
using Sirenix.OdinInspector;
using _ScriptableObjects.Cinemachine;

namespace _Scripts.Cameras
{
    public class TpsCamera : GameplayCamera
    {
        [TitleGroup("References")]
        [Required, SerializeField] private CinemachineInputProvider inputProvider;
        [TitleGroup("Edit properties")]
        [SerializeField] private TpsCameraProperties cameraProperties;

        /// <summary>
        /// Enable or disable input provider
        /// </summary>
        public void EnableInputProvider(bool state)
        {
            inputProvider.enabled = state;
        }

        /// <summary>
        /// Configures the virtual camera properties with scriptableObjects
        /// </summary>
        protected override void SetCameraProperties()
        {
            if (!cameraProperties)
                return;

            cameraProperties.framingTranposer.SetFramingTranposer(vCam.GetCinemachineComponent<CinemachineFramingTransposer>());
            cameraProperties.pov.SetPOV(vCam.GetCinemachineComponent<CinemachinePOV>());
        }
    }
}
