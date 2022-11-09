using UnityEngine;
using Photon.Pun;

namespace _Scripts.Characters.Cameras
{
    [RequireComponent(typeof(PhotonView))]
    public class CameraSetup : NetworkMonoBehaviour
    {
        #region Variables/Props
        [Header("Camera Settings")]
        [SerializeField] protected AdvCameraSettings cameraSettings;

        [Header("Camera references")]
        [SerializeField] protected Camera mainCam;
        public Camera MainCam => mainCam;
        #endregion

        #region Builts_In
        public override void Awake()
        {
            base.Awake();

            if (!ViewIsMine())
                Destroy(gameObject);
        }

        public virtual void Update()
        {
            try
            {
                CameraUpdate();
            }
            catch
            {
                Debug.LogError("Camera has a problem, maybe a missing reference ???");
            }
        }
        #endregion

        #region Inherited Methods
        /// <summary>
        /// Setting the lookAt target of cinemachine cameras
        /// </summary>
        /// <param name="target"> Transform to look at </param>
        public virtual void SetLookAtTarget(Transform target) { }

        /// <summary>
        /// Inherited update method
        /// </summary>
        protected virtual void CameraUpdate() { }
        #endregion
    }
}
