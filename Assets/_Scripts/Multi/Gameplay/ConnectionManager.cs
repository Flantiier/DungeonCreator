using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class ConnectionManager : MonoBehaviourPunCallbacks
{
    #region Variables
    #endregion

    #region Builts_In
    private void Awake()
    {
        PhotonNetwork.ConnectUsingSettings();
    }
    #endregion

    #region Joining Callbacks
    //Called when connected to the master server
    public override void OnConnectedToMaster()
    {
        Debug.Log($"Connected to server at {Time.time}");
        PhotonNetwork.AutomaticallySyncScene = true;
        PhotonNetwork.JoinRandomOrCreateRoom();
    }

    public override void OnJoinedRoom()
    {
        Debug.Log($"Lobby successfully joined !at {Time.time}");
        Debug.Log($"You are player {PhotonNetwork.LocalPlayer.ActorNumber}");
    }

    //Called when joined a lobby
    public override void OnJoinedLobby()
    {
        Debug.Log($"Lobby successfully joined !at {Time.time}");
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.Log($"Connecting player : player {newPlayer.ActorNumber}");
    }
    #endregion

    #region Leaving Callbacks
    //Called when disconnected from the master server
    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.Log($"Disctonned from server to : {cause} at {Time.time}");
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        Debug.Log($"Player left : player {otherPlayer.ActorNumber}");
        Debug.Log($"MasterClient player : player {PhotonNetwork.MasterClient.ActorNumber}");
    }

    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        Debug.Log($"MasterClient set to player {PhotonNetwork.MasterClient.ActorNumber}");
    }
    #endregion
}
