using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class ConnectionManager : MonoBehaviourPunCallbacks
{
    private void Awake()
    {
        if (PhotonNetwork.IsConnected)
            return;

        PhotonNetwork.ConnectUsingSettings();
    }

    //Called when connected to the master server
    public override void OnConnectedToMaster()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
        PhotonNetwork.JoinRandomOrCreateRoom();
    }

    //Called when disconnected from the master server
    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.Log($"Disctonned from server to : {cause} at {Time.time}");
    }

    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        Debug.Log($"MasterClient set to player {PhotonNetwork.MasterClient.ActorNumber}");
    }
}
