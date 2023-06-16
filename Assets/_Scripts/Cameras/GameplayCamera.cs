using UnityEngine;
using Cinemachine;
using Sirenix.OdinInspector;

namespace _Scripts.Cameras
{
    [ExecuteInEditMode]
    public class GameplayCamera : MonoBehaviour
    {
        #region Variables
        [TitleGroup("References")]
        [SerializeField] protected Camera cam;
        [TitleGroup("References")]
        [SerializeField] protected CinemachineVirtualCamera vCam;
        [TitleGroup("Edit properties")]
        [SerializeField] protected bool updateInEditMode = false;
        #endregion

        #region Properties
        public Transform CameraTransform => cam.transform;
        public CinemachineVirtualCamera VCam => vCam;
        #endregion

        #region Builts_In
        protected virtual void Update()
        {
#if UNITY_EDITOR
            if (!updateInEditMode || !vCam)
                return;

            SetCameraProperties();
#endif
        }
        #endregion

        #region Methods
        /// <summary>
        /// Set the lookAt target of the virtual camera
        /// </summary>
        /// <param name="lookAt"> Transform to look at </param>
        public void SetLookAt(Transform lookAt)
        {
            vCam.Follow = lookAt;
            vCam.LookAt = lookAt;
        }

        /// <summary>
        /// Configures camera properties with a scriptable object
        /// </summary>
        protected virtual void SetCameraProperties() { }
    }
    #endregion
}