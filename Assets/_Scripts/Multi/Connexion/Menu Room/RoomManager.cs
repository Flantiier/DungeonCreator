using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

namespace _Scripts.Multi.Connexion
{
    public class RoomManager : MonoBehaviourPunCallbacks
    {
        #region Variables
        [Tooltip("Le text permettant d'afficher le nom de la room actuelle")]
        [SerializeField] private TMP_Text _currentRoomName;
        #endregion

        #region MonoBehaviour CallBacks
        private void Start()
        {
            _currentRoomName.text = PhotonNetwork.CurrentRoom.Name.ToString();
        }

        #endregion

        #region Menu Room

        public override void OnLeftRoom()
        {
            PhotonNetwork.LoadLevel(0);
        }

        public void LeaveRoom()
        {
            PhotonNetwork.LeaveRoom();
        }

        #endregion
    }
}