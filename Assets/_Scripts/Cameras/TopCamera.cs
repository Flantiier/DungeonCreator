using UnityEngine;
using Cinemachine;
using _ScriptableObjects.Cinemachine;

namespace _Scripts.Cameras
{
	public class TopCamera : GameplayCamera
	{
        #region Variables/Properties
        [SerializeField] private TopCameraProperties cameraProperties;
        private CinemachineTransposer _transposer;
        
        public float CameraHeight => _transposer.m_FollowOffset.y;
        #endregion

        #region Builts_In
        private void Awake()
        {
            _transposer = VCam.GetCinemachineComponent<CinemachineTransposer>();
        }
        #endregion

        #region Methods
        protected override void SetCameraProperties()
        {
            if (!cameraProperties)
                return;

            cameraProperties.transposer.SetTranposer(vCam.GetCinemachineComponent<CinemachineTransposer>());
            cameraProperties.composer.SetComposer(vCam.GetCinemachineComponent<CinemachineComposer>());
        }

        /// <summary>
        /// Set the Y track offset of the camera
        /// </summary>
        public void SetCameraHeight(float value)
        {
            if (!VCam)
                return;

            _transposer.m_FollowOffset.y = value;
        }
        #endregion
    }
}
