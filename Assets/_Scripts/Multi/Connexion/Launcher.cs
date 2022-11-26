using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

namespace _Scripts.Multi.Connexion
{
    public class Launcher : MonoBehaviourPunCallbacks
    {
        #region Variables
        [Tooltip("Le panel contenant les éléments UI de gauche")]
        [SerializeField] private GameObject panelGauche;

        [Tooltip("Le panel contenant les éléments UI de droite")]
        [SerializeField] private GameObject panelDroit;

        [Tooltip("Le text affichant la connexion")]
        [SerializeField] private GameObject connexionText;

        private string _gameVersion = "1";
        #endregion

        #region MonoBehaviour CallBacks
        public void Awake()
        {
            if (!PhotonNetwork.IsConnected)
            {
                PhotonNetwork.ConnectUsingSettings();
                PhotonNetwork.GameVersion = _gameVersion;
            }

            PhotonNetwork.AutomaticallySyncScene = true;

            ConnectedUI(false, true, true);
        }
        #endregion

        #region Menu Start Callbacks
        public override void OnConnectedToMaster()
        {
            base.OnConnectedToMaster();
        }

        public void Connect()
        {
            ConnectedUI(true, false, false);
            if (!PhotonNetwork.IsConnected)
            {
                PhotonNetwork.ConnectUsingSettings();
                PhotonNetwork.GameVersion = _gameVersion;
            }
            else
            {
                PhotonNetwork.JoinLobby();
            }
        }

        public override void OnDisconnected(DisconnectCause cause)
        {
            ConnectedUI(false, true, true);
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