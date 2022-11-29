using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using Photon.Pun;
using Photon.Realtime;
using Hashtable = ExitGames.Client.Photon.Hashtable;

namespace _Scripts.Multi.Connexion
{
    public class LobbyManager : MonoBehaviourPunCallbacks
    {
        #region Variables
        [Header("Cr�ation de Rooms")]
        [Tooltip("Le nombre max de joueurs par Room")]
        [SerializeField] public byte maxPlayersPerRoom = 4;

        [Tooltip("Le text permettant de r�cup�rer le nom de la room cr��e")]
        [SerializeField] private TMP_Text roomName;

        [Tooltip("Le text permettant de r�cup�rer le mot de passe de la room cr��e")]
        [SerializeField] private GameObject roomPassword;

        [Tooltip("Le bouton Toggle permettant de r�cup�rer l'information si la room est priv�e ou non")]
        [SerializeField] private Toggle privateToggle;
        #endregion

        #region MonoBehaviour CallBacks

        void Start()
        {
            PhotonNetwork.AutomaticallySyncScene = true;

            roomPassword.SetActive(false);
            privateToggle.isOn = true;

            privateToggle.onValueChanged.AddListener(delegate { ToggleValueChange(privateToggle); });
        }

        #endregion

        #region Menu Lobby

        public override void OnConnectedToMaster()
        {
            PhotonNetwork.JoinLobby();
        }

        public void CreatedRoomForList()
        {
            if (!PhotonNetwork.IsConnected) return;

            RoomOptions roomOptions = new RoomOptions();
            roomOptions.IsOpen = true;
            roomOptions.IsVisible = true;
            roomOptions.MaxPlayers = maxPlayersPerRoom;

            if (privateToggle.isOn == false)
            {
                //roomOptions.IsOpen = false;

                string roomPasswordText = roomPassword.GetComponent<TMP_InputField>().text.ToString();

                Hashtable tablePassword = new Hashtable
                {
                    ["pwd"] = roomPasswordText
                };

                roomOptions.CustomRoomProperties = tablePassword;

                roomOptions.CustomRoomPropertiesForLobby = new string[] { "pwd" };

                PhotonNetwork.LocalPlayer.CustomProperties = tablePassword;
            }

            if (roomName.text.Length <= 1)
            {
                string roomNameRandom = "Room " + Random.Range(1, 100);
                PhotonNetwork.JoinOrCreateRoom(roomNameRandom, roomOptions, TypedLobby.Default);
            }
            else
            {
                PhotonNetwork.JoinOrCreateRoom(roomName.text, roomOptions, TypedLobby.Default);
            }
        }
        public override void OnCreatedRoom()
        {
            if (!PhotonNetwork.IsMasterClient) return;

            PhotonNetwork.LoadLevel("Menu_Room");
        }

        public void ToggleValueChange(Toggle privateToggle)
        {
            if(privateToggle.isOn)
            {
                roomPassword.SetActive(false);
            }
            else 
            {
                roomPassword.SetActive(true);
            }
        }

        public override void OnLeftLobby()
        {
            PhotonNetwork.LoadLevel(0);
        }

        public void LeaveLobby()
        {
            PhotonNetwork.LeaveLobby();
        }

        #endregion
    }
}