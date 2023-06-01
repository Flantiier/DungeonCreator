using TMPro;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;

namespace _Scripts.UI.Menus
{
    public class RoomListing : MonoBehaviourPunCallbacks
    {
        #region Variables
        [SerializeField] private TextMeshProUGUI nameField;
        [SerializeField] private TextMeshProUGUI playersField;

        public RoomInfo RoomInfos { get; private set; }
        #endregion

        #region Methods
        /// <summary>
        /// Set Room properties
        /// </summary>
        public void SetRoomInfos(RoomInfo infos)
        {
            RoomInfos = infos;
            nameField.text = RoomInfos.Name;
            playersField.text = $"{RoomInfos.PlayerCount}/{RoomInfos.MaxPlayers}";
        }

        public void UpdateProperties()
        {
            playersField.text = $"{RoomInfos.PlayerCount}/{RoomInfos.MaxPlayers}";
        }

        /// <summary>
        /// Join stored room
        /// </summary>
        public void JoinRoom()
        {
            CreateAndJoinRoom.JoinInList(RoomInfos.Name);
        }
        #endregion
    }
}
