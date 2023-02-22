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

        [TitleGroup("Properties")]
        [SerializeField] private bool inputEnableAtStart = true;

        [TitleGroup("Settings")]
        [SerializeField] private TpsCameraSettings cameraSettings;
        #endregion

        #region Properties
        public TpsCameraSettings CameraSettings => cameraSettings;
        #endregion

        #region Builts_In
        private void Start()
        {
            EnableInputProvider(inputEnableAtStart);
        }
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
