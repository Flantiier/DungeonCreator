using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

namespace _Scripts.Multi.Connexion
{
    public class ListPlayersMenu : MonoBehaviourPunCallbacks
    {
        [SerializeField] private Transform content;

        [SerializeField] private ListPlayersRoom listPlayersRoomScript;

        private List<ListPlayersRoom> _listPlayersRoom = new List<ListPlayersRoom>();

        private void Awake()
        {
            GetCurrentRoomPlayers();
        }

        private void GetCurrentRoomPlayers()
        {
            foreach(KeyValuePair<int, Player> playerInfo in PhotonNetwork.CurrentRoom.Players)
            {
                AddPlayerList(playerInfo.Value);
            }
        }

        private void AddPlayerList(Player newPlayer)
        {
            ListPlayersRoom listPlayersRoom = Instantiate(listPlayersRoomScript, content);
            if (listPlayersRoom != null)
            {
                listPlayersRoom.SetPlayerInfo(newPlayer);
                _listPlayersRoom.Add(listPlayersRoom);
            }
        }

        public override void OnPlayerEnteredRoom(Player newPlayer)
        {
            AddPlayerList(newPlayer);
        }

        public override void OnPlayerLeftRoom(Player otherPlayer)
        {
            int index = _listPlayersRoom.FindIndex(x => x.Player == otherPlayer);
            if (index != -1)
            {
                Destroy(_listPlayersRoom[index].gameObject);
                _listPlayersRoom.RemoveAt(index);
            }

            if (PhotonNetwork.IsMasterClient)
            {
                otherPlayer.GetNext();
            }
        }
    }
}