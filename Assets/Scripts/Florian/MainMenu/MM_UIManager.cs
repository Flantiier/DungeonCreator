using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System;

public class MM_UIManager : MonoBehaviourPunCallbacks
{
    /// <summary>
    /// Differents state of the menu
    /// </summary>
    public enum MenuState
    {
        Loading,
        Connected,
        InRoom,
        Options
    }

    /// <summary>
    /// Current menu state
    /// </summary>
    public MenuState _currentState { get; private set; }
    /// <summary>
    /// Setting a new delegate for the event when the menu change
    /// </summary>
    public static event Action<MenuState> onMenuChanged;

    private void Awake()
    {
        RaiseMenuEvent(MenuState.Loading);
    }

    /// <summary>
    /// Raising the event
    /// </summary>
    /// <param name="newState">The new state of the menu</param>
    private void RaiseMenuEvent(MenuState newState)
    {
        _currentState = newState; 
        onMenuChanged?.Invoke(newState);
    }

    #region Callbacks
    //Called when the lobby is joined
    public override void OnJoinedLobby()
    {
        RaiseMenuEvent(MenuState.Connected);
    }

    //Called when the room is joined
    public override void OnJoinedRoom()
    {
        RaiseMenuEvent(MenuState.InRoom);
    }
    #endregion
}
