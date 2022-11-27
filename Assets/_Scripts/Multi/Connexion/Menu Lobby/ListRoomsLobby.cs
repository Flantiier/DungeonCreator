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
        [Tooltip("Le text ou afficher le nom de la room")]
        [SerializeField] private TMP_Text roomName;

        [Tooltip("Le text ou afficher le nombre de joueurs")]
        [SerializeField] private TMP_Text numberOfPlayers;

        [Tooltip("Le text ou le mdp pourra être rentré")]
        [SerializeField] private GameObject passwordInputField;

        [Tooltip("Le text ou le mdp pourra être rentré")]
        [SerializeField] private GameObject passwordToggle;

        private string _password;
        private bool _isConnecting = false;

        public RoomInfo RoomInfo { get; private set; }

        public void Start()
        {
            passwordToggle.SetActive(false);

            passwordToggle.GetComponent<Toggle>().onValueChanged.AddListener(delegate { ToggleValueChange(passwordToggle.GetComponent<Toggle>()); });
        }

        public void SetRoomInfo(RoomInfo roomInfo)
        {
            RoomInfo = roomInfo;

            roomName.text = roomInfo.Name;
            numberOfPlayers.text = roomInfo.PlayerCount + " / 4";

            if(roomInfo.IsOpen == false)
            {
                passwordToggle.SetActive(true);

                _password = roomInfo.CustomProperties["pwd"].ToString();

                if(_password == passwordInputField.GetComponent<TMP_InputField>().text.ToString())
                {
                    _isConnecting = true;
                }
                else
                {
                    _isConnecting = false;
                }
            }
            else
            {
                passwordToggle.SetActive(false);
            }
        }

        public void JoinRoomToList()
        {
            if (_isConnecting == true)
            {
                PhotonNetwork.JoinRoom(roomName.text);
            }
            else
            {
                Debug.Log("mot de passe incorrect");
            }
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