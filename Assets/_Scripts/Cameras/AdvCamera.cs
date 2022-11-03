using UnityEngine;
using Cinemachine;

namespace _Scripts.Characters.Cameras
{
    public class AdvCamera : CameraSetup
    {
        #region Variables

        [SerializeField] private CinemachineVirtualCamera tpsCam;
        [SerializeField] private CinemachineVirtualCamera aimCam;

        [Header("Camera Settings")]
        [SerializeField] private AdvCameraSettings camSettings;

        private CinemachinePOV _tpsPov;
        private CinemachinePOV _aimPov;
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

            _tpsPov = tpsCam.GetCinemachineComponent<CinemachinePOV>();
            _aimPov = aimCam.GetCinemachineComponent<CinemachinePOV>();
        }

        public void Update()
        {
            try
            {
                camSettings.SetSensivities(_tpsPov, _aimPov);
                camSettings.SetBodyProperties(tpsCam.GetCinemachineComponent<CinemachineFramingTransposer>());
                camSettings.SetAimProperties(_aimPov);
            }
            catch
            {
                Debug.LogError("Check if body is a FRAMINGTRANSPOSER and the aim is a POV");
            }
        }
        #endregion

        #region Methods
        /// <summary>
        /// Set camera lookAt target
        /// </summary>
        /// <param name="target"> Target to look at </param>
        public void SetCameraInfos(Transform target)
        {
            tpsCam.Follow = target;
            tpsCam.LookAt = target;

            aimCam.Follow = target;
            aimCam.LookAt = target;
        }

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
            _tpsPov.m_HorizontalAxis.Value = _aimPov.m_HorizontalAxis.Value;
            _tpsPov.m_VerticalAxis.Value = _aimPov.m_VerticalAxis.Value;
        }

        /// <summary>
        /// Recenter AimCamera on updated state
        /// </summary>
        public void RecenterAimCamera()
        {
            _aimPov.m_HorizontalAxis.Value = _tpsPov.m_HorizontalAxis.Value;
            _aimPov.m_VerticalAxis.Value = _tpsPov.m_VerticalAxis.Value;
        }
        #endregion
    }
}
