using UnityEngine;
using Cinemachine;
using Photon.Pun;

namespace _Scripts.Characters.Cameras
{
    public class AdvSimpleCamera : MonoBehaviour
    {
        #region Variables

        [SerializeField] protected Camera mainCam;
        [SerializeField] protected CinemachineFreeLook mainFreelook;

        protected PhotonView _Pview;

        #endregion

        #region Properties

        public Camera MainCam => mainCam;

        #endregion

        #region Builts_In

        public virtual void Awake()
        {
            _Pview = mainCam.GetComponent<PhotonView>();

            if (!_Pview.IsMine)
                Destroy(gameObject);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Set cinemachines camera follow and lookAt target
        /// </summary>
        /// <param name="target"> Target object </param>
        public virtual void SetCameraInfos(Transform target)
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
