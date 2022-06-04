using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.Events;

public class ConnectionManager : MonoBehaviourPunCallbacks
{
    [Header("Events to raise")]
    [Tooltip("Events to raise before connected to the server")]
    [SerializeField] private UnityEvent onConnecting;
    [Tooltip("Events to raise when the client is connected to master")]
    [SerializeField] private UnityEvent onConnected;


    private void Awake()
    {
        onConnecting?.Invoke();
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        onConnected?.Invoke();
        Debug.Log("Connected to the server");
        PhotonNetwork.JoinLobby();

    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.Log("Disctonned from server to : " + cause.ToString());
    }


    public override void OnJoinedLobby()
    {
        Debug.Log("Lobby successfully joined !");
    }

}
