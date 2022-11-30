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
        [SerializeField] private GameObject passwordLock;

        private TMP_Text errorText;

        private string _password;

        public RoomInfo RoomInfo { get; private set; }

        public void Start()
        {
            errorText = GameObject.Find("Erreurs Text").GetComponent<TMP_Text>();
        }

        public void SetRoomInfo(RoomInfo roomInfo)
        {
            RoomInfo = roomInfo;

            roomName.text = roomInfo.Name;

            numberOfPlayers.text = roomInfo.PlayerCount + " / 4";

            if(roomInfo.CustomProperties["pwd"] != null)
            {
                passwordLock.SetActive(true);

                _password = roomInfo.CustomProperties["pwd"].ToString();
            }
            else
            {
                passwordLock.SetActive(false);
            }
        }

        public void JoinRoomToList()
        {
            errorText.text = "";
            string passwordEnter = passwordInputField.GetComponent<TMP_InputField>().text.ToString();

            if (_password != null)
            {
                passwordInputField.SetActive(true);

                if (passwordEnter.Length >= 1)
                {
                    if (_password == passwordEnter)
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
                    errorText.text = "Room privée, entrez le mdp";
                }
            }
            else
            {
                PhotonNetwork.JoinRoom(roomName.text);
            }
        }


        public override void OnJoinedRoom()
        {
            passwordInputField.SetActive(false);
        }

        public override void OnJoinRoomFailed(short returnCode, string message)
        {
            Debug.Log(message + " " + returnCode);
        }
    }
}