using System.Collections;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;

namespace _Scripts.Menus
{

    public class ConnexionHandler : MonoBehaviourPunCallbacks
    {
        [SerializeField] private string lobbyScene;
        [SerializeField] private GameEvent onConnectedEvent;

        private AsyncOperation _asyncOperation;

        private void Awake()
        {
            PhotonNetwork.ConnectUsingSettings();
        }

        public override void OnConnectedToMaster()
        {
            Debug.Log("Connected to master");
            PhotonNetwork.JoinLobby();
        }

        public override void OnJoinedLobby()
        {
            Debug.Log("Lobby joined");
            StartCoroutine(LoadSceneAsync());
        }

        private IEnumerator LoadSceneAsync()
        {
            _asyncOperation = SceneManager.LoadSceneAsync(lobbyScene);

            while (!_asyncOperation.isDone)
            {
                yield return new WaitForSeconds(0.1f);
            }
        }
    }
}
