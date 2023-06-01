using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using Sirenix.Utilities;

namespace _Scripts.UI.Menus
{
    public class RoomList : MonoBehaviourPunCallbacks
    {
        #region Variables
        [SerializeField] private RoomListing roomPrefab;
        [SerializeField] private Transform content;

        private List<RoomListing> _listing = new List<RoomListing>();
        #endregion

        #region Methods
        public override void OnRoomListUpdate(List<RoomInfo> roomList)
        {
            Debug.Log("Joined lobby");

            foreach (RoomInfo info in roomList)
            {
                //Room was removed from list
                if (info.RemovedFromList)
                {
                    int index = _listing.FindIndex(x => x.RoomInfos.Name == info.Name);
                    if (index != 1 && index < _listing.Count)
                    {
                        Destroy(_listing[index].gameObject);
                        _listing.RemoveAt(index);
                    }
                }
                //Add a reference to the room
                else
                {
                    int index = _listing.FindIndex(x => x.RoomInfos.Name == info.Name);
                    if (index == -1)
                    {
                        RoomListing instance = Instantiate(roomPrefab, content);
                        if (instance != null)
                        {
                            instance.SetRoomInfos(info);
                            _listing.Add(instance);
                        }
                    }
                    else
                        _listing[index].UpdateProperties();
                }
            }
        }
        #endregion
    }
}
