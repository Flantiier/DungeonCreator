using System.Collections;
using UnityEngine;
using Photon.Pun;

namespace _Scripts.NetworkScript
{
    [RequireComponent(typeof(PhotonView))]
    public class NetworkMonoBehaviour : MonoBehaviourPunCallbacks
    {
        #region Variables
        [Header("Network requirements")]
        [SerializeField] private PhotonView view;

        public PhotonView View => view;
        #endregion

        #region Methods

        #region Network State
        /// <summary>
        /// Method to call RPC on the object
        /// </summary>
        /// <param name="methodName"> RPC method name </param>
        /// <param name="target"> RPC Target type </param>
        /// <param name="parameters"> RPC method parameters </param>
        public void RPCCall(string methodName, RpcTarget target, params object[] parameters)
        {
            if (!PhotonNetwork.IsConnected)
                return;

            view.RPC(methodName, target, parameters);
        }

        /// <summary>
        /// Indicates if the view component on the root object is mine
        /// </summary>
        public bool ViewIsMine()
        {
            return View.IsMine;
        }
        #endregion

        #region RPC Debugging
        /// <summary>
        /// Debug method over the network by selecting a target
        /// </summary>
        /// <param name="rpcTargets"> Target rype </param>
        /// <param name="rpcMessage"> Debug message </param>
        public void RPCDebugging(RpcTarget rpcTargets, string rpcMessage)
        {
            View.RPC("DebugRPC", rpcTargets, rpcMessage);
        }

        /// <summary>
        /// Sending a rpc with a debug message
        /// </summary>
        [PunRPC]
        public void DebugRPC(string rpcMessage)
        {
            Debug.Log(rpcMessage);
        }
        #endregion

        #region Object Destruction
        /// <summary>
        /// Will send the rpc to the master after a referenced delay
        /// </summary>
        /// <param name="view"> Photon view of the object </param>
        /// <param name="delay"> Destroy delay </param>
        public void RPCDestroyWithDelay(PhotonView view, float delay)
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
