using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Realtime;
using Photon.Pun;
using Hashtable = ExitGames.Client.Photon.Hashtable;

namespace _Scripts.Multi.Connexion
{
    public class ListPlayersRoom : MonoBehaviourPunCallbacks
    {
        [SerializeField] private TMP_Text playerNameText;

        [SerializeField] private GameObject leftArrowButton;
        [SerializeField] private GameObject rightArrowButton;

        [SerializeField] private GameObject playerStartButton;

        [SerializeField] private Sprite[] characters;
        [SerializeField] private int dmIndex = 3;

        private Image playerCharacterImage;

        Hashtable playerCharacter = new Hashtable();

        public Player Player { get; private set; }

        public enum Roles { Undefined, Warrior, Archer, Wizard, DM }

        public void Awake()
        {
            playerCharacterImage = GetComponent<Image>();

            playerCharacter["role"] = Roles.Undefined;
            playerCharacter["playerCharacter"] = 0;
            PhotonNetwork.SetPlayerCustomProperties(playerCharacter);
        }

        public void SetPlayerInfo(Player player)
        {
            Player = player;

            UpdatePlayerCharacter(player);

            playerNameText.text = player.NickName;
        }

        public void ApplyLocalChanges()
        {
            leftArrowButton.SetActive(true);
            rightArrowButton.SetActive(true);
            playerStartButton.SetActive(true);
        }

        public void OnClickPlayerStart()
        {
            int playerChoiceCharacter = (int)PhotonNetwork.LocalPlayer.CustomProperties["playerCharacter"];

            /*if(playerChoiceCharacter != dmIndex)
            {
                UpdatePlayerCustomProperties(Roles.Adventurer);
                return;
            }*/
            UpdatePlayerCustomProperties(Roles.DM);
        }

        public void UpdatePlayerCustomProperties(Roles myRole)
        {
            playerCharacter["role"] = myRole;
            PhotonNetwork.SetPlayerCustomProperties(playerCharacter);
        }

        public void OnClickLeftArrow()
        {
            if ((int)playerCharacter["playerCharacter"] == 0)
            {
                playerCharacter["playerCharacter"] = characters.Length - 1;
            }
            else
            {
                playerCharacter["playerCharacter"] = (int)playerCharacter["playerCharacter"] - 1;
            }
            PhotonNetwork.SetPlayerCustomProperties(playerCharacter);
        }

        public void OnClickRightArrow()
        {
            if ((int)playerCharacter["playerCharacter"] == characters.Length - 1)
            {
                playerCharacter["playerCharacter"] = 0;
            }
            else
            {
                playerCharacter["playerCharacter"] = (int)playerCharacter["playerCharacter"] + 1;
            }
            PhotonNetwork.SetPlayerCustomProperties(playerCharacter);
        }

        public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
        {
            if (Player == targetPlayer)
            {
                UpdatePlayerCharacter(targetPlayer);
            }
        }

        public void UpdatePlayerCharacter(Player player)
        {
            if (player.CustomProperties.ContainsKey("playerCharacter"))
            {
                playerCharacterImage.sprite = characters[(int)player.CustomProperties["playerCharacter"]];
                playerCharacter["playerCharacter"] = (int)player.CustomProperties["playerCharacter"];
            }
            else
            {
                playerCharacter["playerCharacter"] = 0;
                playerCharacter["role"] = Roles.Undefined;
            }
        }
    }
}