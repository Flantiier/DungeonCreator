using TMPro;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;

namespace _Scripts.UI.Menus
{
    public class Room : MonoBehaviourPunCallbacks
    {
        [SerializeField] private TextMeshProUGUI nameField;
        [SerializeField] private TextMeshProUGUI playersField;
        public RoomInfo Infos { get; set; }

        public void JoinRoom()
        {
            CreateAndJoinRoom.JoinInList(Infos.Name);
        }

        public void SetRoomInfos()
        {
            nameField.text = Infos.Name;
            playersField.text = $"{Infos.PlayerCount}/{Infos.MaxPlayers}";
        }

        public override void OnRoomPropertiesUpdate(Hashtable propertiesThatChanged)
        {
            SetRoomInfos();
        }

    }
}
