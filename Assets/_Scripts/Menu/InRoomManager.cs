using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;
using Photon.Realtime;

public class InRoomManager : MonoBehaviourPunCallbacks
{/*
    [Header("References")]
    [Tooltip("Referencing the text to display the roomName")]
    [SerializeField] private TextMeshProUGUI roomName;
    [Tooltip("Referencing the tag prefab for the DungeonMaster")]
    [SerializeField] private PlayerTagEditor dungeonMasterTag;
    [Tooltip("Referencing the tag prefab for the Adventurer")]
    [SerializeField] private PlayerTagEditor[] adventurerTags;
    [Tooltip("Referencing the transform of the playerList")]
    [SerializeField] private Transform playerList;

    /// <summary>
    /// Current player list in the room
    /// </summary>
    private List<PlayerInstance> _players = new List<PlayerInstance>();

    #region PlayerList Methods
    /// <summary>
    /// Sorting the player list thanks to the current players actorNumbers in the room
    /// </summary>
    private void GetSortedPlayerList()
    {
        //For each player in the room list 
        foreach (KeyValuePair<int, Player> player in PhotonNetwork.CurrentRoom.Players)
        {
            //Creating a list of Players with a PlayerInstance class
            PlayerInstance instance = new PlayerInstance(player.Value, player.Key);
            _players.Add(instance);

            //Sorting these list of players
            _players.Sort((x, y) => x.ActorNumber.CompareTo(y.ActorNumber));
        }

        //Setting player Id in the room
        SettingPlayerIDs();

        //For each player in the room
        foreach (PlayerInstance player in _players)
            //Instantiate a new tag according to his role
            NewPlayer(player.player);
    }

    /// <summary>
    /// Setting the index of all players
    /// </summary>
    private void SettingPlayerIDs()
    {
        for (int i = 0; i < _players.Count; i++)
            _players[i].SetIndex(i);
    }

    /// <summary>
    /// Checking if the player is already in the local playerList
    /// </summary>
    /// <param name="_player">Player to check</param>
    private bool PlayerAlreadyInRoom(Player _player)
    {
        foreach (PlayerInstance player in _players)
        {
            if (_player.ActorNumber != player.ActorNumber)
                continue;
            else
                return true;
        }

        return false;
    }

    /// <summary>
    /// Getting player id in the playerList
    /// </summary>
    /// <param name="Player">Player to get his index</param>
    private int GetPlayerIndex(Player Player)
    {
        int index = 0;

        foreach (PlayerInstance player in _players)
        {
            if (player.ActorNumber != Player.ActorNumber)
                continue;
            else
                index = player.PlayerIndex;
        }

        return index;
    }

    /// <summary>
    /// Instantiate a new tag for certain player
    /// /// </summary>
    /// <param name="_player">Instantiate this player tag</param>
    private void NewPlayer(Player _player)
    {
        //Getting player index previously
        int index = GetPlayerIndex(_player);

        //If the player is the masterClient, he is the dungeonMaster
        if (_player.IsMasterClient)
            dungeonMasterTag.SetPlayerInfo(_players[index], $"Joueur : {index + 1} (Dungeon Master)");
        //Else he is an adventurer
        else
        {
            //For each adventurer ag
            foreach (PlayerTagEditor tag in adventurerTags)
            {
                if (tag.playerInstance == null)
                {
                    tag.SetPlayerInfo(_players[index], $"Joueur : {index + 1} (Adventurer)");
                    break;
                }
                else
                    continue;
            }
        }
    }

    /// <summary>
    /// Getting an adventurer index in the adventurer arry
    /// </summary>
    /// <param name="player">Player to gets index of</param>
    private int GetAdventurerIndex(Player player)
    {
        int index = 0;

        for (int i = 0; i < adventurerTags.Length; i++)
        {
            if (adventurerTags[i].playerInstance != null)
            {
                if (adventurerTags[i].playerInstance.player != player)
                    continue;
                else
                    index = i;
            }
        }

        return index;
    }

    /// <summary>
    /// Resetting the playerList and other roomInfo 
    /// </summary>
    private void ResetRoomInfo()
    {
        dungeonMasterTag.ResetTag();

        foreach (PlayerTagEditor tag in adventurerTags)
            tag.ResetTag();

        _players = new List<PlayerInstance>();
    }
    #endregion

    #region Callbacks
    //Called when joined th room
    public override void OnJoinedRoom()
    {
        //Look the playerList
        GetSortedPlayerList();
    }

    /// <summary>
    /// Method to leave the room
    /// </summary>
    public void LeaveRoom()
    {
        if (!PhotonNetwork.IsConnected)
            return;

        PhotonNetwork.LeaveRoom();
    }

    //Called when a player enter in the room
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        if (!PlayerAlreadyInRoom(newPlayer))
        {
            _players.Add(new PlayerInstance(newPlayer, newPlayer.ActorNumber));
            _players.Sort((x, y) => x.ActorNumber.CompareTo(y.ActorNumber));
        }

        SettingPlayerIDs();

        NewPlayer(newPlayer);
    }

    //Called when a player left the room
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        int index = GetPlayerIndex(otherPlayer);

        if (PlayerAlreadyInRoom(otherPlayer))
        {
            adventurerTags[GetAdventurerIndex(otherPlayer)].ResetTag();
            _players.RemoveAt(index);
        }
    }

    //Called when the local player left the room
    public override void OnLeftRoom()
    {
        ResetRoomInfo();
    }
    #endregion*/
}
