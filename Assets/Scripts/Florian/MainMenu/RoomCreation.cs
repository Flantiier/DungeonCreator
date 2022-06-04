using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine.UI;

public class RoomCreation : MonoBehaviourPunCallbacks
{
    [Header("References")]
    [Tooltip("Input field to set the room name")]
    [SerializeField] private TMP_InputField roomNameField;
    [Tooltip("Referencing the create room button")]
    [SerializeField] private Button createRoomButton;

    [Header("InputField Bounds")]
    [Tooltip("Minimum size of the room name")]
    [SerializeField] private int minLength = 3;
    [Tooltip("Maximum size of the room name")]
    [SerializeField] private int maxLength = 15;

    [Header("RoomOptions Profil")]
    [Tooltip("Maximum number of players in one room")]
    [SerializeField] private int maxPlayers = 4;
    [Tooltip("Time before a client gets diconnected (milliseconds)")]
    [SerializeField] private int maxTtlTime = 1;
    [Tooltip("Time before the room gets destroy when the last client left (milliseconds)")]
    [SerializeField] private int maxTimeBeforeDestroy = 1;

    #region Builts-In
    private void Awake()
    {
        //Limiting the length of the inputField
        roomNameField.characterLimit = maxLength;

        //Disable createRoom button
        createRoomButton.interactable = false;

        //Subscribe a method to verify the roomName length before its submit
        roomNameField.onValueChanged.AddListener(delegate { VerifyingRoomName(); });
    }
    #endregion

    #region Room Creation Methods
    public void CreateRoom()
    {
        //Checking connection before creating a room
        if (!PhotonNetwork.IsConnected)
            return;

        //Checking the inputField text
        string roomName = roomNameField.text;

        //Creating a new roomOption profil
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = (byte)maxPlayers;
        roomOptions.PlayerTtl = maxTtlTime * 10;
        roomOptions.EmptyRoomTtl = maxTimeBeforeDestroy * 10;

        //Creating a new room with the roomOptions created just before
        PhotonNetwork.CreateRoom(roomName, roomOptions, TypedLobby.Default);
    }

    private void VerifyingRoomName()
    {
        //Get inputField text
        string roomName = roomNameField.text;

        //Sise too short => Can't create a room
        if (roomName.Length < minLength)
            createRoomButton.interactable = false;
        //Good size => Can create a room
        else
            createRoomButton.interactable = true;
    }
    #endregion

    #region Callbacks
    public override void OnCreatedRoom()
    {
        Debug.Log("Room succesfully created !");
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.Log($"Error during room creation, reason : {message}");
    }
    #endregion
}
