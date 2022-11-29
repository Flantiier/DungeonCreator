using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using Hashtable = ExitGames.Client.Photon.Hashtable;

namespace _Scripts.Multi.Connexion
{
    public class ListRoomsLobby : MonoBehaviourPunCallbacks
    {
        const string roomPasswordPrefKey = "";

        [Tooltip("Le text ou afficher le nom de la room")]
        [SerializeField] private TMP_Text roomName;

        [Tooltip("Le text ou afficher le nombre de joueurs")]
        [SerializeField] private TMP_Text numberOfPlayers;

        [Tooltip("Le text ou le mdp pourra être rentré")]
        [SerializeField] private GameObject passwordInputField;

        [Tooltip("Le text ou le mdp pourra être rentré")]
        [SerializeField] private GameObject passwordToggle;

        private TMP_Text errorText;

        private string _password;

        public RoomInfo RoomInfo { get; private set; }

        public void Start()
        {
            errorText = GameObject.Find("Erreurs Text").GetComponent<TMP_Text>();

            passwordToggle.GetComponent<Toggle>().isOn = true;

            passwordToggle.GetComponent<Toggle>().onValueChanged.AddListener(delegate { ToggleValueChange(passwordToggle.GetComponent<Toggle>()); });
        }

        public void SetRoomInfo(RoomInfo roomInfo)
        {
            RoomInfo = roomInfo;

            roomName.text = roomInfo.Name;

            numberOfPlayers.text = roomInfo.PlayerCount + " / 4";

            if(roomInfo.CustomProperties["pwd"] != null)
            {
                passwordToggle.SetActive(true);

                _password = roomInfo.CustomProperties["pwd"].ToString();
            }
            else
            {
                passwordToggle.SetActive(false);
            }
        }

        public void JoinRoomToList()
        {
            errorText.text = "";

            if (!passwordToggle.GetComponent<Toggle>().isOn)
            {
                if (_password == passwordInputField.GetComponent<TMP_InputField>().text.ToString())
                {
                    PhotonNetwork.JoinRoom(roomName.text);
                }
                else
                {
                    errorText.text = "Mot de Passe Incorrect";
                }
            }
            else
            {
                PhotonNetwork.JoinRoom(roomName.text);
            }
        }

        public override void OnJoinedRoom()
        {
            PhotonNetwork.LoadLevel("Menu_Room");
        }

        public override void OnJoinRoomFailed(short returnCode, string message)
        {
            Debug.Log(message + " " + returnCode);
        }

        public void ToggleValueChange(Toggle privateToggle)
        {
            if (!privateToggle.isOn)
            {
                passwordInputField.SetActive(false);
            }
            else
            {
                passwordInputField.SetActive(true);
            }
        }
    }
}