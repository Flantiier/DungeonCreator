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

        [SerializeField] private Sprite[] characters;

        private Image playerCharacter;

        Hashtable playerProperties = new Hashtable();

        public Player Player { get; private set; }

        public void Awake()
        {
            playerCharacter = GetComponent<Image>();
        }

        public void SetPlayerInfo(Player player)
        {
            Player = player;

            UpdatePlayerCharacter(player);

            playerNameText.text = player.NickName;

/*            if(player.NickName == "Entrer votre nom..." || player.NickName == "")
            {
                playerNameText.text = "Player " + PhotonNetwork.CurrentRoom.PlayerCount;
                player.NickName = playerNameText.text;
            }
            else if(player.NickName.Length != 0)
            {
                playerNameText.text = player.NickName;
            }*/
        }

        public void ApplyLocalChanges()
        {
            leftArrowButton.SetActive(true);
            rightArrowButton.SetActive(true);
        }

        public void OnClickLeftArrow()
        {
            if((int)playerProperties["playerCharacter"] == 0)
            {
                playerProperties["playerCharacter"] = characters.Length - 1;
            } 
            else
            {
                playerProperties["playerCharacter"] = (int)playerProperties["playerCharacter"] - 1;
            }
            PhotonNetwork.SetPlayerCustomProperties(playerProperties);
        }

        public void OnClickRightArrow()
        {
            if ((int)playerProperties["playerCharacter"] == characters.Length - 1)
            {
                playerProperties["playerCharacter"] = 0;
            }
            else
            {
                playerProperties["playerCharacter"] = (int)playerProperties["playerCharacter"] + 1;
            }
            PhotonNetwork.SetPlayerCustomProperties(playerProperties);
        }

        public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
        {
            if(Player == targetPlayer)
            {
                UpdatePlayerCharacter(targetPlayer);
            }
        }

        public void UpdatePlayerCharacter(Player player)
        {
            if (player.CustomProperties.ContainsKey("playerCharacter"))
            {
                playerCharacter.sprite = characters[(int)player.CustomProperties["playerCharacter"]];
                playerProperties["playerCharacter"] = (int)player.CustomProperties["playerCharacter"];
            }
            else
            {
                playerProperties["playerCharacter"] = 0;
            }
        }
    }
}