using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;

namespace _Scripts.Multi.Connexion
{
    public class Launcher : MonoBehaviourPunCallbacks
    {
        #region Variables
        [Tooltip("Le panel contenant les �l�ments UI de gauche")]
        [SerializeField] private GameObject panelGauche;

        [Tooltip("Le panel contenant les �l�ments UI de droite")]
        [SerializeField] private GameObject panelDroit;

        [Tooltip("Le text affichant la connexion")]
        [SerializeField] private GameObject connexionText;

        private string _gameVersion = "1";
        private bool _isConnecting;
        #endregion

        #region MonoBehaviour CallBacks
        public void Awake()
        {
            if (!PhotonNetwork.IsConnected)
            {
                ConnectedUI(true, false, false);

                PhotonNetwork.ConnectUsingSettings();
                PhotonNetwork.GameVersion = _gameVersion;

                _isConnecting = true;
            }
            else
            {
                ConnectedUI(false, true, true);
            }

            PhotonNetwork.AutomaticallySyncScene = true;
        }
        #endregion

        #region Menu Start Callbacks
        public override void OnConnectedToMaster()
        {
            if (_isConnecting)
            {
                ConnectedUI(false, true, true);

                _isConnecting = false;
            }
        }

        public void Connect()
        {
            if (!PhotonNetwork.IsConnected)
            {
                ConnectedUI(true, false, false);

                _isConnecting = PhotonNetwork.ConnectUsingSettings();
                PhotonNetwork.GameVersion = _gameVersion;

                _isConnecting = true;
            }
            else
            {
                PhotonNetwork.JoinLobby();
            }
        }

        public override void OnDisconnected(DisconnectCause cause)
        {
            _isConnecting = false;
        }

        public override void OnJoinedLobby()
        {
            PhotonNetwork.LoadLevel("Menu_Lobby");
        }
        #endregion

        private void ConnectedUI(bool connexionSetActive, bool panelGaucheSetActive, bool panelDroiteSetActive)
        {
            if(connexionText != null && panelGauche != null && panelDroit != null)
            {
                connexionText.SetActive(connexionSetActive);
                panelGauche.SetActive(panelGaucheSetActive);
                panelDroit.SetActive(panelDroiteSetActive);
            }
        }
    }

}