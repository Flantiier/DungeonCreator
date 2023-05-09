using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;

namespace _Scripts.UI.Menus
{
    public class ConnexionHandler : MonoBehaviourPunCallbacks
    {
        #region Variables
        [SerializeField] private string mainScreenScene = "MainScreen";
        [SerializeField] private float waitTime = 2f;
        [SerializeField] private AnimatedTextField textField;
        #endregion

        #region Builts_In
        private void Awake()
        {
            PhotonNetwork.ConnectUsingSettings();
            textField.SetBaseText("Connection to Photon Servers");
        }
        #endregion

        #region Methods
        public IEnumerator LoadMenu()
        {
            textField.SetBaseText("Loading main screen");
            AsyncOperation operation = SceneManager.LoadSceneAsync(mainScreenScene);

            while (!operation.isDone) { yield return null; }
        }
        #endregion

        #region Callbacks
        public override void OnConnectedToMaster()
        {
            Debug.Log("Connected to master");
            PhotonNetwork.JoinLobby();
            textField.SetBaseText("Joining a lobby");
        }

        public override void OnJoinedLobby()
        {
            Debug.Log("Lobby joined");
            StartCoroutine(LoadMenu());
        }
        #endregion
    }
}
