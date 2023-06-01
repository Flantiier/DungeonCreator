using UnityEngine;
using TMPro;
using Photon.Pun;
using Photon.Realtime;

namespace _Scripts.UI.Menus
{
    public class CreateAndJoinRoom : MonoBehaviourPunCallbacks
    {
        #region Variables
        [SerializeField] private GameObject panel;
        [SerializeField] private TMP_InputField createField;
        [SerializeField] private TMP_InputField joinField;
        [SerializeField] private string roomMenuScene = "RoomMenu";
        [SerializeField] private GameObject textLoad;
        #endregion

        #region Methods
        /// <summary>
        /// Create a room with the given name
        /// </summary>
        public void CreateRoom()
        {
            if (createField.text.Length <= 2)
                return;

            RoomOptions roomOptions = new RoomOptions();
            roomOptions.MaxPlayers = 4;
            roomOptions.IsOpen = true;
            roomOptions.EmptyRoomTtl = 0;

            PhotonNetwork.CreateRoom(createField.text, roomOptions);
        }

        /// <summary>
        /// Join given room
        /// </summary>
        public void JoinRoom()
        {
            PhotonNetwork.JoinRoom(joinField.text);
        }

        public void JoinLobby()
        {
            PhotonNetwork.JoinLobby();
        }

        public void LeaveLobby()
        {
            PhotonNetwork.LeaveLobby();
        }

        /// <summary>
        /// Join room in list
        /// </summary>
        public static void JoinInList(string name)
        {
            PhotonNetwork.JoinRoom(name);
        }

        /// <summary>
        /// On joined room callback
        /// </summary>
        public override void OnJoinedRoom()
        {
            textLoad.SetActive(true);
            PhotonNetwork.LoadLevel(roomMenuScene);
        }
        #endregion
    }
}
