using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using Photon.Pun;

namespace _Scripts.Multi.Connexion
{
    public class ListRoomsMenu : MonoBehaviourPunCallbacks
    {
        [SerializeField] private Transform content;

        [SerializeField] private ListRoomsLobby listRoomsLobbyScript;

        private List<ListRoomsLobby> _listRoomsLobby = new List<ListRoomsLobby>();

        private TypedLobby customLobby = new TypedLobby("customLobby", LobbyType.Default);

        public void JoinLobby()
        {
            PhotonNetwork.JoinLobby(customLobby);
        }

        public override void OnRoomListUpdate(List<RoomInfo> roomList)
        {
            foreach (RoomInfo roomInfo in roomList)
            {
                if (roomInfo.RemovedFromList)
                {
                    int index = _listRoomsLobby.FindIndex(x => x.RoomInfo.Name == roomInfo.Name);
                    if (index != -1)
                    {
                        Destroy(_listRoomsLobby[index].gameObject);
                        _listRoomsLobby.RemoveAt(index);
                    }
                }
                else
                {
                    ListRoomsLobby listRoomsLobby = Instantiate(listRoomsLobbyScript, content);
                    if (listRoomsLobby != null)
                    {
                        listRoomsLobby.SetRoomInfo(roomInfo);
                        _listRoomsLobby.Add(listRoomsLobby);
                    }
                }
            }
        }

        public override void OnJoinedLobby()
        {
            _listRoomsLobby.Clear();
        }

        public override void OnLeftLobby()
        {
            _listRoomsLobby.Clear();
        }

        public override void OnDisconnected(DisconnectCause cause)
        {
            _listRoomsLobby.Clear();
        }
    }
}