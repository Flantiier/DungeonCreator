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
        [TitleGroup("References")]
        [SerializeField] private GeneralSettings settings;
        [TitleGroup("References")]
        [Required, SerializeField] private CinemachineInputProvider inputProvider;
        [TitleGroup("Edit properties")]
        [SerializeField] private TpsCameraProperties cameraProperties;

        public CinemachineVirtualCamera VCam { get; private set; }

        private void Start()
        {
            vCam = GetComponentInChildren<CinemachineVirtualCamera>();
            SetCameraProperties();
        }

        private void Update()
        {
            UpdateSensivity();
        }

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

        private void UpdateSensivity()
        {
            CinemachinePOV pov = vCam.GetCinemachineComponent<CinemachinePOV>();
            pov.m_HorizontalAxis.m_MaxSpeed = settings.sensitivity / 100f;
        }
    }
}
