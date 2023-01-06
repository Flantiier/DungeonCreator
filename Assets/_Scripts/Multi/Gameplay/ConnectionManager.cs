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
        PhotonNetwork.AutomaticallySyncScene = true;
        PhotonNetwork.JoinRandomOrCreateRoom();
    }
    #endregion

    #region Leaving Callbacks
    //Called when disconnected from the master server
    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.Log($"Disctonned from server to : {cause} at {Time.time}");
    }

    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        Debug.Log($"MasterClient set to player {PhotonNetwork.MasterClient.ActorNumber}");
    }
    #endregion
}
