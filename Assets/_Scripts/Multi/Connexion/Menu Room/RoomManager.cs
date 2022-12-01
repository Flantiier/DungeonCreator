using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using static _Scripts.Multi.Connexion.ListPlayersRoom;

namespace _Scripts.Multi.Connexion
{

    public class RoomManager : MonoBehaviourPunCallbacks
    {
        #region Variables
        [Header("Chargement des scenes")]
        [SerializeField] private string sceneToLoad = "GameScene";

        [Header("UI references")]
        [Tooltip("Le text permettant d'afficher le nom de la room actuelle")]
        [SerializeField] private TMP_Text currentRoomName;

        [Tooltip("Le bouton permettant au master client de lancer la game")]
        [SerializeField] private GameObject playButton;

        [Tooltip("Le text ou sera affiché les éventuelles erreurs")]
        [SerializeField] private TMP_Text errorText;

        public Roles selectedRole = Roles.Undefined;
        #endregion

        #region MonoBehaviour CallBacks
        private void Start()
        {
            PhotonNetwork.AutomaticallySyncScene = true;

            playButton.SetActive(false);

            errorText.text = "";
            currentRoomName.text = "Nom de la Room : " + PhotonNetwork.CurrentRoom.Name.ToString();
        }

        private void Update()
        {
            UpdateSelectedRole();
            EnableStartGame();
        }

        /// <summary>
        /// Enable od disable start game button
        /// </summary>
        public void EnableStartGame()
        {
            bool condition = PhotonNetwork.IsMasterClient && PhotonNetwork.PlayerList.Length >= 2;
            playButton.SetActive(condition);

            if (!condition)
                errorText.text = "Il faut un Dm et minimum un Aventurier pour commencer.";
        }

        #endregion

        #region Menu Room

        public override void OnConnectedToMaster()
        {
            PhotonNetwork.JoinLobby();
        }

        /// <summary>
        /// Update the selected role
        /// </summary>
        public void UpdateSelectedRole()
        {
            if (PhotonNetwork.LocalPlayer.CustomProperties["role"] == null)
            {
                Debug.Log("Custom property not existing");
                return;
            }

            Debug.Log("My role is : " + (Roles)PhotonNetwork.LocalPlayer.CustomProperties["role"]);
            selectedRole = (Roles)PhotonNetwork.LocalPlayer.CustomProperties["role"];

            switch (selectedRole)
            {
                case Roles.Undefined:
                    SetSelectedRole(Roles.Undefined);
                    break;

                case Roles.Adventurer:
                    SetSelectedRole(Roles.Adventurer);
                    break;

                case Roles.DM:
                    SetSelectedRole(Roles.DM);
                    break;
            }
        }

        /// <summary>
        /// Set the selected role to a new role
        /// </summary>
        /// <param name="newRole"> New role </param>
        private void SetSelectedRole(Roles newRole)
        {
            selectedRole = newRole;
        }


        public void OnClickPlayButton()
        {
            PhotonNetwork.LoadLevel(sceneToLoad);
        }

        public override void OnLeftRoom()
        {
            if (playButton != null)
            {
                playButton.SetActive(false);
            }

            PhotonNetwork.LoadLevel(0);
        }

        public void LeaveRoom()
        {
            PhotonNetwork.LeaveRoom();
        }

        #endregion
    }
}