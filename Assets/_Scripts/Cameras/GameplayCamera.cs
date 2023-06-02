using UnityEngine;
using Cinemachine;
using Sirenix.OdinInspector;

namespace _Scripts.Cameras
{
    [ExecuteInEditMode]
    public class GameplayCamera : MonoBehaviour
    {
        [TitleGroup("References")]
        [SerializeField] protected Camera cam;
        [TitleGroup("References")]
        [SerializeField] protected CinemachineVirtualCamera vCam;
        [TitleGroup("Edit properties")]
        [SerializeField] private bool updateInEditMode = false;

        public Transform CameraTransform => cam.transform;
        public CinemachineVirtualCamera VCam => vCam;

        private void Update()
        {
#if UNITY_EDITOR
            if (!updateInEditMode || !vCam)
                return;

            SetCameraProperties();
#endif
        }

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
}