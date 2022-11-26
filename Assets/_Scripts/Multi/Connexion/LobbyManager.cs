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
        [Header("Création de Rooms")]
        [Tooltip("Le nombre max de joueurs par Room")]
        [SerializeField] private byte maxPlayersPerRoom = 4;

        [Tooltip("Le text permettant de récupérer le nom de la room créée")]
        [SerializeField] private TMP_Text roomName;

        [Tooltip("Le text permettant de récupérer le nom de la room créée")]
        [SerializeField] private TMP_Text roomPasswordText;

        [Tooltip("Le text permettant de récupérer le mot de passe de la room créée")]
        [SerializeField] private GameObject roomPassword;

        [Tooltip("Le bouton Toggle permettant de récupérer l'information si la room est privée ou non")]
        [SerializeField] private Toggle privateToggle;

        [Header("Affichages Rooms")]
        [Tooltip("Le menu a afficher lors de la création de room")]
        [SerializeField] private GameObject createOrJoinRoom;

        [Tooltip("Le menu a afficher lorsque la room a été créée")]
        [SerializeField] private GameObject currentRoom;

        [Tooltip("Le menu a afficher lorsque la room a été créée")]
        [SerializeField] private TMP_Text currentRoomName;

        [Tooltip("Le menu a afficher lorsque la room a été créée")]
        [SerializeField] private TMP_Text currentRoomPassword;
        #endregion

        #region MonoBehaviour CallBacks

        void Start()
        {
            currentRoom.SetActive(false);

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
                roomOptions.IsOpen = false;

                Hashtable tablePassword = new Hashtable
                {
                    { "Password", roomPasswordText.text }
                };

                roomOptions.CustomRoomProperties = tablePassword;

                roomOptions.CustomRoomPropertiesForLobby = new string[] { "Password" };

                PhotonNetwork.LocalPlayer.CustomProperties = tablePassword;

                currentRoomPassword.text = "*****";
                //Debug.Log(tablePassword);
            }
            currentRoomName.text = roomName.text;

            PhotonNetwork.JoinOrCreateRoom(roomName.text, roomOptions, TypedLobby.Default);

            currentRoom.SetActive(true);
            createOrJoinRoom.SetActive(false);
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
            SceneManager.LoadSceneAsync(0);
        }

        public void LeaveLobby()
        {
            PhotonNetwork.LeaveLobby();

            currentRoom.SetActive(false);
            createOrJoinRoom.SetActive(true);

        }

        #endregion
    }
}