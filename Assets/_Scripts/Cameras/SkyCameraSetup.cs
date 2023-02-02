using UnityEngine;
using Cinemachine;
using Sirenix.OdinInspector;
using _ScriptableObjects.DM;

namespace _Scripts.Characters.Cameras
{
	public class SkyCameraSetup : CameraSetup
	{
        #region Variables/Properties
        [SerializeField] private CinemachineVirtualCamera vCam;
        [SerializeField] private DMDatas datas;
        public CinemachineVirtualCamera VCam => vCam;
        #endregion

        #region Methods
        public override void SetLookAtTarget(Transform target)
        {
            VCam.Follow = target;
            VCam.LookAt = target;
        }

        /// <summary>
        /// Updates virtual camera transposer properties
        /// </summary>
        [ShowIf("datas")]
        [Button("Update Camera", 30), GUIColor(2, 1, 0.3f)]
        private void UpdateCamProperties()
        {
            if (!vCam || !datas)
                return;

            CinemachineTransposer transposer = vCam.GetCinemachineComponent<CinemachineTransposer>();
            //OFFSET
            transposer.m_FollowOffset = datas.followOffset;
            //DAMPING
            transposer.m_XDamping = datas.XYZDamping.x;
            transposer.m_YDamping = datas.XYZDamping.y;
            transposer.m_ZDamping = datas.XYZDamping.z;
            //YAW
            transposer.m_YawDamping = datas.YawDamping;
        }
        #endregion
    }
}
