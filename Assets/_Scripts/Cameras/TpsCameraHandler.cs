using UnityEngine;
using Cinemachine;
using _ScriptablesObjects.Settings.Adventurer.Camera;

namespace _Scripts.Characters.Cameras
{
    public class TpsCameraHandler : CameraSetup
    {
        #region Variables
        [SerializeField] protected TpsCameraSettings cameraSettings;
        [SerializeField] protected CinemachineVirtualCamera tpsCam;

        protected CinemachineFramingTransposer _tpsBodyProperties;
        protected CinemachinePOV _tpsAimProperties;
        protected CinemachineInputProvider _inputProvider;

        public TpsCameraSettings CameraSettings => cameraSettings;
        #endregion

        #region Builts_In
        public override void Awake()
        {
            base.Awake();

            _tpsBodyProperties = tpsCam.GetCinemachineComponent<CinemachineFramingTransposer>();
            _tpsAimProperties = tpsCam.GetCinemachineComponent<CinemachinePOV>();
            _inputProvider = tpsCam.GetComponent<CinemachineInputProvider>();
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
        /// Enable camera recentering on X and Y axis
        /// </summary>
        /// <param name="state"> enable state </param>
        public virtual void EnableRecentering(bool state)
        {
            _tpsAimProperties.m_HorizontalRecentering.m_enabled = state;
            _tpsAimProperties.m_VerticalRecentering.m_enabled = state;
            _inputProvider.enabled = !state;
        }
        #endregion
    }
}
