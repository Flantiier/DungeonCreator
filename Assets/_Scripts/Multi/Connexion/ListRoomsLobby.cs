using System.Collections.Generic;
using UnityEngine;
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

        public RoomInfo RoomInfo { get; private set; }

        public void SetRoomInfo(RoomInfo roomInfo)
        {
            RoomInfo = roomInfo;

            roomName.text = roomInfo.Name;
            numberOfPlayers.text = roomInfo.PlayerCount + " / 4";
        }
    }
}