using UnityEngine;
using Cinemachine;
using _ScriptableObjects.Settings.Adventurer.Camera;
using _Scripts.Managers;

namespace _Scripts.Characters.Cameras
{
    public class TpsCameraProfile : CameraSetup
    {
        #region Variables
        [SerializeField] protected TpsCameraSettings cameraSettings;
        [SerializeField] protected CinemachineVirtualCamera tpsCam;

        protected CinemachineFramingTransposer _tpsBodyProperties;
        protected CinemachinePOV _tpsAimProperties;
        protected CinemachineInputProvider _inputProvider;

        public TpsCameraSettings CameraSettings => cameraSettings;
        public float CameraDistance => _tpsBodyProperties.m_CameraDistance;
        #endregion

        #region Builts_In
        private void Awake()
        {
            _tpsBodyProperties = tpsCam.GetCinemachineComponent<CinemachineFramingTransposer>();
            _tpsAimProperties = tpsCam.GetCinemachineComponent<CinemachinePOV>();
            _inputProvider = tpsCam.GetComponent<CinemachineInputProvider>();
        }

        private void OnEnable()
        {
            GameUIManager.Instance.OnOptionsMenuChanged += ctx => EnableInputs(!ctx);
        }

        private void OnDisable()
        {
            GameUIManager.Instance.OnOptionsMenuChanged -= ctx => EnableInputs(!ctx);
        }
        #endregion

        #region Inherited Methods
        public override void SetLookAtTarget(Transform target)
        {
            tpsCam.LookAt = target;
            tpsCam.Follow = target;
        }

        protected override void CameraUpdate()
        {
            cameraSettings.SetSensitivity(_tpsAimProperties, cameraSettings.sensivity);
            cameraSettings.SetBodyProperties(_tpsBodyProperties, cameraSettings.tpsMaxCameraDistance, cameraSettings.tpsCamProperties);
            cameraSettings.SetAimProperties(_tpsAimProperties, cameraSettings.tpsCamProperties);
            cameraSettings.SetRecenteringProperties(_tpsAimProperties);
        }
        #endregion

        #region Methods
        /// <summary>
        /// Enable or disable input provider
        /// </summary>
        private void EnableInputs(bool state)
        {
            if (!_inputProvider)
                return;

            _inputProvider.enabled = state;
        }

        /// <summary>
        /// Enable camera recentering on X and Y axis
        /// </summary>
        /// <param name="state"> enable state </param>
        public virtual void EnableRecentering(bool state)
        {
            _tpsAimProperties.m_HorizontalRecentering.m_enabled = state;
            _tpsAimProperties.m_VerticalRecentering.m_enabled = state;
            EnableInputs(!state);
        }
        #endregion
    }
}
