using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

namespace _Scripts.UI.Menus
{
    public class RoomList : MonoBehaviourPunCallbacks
    {
        [SerializeField] private Room roomPrefab;
        [SerializeField] private Transform content;
        [SerializeField] private GameObject noRoomText;
        private readonly List<Room> _listing = new List<Room>();

        public void Awake()
        {
            if (!PhotonNetwork.IsConnected)
                return;

            PhotonNetwork.JoinLobby();
        }

        private void Update()
        {
            if (PhotonNetwork.IsConnected || !content.gameObject.activeInHierarchy)
                return;

            bool enabled = PhotonNetwork.CountOfRooms <= 0;
            if(noRoomText.activeSelf != enabled)
                noRoomText.SetActive(enabled);
        }

        public override void OnRoomListUpdate(List<RoomInfo> roomList)
        {
            foreach (RoomInfo room in roomList)
            {
                //Room was removed from list
                if (room.RemovedFromList)
                {
                    //If it doesn't contain the room then pass
                    if (!_listing.Contains(roomPrefab))
                        continue;

                    int index = _listing.FindIndex(x => x.Infos.Name == room.Name);
                    if (index != 1)
                    {
                        Destroy(_listing[index].gameObject);
                        _listing.RemoveAt(index);
                    }
                }
                //Add a reference to the room
                else
                {
                    Room instance = Instantiate(roomPrefab, content);
                    if (instance != null)
                    {
                        instance.Infos = room;
                        instance.SetRoomInfos();
                        _listing.Add(instance);
                    }
                }
            }
        }
    }
}
