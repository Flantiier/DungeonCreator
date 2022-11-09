using UnityEngine;
using Cinemachine;

namespace _Scripts.Characters.Cameras
{
    public class AimCameraHandler : TpsCameraHandler
    {
        #region Variables
        [SerializeField] private CinemachineVirtualCamera aimCam;

        private CinemachinePOV _aimCamAimProperties;
        private bool _switchToAim;
        #endregion

        #region Properties
        public bool SwitchToAim
        {
            get => _switchToAim;
            set
            {
                if (_switchToAim == value)
                    return;

                _switchToAim = value;
                tpsCam.Priority = _switchToAim ? 8 : 10;

                if (_switchToAim)
                    RecenterAimCamera();
                else
                    RecenterTpsCamera();
            }
        }
        #endregion

        #region Builts_In
        public override void Awake()
        {
            base.Awake();

            _aimCamAimProperties = aimCam.GetCinemachineComponent<CinemachinePOV>();
        }
        #endregion

        #region Inherited Methods
        public override void SetLookAtTarget(Transform target)
        {
            base.SetLookAtTarget(target);

            aimCam.Follow = target;
            aimCam.LookAt = target;
        }

        protected override void CameraUpdate()
        {
            base.CameraUpdate();

            cameraSettings.SetSensitivity(_aimCamAimProperties, cameraSettings.aimSensivity);
        }
        #endregion

        #region Methods
        /// <summary>
        /// Switch between aim an tps cameras
        /// </summary>
        /// <param name="state"> Character aiming state </param>
        public void CameraSwitch(bool state)
        {
            SwitchToAim = state;
        }

        /// <summary>
        /// Recenter TpsCamera on updated state
        /// </summary>
        public void RecenterTpsCamera()
        {
            _tpsAimProperties.m_HorizontalAxis.Value = _aimCamAimProperties.m_HorizontalAxis.Value;
            _tpsAimProperties.m_VerticalAxis.Value = _aimCamAimProperties.m_VerticalAxis.Value;
        }

        /// <summary>
        /// Recenter AimCamera on updated state
        /// </summary>
        public void RecenterAimCamera()
        {
            _aimCamAimProperties.m_HorizontalAxis.Value = _tpsAimProperties.m_HorizontalAxis.Value;
            _aimCamAimProperties.m_VerticalAxis.Value = _tpsAimProperties.m_VerticalAxis.Value;
        }
        #endregion
    }
}
