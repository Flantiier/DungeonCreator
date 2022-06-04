using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class RoomListing : MonoBehaviourPunCallbacks
{
    [Header("Room UI")]
    [Tooltip("Referencing here the prefab to fill the roomList")]
    [SerializeField] private Room roomPrefab;
    [Tooltip("Referencing here the parent for the list")]
    [SerializeField] private Transform content;

    //Current list of open rooms
    private List<Room> _rooms = new List<Room>();


    #region Join Methods 
    private void UpdatingRoomList(List<RoomInfo> roomList)
    {
        foreach (RoomInfo roomInfo in roomList)
        {
            //Removed from the list
            if (roomInfo.RemovedFromList)
            {
                //Find the room index
                int index = FindRoomIndex(roomInfo);
                //Remove it from the list
                if (index != -1)
                {
                    Destroy(_rooms[index].gameObject);
                    _rooms.RemoveAt(index);
                }
            }
            //Added to the list
            else
            {
                //Create a new room GameObject
                Room room = Instantiate(roomPrefab, content);

                //Checking if not null
                if (room != null)
                {
                    //Set his text
                    room.SetRoomInfo(roomInfo);
                    //Add it to the list
                    _rooms.Add(room);
                }
            }
        }
    }
    private int FindRoomIndex(RoomInfo roomInfo)
    {
        int roomIndex = 0;

        for (int i = 0; i < _rooms.Count; i++)
        {
            if (_rooms[i].RoomInfo.Name != roomInfo.Name)
                continue;
            else
            {
                roomIndex = i;
                break;
            }
        }
        return roomIndex;
    }
    #endregion

    #region Callbacks
    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        UpdatingRoomList(roomList);
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("Room successfully joined ! Room Name : " + PhotonNetwork.CurrentRoom.Name);
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        Debug.Log($"Error during room creation, reason : {message}");
    }
    #endregion
}
