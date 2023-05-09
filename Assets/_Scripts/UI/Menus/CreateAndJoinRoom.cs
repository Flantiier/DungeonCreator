using UnityEngine;
using TMPro;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;

namespace _Scripts.UI.Menus
{
    public class CreateAndJoinRoom : MonoBehaviourPunCallbacks
    {
        [SerializeField] private TMP_InputField createField;
        [SerializeField] private TMP_InputField joinField;

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
            Debug.Log("Joined room");
            SceneManager.LoadScene("RoomMenu");
        }
    }
}
