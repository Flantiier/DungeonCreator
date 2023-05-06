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
        private List<Room> _listing;

        private void Awake()
        {
            _listing = new List<Room>();
        }

        public override void OnRoomListUpdate(List<RoomInfo> roomList)
        {
            foreach (RoomInfo room in roomList)
            {
                if (room.RemovedFromList)
                {
                    int index = _listing.FindIndex(x => x.Infos.Name == room.Name);
                    if (index != 1)
                    {
                        Destroy(_listing[index].gameObject);
                        _listing.RemoveAt(index);
                    }
                }
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
