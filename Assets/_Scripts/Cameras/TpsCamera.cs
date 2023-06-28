using UnityEngine;
using Cinemachine;
using Sirenix.OdinInspector;
using _ScriptableObjects.Cinemachine;
using _ScriptableObjects.GameManagement;

namespace _Scripts.Cameras
{
    public class TpsCamera : GameplayCamera
    {
        #region Variables
        [TitleGroup("References")]
        [SerializeField] private TpsCameraProperties cameraProperties;
        [TitleGroup("References")]
        [SerializeField] private GeneralSettings settings;
        
        private CinemachineInputProvider _inputProvider;
        #endregion

        #region Builts_In
        private void Start()
        {
            _inputProvider = GetComponentInChildren<CinemachineInputProvider>();
            SetCameraProperties();
        }

        protected override void Update()
        {
            base.Update();
            UpdateSensivity();
        }
        #endregion

        #region Methods
        /// <summary>
        /// Enable or disable input provider
        /// </summary>
        public void EnableInputProvider(bool state)
        {
            if(state)
                _inputProvider.XYAxis.action.Enable();
            else
                _inputProvider.XYAxis.action.Disable();
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
            pov.m_VerticalAxis.m_MaxSpeed = settings.sensitivity / 100f;
        }
        #endregion
    }
}
