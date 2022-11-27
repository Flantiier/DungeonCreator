using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Realtime;
using Photon.Pun;

namespace _Scripts.Multi.Connexion
{
    public class ListPlayersRoom : MonoBehaviour
    {
        [SerializeField] private TMP_Text playerNameText;
        
        public Player Player { get; private set; }

        public void SetPlayerInfo(Player player)
        {
            Player = player;

            if(player.NickName.Length != 0)
            {
                playerNameText.text = player.NickName;
            }
            else
            {
                playerNameText.text = "player " + Random.Range(3, 100);
            }
        }
    }
}