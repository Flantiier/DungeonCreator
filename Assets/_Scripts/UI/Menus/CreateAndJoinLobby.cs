using UnityEngine;
using TMPro;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;

namespace _Scripts.UI.Menus
{
	public class CreateAndJoinLobby : MonoBehaviourPunCallbacks
	{
		[SerializeField] private TMP_InputField createField;
		[SerializeField] private TMP_InputField joinField;
		[SerializeField] private GameObject ui;

        private void Awake()
        {
            ui.SetActive(false);
            PhotonNetwork.ConnectUsingSettings();
        }

        public override void OnConnectedToMaster()
        {
			ui.SetActive(true);
			PhotonNetwork.JoinLobby();
        }
        public void CreateRoom()
		{
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
