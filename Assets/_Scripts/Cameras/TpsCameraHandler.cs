using UnityEngine;
using Cinemachine;

namespace _Scripts.Characters.Cameras
{
    public class TpsCameraHandler : CameraSetup
    {
        #region Variables
        [SerializeField] protected CinemachineVirtualCamera tpsCam;

        protected CinemachineFramingTransposer _tpsBodyProperties;
        protected CinemachinePOV _tpsAimProperties;
        #endregion

        #region Builts_In
        public override void Awake()
        {
            base.Awake();

            _tpsBodyProperties = tpsCam.GetCinemachineComponent<CinemachineFramingTransposer>();
            _tpsAimProperties = tpsCam.GetCinemachineComponent<CinemachinePOV>();
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
        }
        #endregion
    }
}
