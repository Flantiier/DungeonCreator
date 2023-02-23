using UnityEngine;
using Cinemachine;
using _ScriptableObjects.Settings.Adventurer.Camera;
using Sirenix.OdinInspector;

namespace _Scripts.Characters.Cameras
{
    public class TpsCamera : MonoBehaviour
    {
        #region Variables
        [TitleGroup("Cinmeachine References")]
        [Required, SerializeField] private CinemachineVirtualCamera vCam; 
        [Required, SerializeField] private CinemachineInputProvider inputProvider;

        [TitleGroup("Settings")]
        [SerializeField] private VirtualCameraSettings cameraSettings;
        #endregion

        #region Properties
        public VirtualCameraSettings CameraSettings => cameraSettings;
        #endregion

        #region Camera Methods
        /// <summary>
        /// Set the lookAtTarget of the virtual camera
        /// </summary>
        /// <param name="target"> Target to look at </param>
        public void SetLookAtTarget(Transform target)
        {
            vCam.LookAt = target;
            vCam.Follow = target;
        }

        /// <summary>
        /// Enable or disable input provider
        /// </summary>
        private void EnableInputProvider(bool state)
        {
            inputProvider.enabled = state;
        }
        #endregion
    }
}
