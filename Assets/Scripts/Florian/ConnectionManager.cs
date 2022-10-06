using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class ConnectionManager : MonoBehaviourPunCallbacks
{
    private void Awake()
    {
        PhotonNetwork.ConnectUsingSettings();
    }

    //Called when connected to the master server
    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected to the server");
        PhotonNetwork.JoinLobby();
    }

    //Called when disconnected from the master server
    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.Log("Disctonned from server to : " + cause.ToString());
    }

    //Called when joined a lobby
    public override void OnJoinedLobby()
    {
        Debug.Log("Lobby successfully joined !");
    }

}
