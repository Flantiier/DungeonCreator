using UnityEngine;
using TMPro;
using Photon.Pun;
using Photon.Realtime;

public class Room : MonoBehaviour
{
    [Header("References")]
    [Tooltip("Referecing the text which will display the room name and the number of players in this room")]
    [SerializeField] private TextMeshProUGUI roomText;

    /// <summary>
    /// Get an instance of the roomInfo class for this room
    /// </summary>
    public RoomInfo RoomInfo { get; private set; }

    /// <summary>
    /// Setting the text of this room (displaying roomName and number of players)
    /// </summary>
    /// <param name="roomInfo">Instance of the roomInfo class for this room</param>
    public void SetRoomInfo(RoomInfo roomInfo)
    {
        roomText.SetText(roomInfo.Name + "    ...   " + roomInfo.PlayerCount + " / " + roomInfo.MaxPlayers);
        RoomInfo = roomInfo;
    }

    /// <summary>
    /// Method to join the room
    /// </summary>
    public void JoinRoom()
    {
        if (RoomInfo == null)
            return;

        PhotonNetwork.JoinRoom(RoomInfo.Name);
    }
}
