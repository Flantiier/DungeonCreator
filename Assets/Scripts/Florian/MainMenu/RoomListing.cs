using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class RoomListing : MonoBehaviourPunCallbacks
{
    [Header("References")]
    [Tooltip("Referencing here the prefab to fill the roomList")]
    [SerializeField] private Room roomPrefab;
    [Tooltip("Referencing here the parent for the list")]
    [SerializeField] private Transform content;

    /// <summary>
    /// Current list of available rooms
    /// </summary>
    private List<Room> _rooms = new List<Room>();

    #region Listing Methods
    /// <summary>
    /// Updating the room list when the UpdateRoomList Callback is called
    /// </summary>
    /// <param name="roomList">Room List from callback</param>
    private void UpdatingRoomList(List<RoomInfo> roomList)
    {
        //For each available room in the callback roomList
        foreach (RoomInfo roomInfo in roomList)
        {
            //If this room has been removed from the list
            if (roomInfo.RemovedFromList)
            {
                //Find removed room index 
                int index = _rooms.FindIndex(x => x.RoomInfo.Name == roomInfo.Name);
                //Remove it from the room list
                if(index != -1)
                {
                    Destroy(_rooms[index].gameObject);
                    _rooms.RemoveAt(index);
                }
            }
            //If this room has been added to the list
            else
            {
                //Checking if the local roomList already contains this one
                //If it contains this room, continue looping
                if (ContainsRoom(roomInfo))
                    continue;
                //If not, add it to the list the local list
                else
                {
                    Room room = Instantiate(roomPrefab, content);
                    room.SetRoomInfo(roomInfo);

                    _rooms.Add(room);
                }
            }
        }
    }

    /// <summary>
    /// Checking by roomName if the local roomList has a certain room
    /// </summary>
    /// <param name="roomInfo">Room to check infos</param>
    private bool ContainsRoom(RoomInfo roomInfo)
    {
        for (int i = 0; i < _rooms.Count; i++)
        {
            if (_rooms[i].RoomInfo.Name != roomInfo.Name)
                continue;
            else
                return true;
        }

        return false;
    }
    #endregion

    #region Callbacks
    //Called chen the roomList server is updated
    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        UpdatingRoomList(roomList);
    }
    #endregion
}
