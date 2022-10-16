using Photon.Pun;
using UnityEngine;

namespace _Scripts.Characters.Cameras
{
	public class CameraSetup : MonoBehaviour
	{
        #region Variables/Props

        [SerializeField] protected Camera mainCam;
		public Camera MainCam => mainCam;

        private PhotonView _view;

        #endregion

        #region Methods

        public virtual void Awake()
        {
            _view = GetComponent<PhotonView>();

            if (!_view.IsMine)
                Destroy(gameObject);
        }

        public virtual void SetCamera(Transform target) { }

        #endregion
    }
}
