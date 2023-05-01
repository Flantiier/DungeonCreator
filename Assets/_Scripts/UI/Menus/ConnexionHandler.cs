using Photon.Pun;
using UnityEngine;
using _Scripts.UI;
using _Scripts.Managers;
using Sirenix.OdinInspector;

namespace _Scripts.Menus
{
    public class ConnexionHandler : MonoBehaviourPunCallbacks
    {
        [TitleGroup("References")]
        [SerializeField] private SceneLoader sceneLoader;

        [TitleGroup("UI")]
        [SerializeField] private string lobbyScene;
        [SerializeField] private AnimatedTextField textField;

        private void Awake()
        {
            PhotonNetwork.ConnectUsingSettings();
        }

        public override void OnConnectedToMaster()
        {
            Debug.Log("Connected to master");
            PhotonNetwork.JoinLobby();
            textField.SetBaseText("Joining lobby");
        }

        public override void OnJoinedLobby()
        {
            Debug.Log("Lobby joined");
            textField.SetBaseText("Chargement du menu");
            sceneLoader.LoadSceneAsync(lobbyScene);
        }
    }
}
