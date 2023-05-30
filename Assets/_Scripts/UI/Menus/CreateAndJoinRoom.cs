using UnityEngine;
using TMPro;
using Photon.Pun;
using Photon.Realtime;
using _Scripts.Managers;

namespace _Scripts.UI.Menus
{
    public class CreateAndJoinRoom : MonoBehaviourPunCallbacks
    {
        [SerializeField] private TMP_InputField createField;
        [SerializeField] private TMP_InputField joinField;
        [SerializeField] private string roomMenuScene = "RoomMenu";
        [SerializeField] private SceneLoader loader;

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

        public void JoinRoom()
        {
            PhotonNetwork.JoinRoom(joinField.text);
        }

        public static void JoinInList(string name)
        {
            PhotonNetwork.JoinRoom(name);
        }

        public override void OnJoinedRoom()
        {
            loader.LoadSceneAsync(roomMenuScene);
        }
}
}
