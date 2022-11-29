using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

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
        private bool _isConnecting = false;

        public RoomInfo RoomInfo { get; private set; }

        public void Start()
        {
            errorText = GameObject.Find("Erreurs Text").GetComponent<TMP_Text>();

            passwordToggle.GetComponent<Toggle>().onValueChanged.AddListener(delegate { ToggleValueChange(passwordToggle.GetComponent<Toggle>()); });
        }

        public void SetRoomInfo(RoomInfo roomInfo)
        {
            RoomInfo = roomInfo;

            roomName.text = roomInfo.Name;
            PhotonNetwork.LocalPlayer.CustomProperties.TryAdd("roomname", roomName.text);

            numberOfPlayers.text = roomInfo.PlayerCount + " / 4";

            if(roomInfo.IsOpen == false)
            {
                passwordToggle.SetActive(true);

                _password = roomInfo.CustomProperties["pwd"].ToString();

                PhotonNetwork.LocalPlayer.CustomProperties.TryAdd("pwd", _password);
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
                if (PhotonNetwork.LocalPlayer.CustomProperties["pwd"].ToString() == passwordInputField.GetComponent<TMP_InputField>().text.ToString())
                {
                    PhotonNetwork.JoinRoom(roomName.text);
                    Debug.Log(roomName.text);
                }
                else
                {
                    errorText.text = "Mot de Passe Incorrect";
                }
            }
            else
            {
                    errorText.text = "Vous n'avez pas entré le mdp";
            }
        }

        public override void OnJoinedRoom()
        {
            PhotonNetwork.LoadLevel("Menu_Room");
        }

        public void ToggleValueChange(Toggle privateToggle)
        {
            if (privateToggle.isOn)
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