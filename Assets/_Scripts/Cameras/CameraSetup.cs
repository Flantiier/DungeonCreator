using UnityEngine;
using Photon.Pun;

namespace _Scripts.Characters.Cameras
{
    [RequireComponent(typeof(PhotonView))]
	public class CameraSetup : NetworkMonoBehaviour
	{
        #region Variables/Props
        [Header("Camera references")]
        [SerializeField] protected Camera mainCam;
		public Camera MainCam => mainCam;
        #endregion

        #region Methods
        public override void Awake()
        {
            base.Awake();

            if (!ViewIsMine())
                Destroy(gameObject);
        }
        #endregion
    }
}
