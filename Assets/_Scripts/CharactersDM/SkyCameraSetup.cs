using UnityEngine;
using Cinemachine;
using Photon.Pun;

namespace _Scripts.Characters.Cameras
{
	public class SkyCameraSetup : CameraSetup
	{
        #region Variables/Properties

        [SerializeField] private CinemachineVirtualCamera vCam;
        public CinemachineVirtualCamera VCam => vCam;

        #endregion

        #region Methods

		public override void SetCamera(Transform target)
		{
			VCam.Follow = target;
			VCam.LookAt = target;
		}

        #endregion
    }
}
