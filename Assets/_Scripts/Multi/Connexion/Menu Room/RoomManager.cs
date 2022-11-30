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

        [Tooltip("Le text ou sera affiché les éventuelles erreurs")]
        [SerializeField] private TMP_Text errorText;

        private bool isDM;
        private bool isAdventurer;
        #endregion

        #region MonoBehaviour CallBacks
        private void Start()
        {
            PhotonNetwork.AutomaticallySyncScene = true;

            playButton.SetActive(false);

            errorText.text = "";
            currentRoomName.text = "Nom de la Room : " + PhotonNetwork.CurrentRoom.Name.ToString();
        }

        private void Update()
        {
            if (PhotonNetwork.IsMasterClient && PhotonNetwork.CurrentRoom.PlayerCount >= 2)
            {
                playButton.SetActive(true);
            }

/*            CheckDMBeforeGame();
            CheckAdventurerBeforeGame();

            if (PhotonNetwork.IsMasterClient && PhotonNetwork.CurrentRoom.PlayerCount >= 2)
            {
                Debug.Log("master + 2");
                if (isAdventurer == true)
                {
                    if(isDM == true)
                    {
                        Debug.Log("true * 2");

                        playButton.SetActive(true);
                    }
                }
                else
                {
                    errorText.text = "Pour commencer la partie il faut au moins un Donjon Master et un Aventurier";
                }
            }*/
        }

        #endregion

        #region Menu Room

        public override void OnConnectedToMaster()
        {
            PhotonNetwork.JoinLobby();
        }

        public void CheckDMBeforeGame()
        {
            for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
            {
                Player player = PhotonNetwork.PlayerList[i];

                if (player.CustomProperties["dm"] != null)
                {
                    Debug.Log(player.CustomProperties["dm"].ToString());
                    if (player.CustomProperties["dm"].ToString() == "true")
                    {
                        Debug.Log("dm + true");

                        isDM = true;
                    }
                }
            }
        }

        public void CheckAdventurerBeforeGame()
        {
            for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
            {
                Player player = PhotonNetwork.PlayerList[i];
                if (player.CustomProperties["adv"] != null)
                {
                    Debug.Log("adv" + player.CustomProperties["adv"].ToString());
                    if (player.CustomProperties["adv"].ToString() == "true")
                    {
                        Debug.Log("adv + true");

                        isAdventurer = true;
                    }
                }
            }
        }

        public void OnClickPlayButton()
        {
            PhotonNetwork.LoadLevel("Cassandra");
        }

        public override void OnLeftRoom()
        {
            if(playButton != null)
            {
                playButton.SetActive(false);
            }

            PhotonNetwork.LoadLevel(0);
        }

        public void LeaveRoom()
        {
            PhotonNetwork.LeaveRoom();
        }

        #endregion
    }
}