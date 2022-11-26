using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;

namespace _Scripts.Multi.Connexion
{
    public class RoomManager : MonoBehaviourPunCallbacks
    {
        #region Variables

        #endregion

        #region MonoBehaviour CallBacks

        void Start()
        {

        }

        void Update()
        {

        }

        #endregion

        #region Menu Room

        public override void OnJoinedRoom()
        {
            Debug.Log("j'ai bien rejoint la room " + PhotonNetwork.CurrentRoom.Name);
        }

        public override void OnLeftRoom()
        {
            SceneManager.LoadSceneAsync(0);
        }

        private void LeaveRoom()
        {
            PhotonNetwork.LeaveRoom();
        }

        #endregion
    }
}