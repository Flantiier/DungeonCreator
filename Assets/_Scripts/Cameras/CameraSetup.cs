using UnityEngine;

namespace _Scripts.Characters.Cameras
{
    public class CameraSetup : MonoBehaviour
    {
        #region Variables
        [Header("Camera references")]
        [SerializeField] protected Camera mainCam;
        public Camera MainCam => mainCam;
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
