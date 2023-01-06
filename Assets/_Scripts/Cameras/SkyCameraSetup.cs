using UnityEngine;
using Cinemachine;

namespace _Scripts.Characters.Cameras
{
	public class SkyCameraSetup : CameraSetup
	{
        #region Variables/Properties
        [SerializeField] private CinemachineVirtualCamera vCam;
        public CinemachineVirtualCamera VCam => vCam;
        #endregion

        #region Methods
        public override void SetLookAtTarget(Transform target)
        {
            VCam.Follow = target;
            VCam.LookAt = target;
        }
        #endregion
    }
}
