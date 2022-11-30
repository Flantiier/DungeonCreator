using UnityEngine;
using UnityEngine.UI;
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
        [SerializeField] private TMP_Text currentRoomName;

        [Tooltip("Le bouton permettant au master client de lancer la game")]
        [SerializeField] private GameObject playButton;
        #endregion

        #region MonoBehaviour CallBacks
        private void Start()
        {
            PhotonNetwork.AutomaticallySyncScene = true;

            currentRoomName.text = "Nom de la Room : " + PhotonNetwork.CurrentRoom.Name.ToString();
        }

        private void Update()
        {
            if(PhotonNetwork.IsMasterClient && PhotonNetwork.CurrentRoom.PlayerCount == PhotonNetwork.CurrentRoom.MaxPlayers)
            {
                playButton.SetActive(true);
            }
            else
            {
                playButton.SetActive(false);
            }
        }

        #endregion

        #region Menu Room

        public override void OnConnectedToMaster()
        {
            PhotonNetwork.JoinLobby();
        }

        public void OnClickPlayButton()
        {
            PhotonNetwork.LoadLevel("GameScene");
        }

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