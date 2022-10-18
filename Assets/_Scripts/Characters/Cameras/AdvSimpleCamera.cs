using UnityEngine;
using Cinemachine;

namespace _Scripts.Characters.Cameras
{
    public class AdvSimpleCamera : CameraSetup
    {
        #region Variables

        [SerializeField] protected CinemachineFreeLook mainFreelook;

        #endregion

        #region Methods

        public override void SetCamera(Transform target)
        {
            mainFreelook.Follow = target;
            mainFreelook.LookAt = target;
        }

        /// <summary>
        /// Method to switch between main and aim camera
        /// </summary>
        public virtual void SwicthToAim(int mainPriority, int aimPriority) { }

        #endregion
    }
}
