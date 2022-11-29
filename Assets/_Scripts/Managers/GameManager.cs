using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using _ScriptablesObjects.GameManagement;
using Assets._Scripts.Multi.Gameplay;
using _Scripts.Utilities.Florian;

namespace _Scripts.Managers
{
    public class GameManager : MonoBehaviourSingleton<GameManager>
    {
        #region Variables
        public bool canPlay;

        [Header("Game properties")]
        [SerializeField] private GameSettings gameSettings;
        #endregion

        #region Properties
        public GameSettings GameSettings => gameSettings;
        public Timer Timer { get; private set; }
        #endregion

        #region Builts_In
        public override void Awake()
        {
            base.Awake();

            if (!PhotonNetwork.IsMasterClient)
                return;

            Timer = new Timer(TimeFunctions.GetTimeInSeconds(gameSettings.duration, TimeFunctions.TimeUnit.Minuts));
        }
        #endregion

        #region Methods
        public void HelloWorld()
        {
            Debug.Log("HelloWorld");
        }

        public void HandleGameTimer()
        {
            if (!PhotonNetwork.IsMasterClient)
                return;

            Timer.SetTimer(false);
        }

        #endregion

        #region Callbacks
        public override void OnJoinedRoom()
        {
            canPlay = PhotonNetwork.PlayerList.Length > 1;
        }

        public override void OnPlayerEnteredRoom(Player newPlayer)
        {
            canPlay = PhotonNetwork.PlayerList.Length > 1;
        }

        public override void OnPlayerLeftRoom(Player otherPlayer)
        {
            canPlay = PhotonNetwork.PlayerList.Length > 1;
        }
        #endregion
    }
}