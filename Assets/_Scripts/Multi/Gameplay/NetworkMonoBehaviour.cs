using UnityEngine;
using Photon.Pun;

namespace _Scripts.NetworkScript
{
    public class NetworkMonoBehaviour : MonoBehaviour
    {
        #region Variables
        public PhotonView View { get; private set; }
        #endregion

        #region Builts_In
        public virtual void Awake()
        {
            SetView(gameObject);
        }
        #endregion

        #region Methods
        /// <summary>
        /// Set the view component
        /// </summary>
        /// <param name="root"> root object to get from </param>
        public void SetView(GameObject root)
        {
            if (!root.TryGetComponent(out PhotonView view))
            {
                Debug.Log($"{gameObject} missing photon view reference");
                return;
            }

            View = view;
        }

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
