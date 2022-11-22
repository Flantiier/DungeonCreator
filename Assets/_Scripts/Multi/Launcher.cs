using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

namespace _Scripts.Multi
{
    public class Launcher : MonoBehaviourPunCallbacks
    {
        [Tooltip("Le nombre max de joueurs par Room")]
        [SerializeField] private int maxPlayersPerRoom = 4;

        private string _gameVersion = "1";
        private string _playerName;

        public void Awake()
        {
            PhotonNetwork.AutomaticallySyncScene = true;

            PhotonNetwork.ConnectUsingSettings();
            PhotonNetwork.GameVersion = _gameVersion;
            _playerName = PlayerPrefs.GetString("name");
        }

        public void Connect()
        {
            if(PhotonNetwork.IsConnected)
            {
                PhotonNetwork.LoadLevel("Menu_Loading");
            }
        }
    }

}