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

        [TitleGroup("Camera Properties")]
        [SerializeField] private TpsCameraProperties cameraProperties;
        [ShowIf("cameraProperties")]
        [SerializeField] private bool updateInEditor = true;
        #endregion

        private void Update()
        {
            if (!updateInEditor || !vCam || !cameraProperties)
                return;

            cameraProperties.SetCameraProperties(vCam);
        }

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
