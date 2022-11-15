using UnityEngine;
using Photon.Pun;

namespace _Scripts.NetworkScript
{
    public class NetworkMonoBehaviour : MonoBehaviour
    {
        #region Variables
        [Header("Network Object")]
        [SerializeField] private PhotonView view;

        public PhotonView View => view;
        #endregion

        #region Methods
        /// <summary>
        /// Indicates if the view component on the root object is mine
        /// </summary>
        public bool ViewIsMine()
        {
            return View.IsMine;
        }
        #endregion
    }
}
