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
            textField.SetBaseText("connection aux serveurs");
        }
        #endregion

        #region Methods
        public IEnumerator LoadMenu()
        {
            textField.SetBaseText("chargement du menu");
            AsyncOperation operation = SceneManager.LoadSceneAsync(mainScreenScene);

            while (!operation.isDone) { yield return null; }
        }
        #endregion

        #region Callbacks
        public override void OnConnectedToMaster()
        {
            StartCoroutine(LoadMenu());
        }
        #endregion
    }
}
