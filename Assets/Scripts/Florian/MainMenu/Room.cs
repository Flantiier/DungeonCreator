using UnityEngine;
using TMPro;
using Photon.Realtime;

public class Room : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI roomText;

    public RoomInfo RoomInfo { get; private set; }

    public void SetRoomInfo(RoomInfo roomInfo)
    {
        roomText.text = roomInfo.Name + "    ...   " + roomInfo.PlayerCount + " / " + roomInfo.MaxPlayers;
        RoomInfo = roomInfo;
    }
}
