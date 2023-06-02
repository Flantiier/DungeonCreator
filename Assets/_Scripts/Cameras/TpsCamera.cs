using UnityEngine;
using Cinemachine;
using Sirenix.OdinInspector;
using _ScriptableObjects.Cinemachine;
using _Scripts.Managers;
using _ScriptableObjects.GameManagement;

namespace _Scripts.Cameras
{
    public class TpsCamera : GameplayCamera
    {
        #region Variables
        [TitleGroup("References")]
        [SerializeField] private GeneralSettings settings;
        [TitleGroup("References")]
        [Required, SerializeField] private CinemachineInputProvider inputProvider;
        [TitleGroup("Edit properties")]
        [SerializeField] private TpsCameraProperties cameraProperties;
        #endregion

        #region Builts_In
        private void Start()
        {
            SetCameraProperties();
        }

        private void Update()
        {
            UpdateSensivity();
        }
        #endregion

        #region Methods
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
        }

        /// <summary>
        /// Update camera sensitivity
        /// </summary>
        private void UpdateSensivity()
        {
            CinemachinePOV pov = vCam.GetCinemachineComponent<CinemachinePOV>();
            pov.m_HorizontalAxis.m_MaxSpeed = settings.sensitivity / 100f;
        }
        #endregion
    }
}
