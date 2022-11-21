using System.Collections;
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

        #region Object Destruction
        /// <summary>
        /// Will send the rpc to the master after a referenced delay
        /// </summary>
        /// <param name="view"> Photon view of the object </param>
        /// <param name="delay"> Destroy delay </param>
        public void DestroyWithDelay(PhotonView view, float delay)
        {
            StartCoroutine(DestroyWithDelayRoutine(view, delay));
        }

        /// <summary>
        /// Destroy coroutine
        /// </summary>
        /// <param name="view"> Target photon view </param>
        /// <param name="delay"> Destroy delay </param>
        private IEnumerator DestroyWithDelayRoutine(PhotonView view, float delay)
        {
            yield return new WaitForSecondsRealtime(delay);

            View.RPC("DestroyObject", RpcTarget.MasterClient, view.ViewID);
        }

        /// <summary>
        /// Destroy RPC executed by the master client by finding a photon view 
        /// </summary>
        /// <param name="viewID"> View ID of the photon view </param>
        [PunRPC]
        public void DestroyObject(int viewID)
        {
            PhotonNetwork.Destroy(PhotonView.Find(viewID).gameObject);
        }

        #endregion

        #endregion
    }
}
