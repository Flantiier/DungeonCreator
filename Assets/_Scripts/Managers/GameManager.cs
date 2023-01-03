using System;
using UnityEngine;
using Photon.Pun;
using _Scripts.Utilities.Florian;
using _ScriptableObjects.GameManagement;

namespace _Scripts.Managers
{
    public class GameManager : MonoBehaviourSingleton<GameManager>
    {
        #region Variables
        [Header("Game properties")]
        [SerializeField] private GameSettings gameSettings;

        [Header("Temporary")]
        [SerializeField] private int requiredPlayerNumber = 1;

        public event Action OnBossFightReached;
        public event Action OnBossFightStarted;
        #endregion

        #region Properties
        public GameSettings GameSettings => gameSettings;
        public GameStatements GameStatement { get; private set; } = new GameStatements();
        public bool ValidGame { get; private set; } = false;
        public GlobalGameTime GameTime { get; private set; }
        #endregion

        #region Builts_In
        public override void Awake()
        {
            base.Awake();

            GameTime = new GlobalGameTime();
            GameTime.RemainingTime = gameSettings.duration.GetTimeValue();
        }

        private void Update()
        {
            if (!PhotonNetwork.IsConnected || !PhotonNetwork.IsMasterClient)
                return;

            UpdateGameValidity();

            if (!ValidGame || GameStatement.IsStateOf(GameStatements.Statements.Over) || GameStatement.IsStateOf(GameStatements.Statements.BossFight))
                return;

            UpdateGameTime();
        }
        #endregion

        #region Methods
        /// <summary>
        /// Indicates if the game can be played (much players)
        /// </summary>
        private void UpdateGameValidity()
        {
            if (GameStatement.IsStateOf(GameStatements.Statements.Over))
                return;

            if (PhotonNetwork.PlayerList.Length >= requiredPlayerNumber)
            {
                ValidGame = true;
                RPCCall("SetGameStateRPC", RpcTarget.AllBuffered, GameStatements.Statements.InGame);
            }
            else
            {
                ValidGame = false;
                RPCCall("SetGameStateRPC", RpcTarget.AllBuffered, GameStatements.Statements.Waiting);
            }
        }

        #region Game Duration
        /// <summary>
        /// Start the game timer and send it into a rpc
        /// </summary>
        /// <param name="time"></param>
        public void InitializeGameTime(float time)
        {
            StartGame();

            GameTime.RemainingTime = time;
            RPCCall("SetGameTimeRPC", RpcTarget.Others, GameTime.RemainingTime);
        }

        /// <summary>
        /// Updates the game timer and indicates when it reached 0
        /// </summary>
        private void UpdateGameTime()
        {
            if (GameTime.RemainingTime <= 0)
            {
                GameEnded();
                return;
            }

            GameTime.RemainingTime -= Time.deltaTime;
            Mathf.Clamp(GameTime.RemainingTime, 0f, Mathf.Infinity);
            RPCCall("SetGameTimeRPC", RpcTarget.Others, GameTime.RemainingTime);
        }

        [PunRPC]
        public void SetGameTimeRPC(float updatedTime)
        {
            GameTime.RemainingTime = updatedTime;
        }
        #endregion

        #region GameState
        /// <summary>
        /// Set the game state to Game
        /// </summary>
        private void StartGame()
        {
            Debug.Log("The game begin !");
            RPCCall("SetGameStateRPC", RpcTarget.AllBuffered, GameStatements.Statements.InGame);
        }

        /// <summary>
        /// Set the game state to over
        /// </summary>
        private void GameEnded()
        {
            Debug.Log("The game is finished !");
            RPCCall("SetGameStateRPC", RpcTarget.AllBuffered, GameStatements.Statements.Over);
        }

        [PunRPC]
        public void SetGameStateRPC(GameStatements.Statements newState)
        {
            GameStatement.CurrentState = newState;
        }
        #endregion

        #region BossFight Manager
        /// <summary>
        /// Called when the boss fight is reached
        /// </summary>
        public void BossFightReached()
        {
            GameStatement.CurrentState = GameStatements.Statements.BossFight;
            OnBossFightReached?.Invoke();
        }

        /// <summary>
        /// Starting the fight boss
        /// </summary>
        public void StartBossFight()
        {
            OnBossFightStarted?.Invoke();
        }
        #endregion

        #endregion
    }
}

#region GameStatements_Class
[Serializable]
public class GameStatements
{
    public enum Statements { Connecting, Waiting, InGame, BossFight, Over }
    public Statements CurrentState = Statements.Connecting;

    public bool IsStateOf(Statements targetState)
    {
        return CurrentState == targetState;
    }
}
#endregion

#region GlobalGameTime_Class
[Serializable]
public class GlobalGameTime
{
    private float _remainingTime;
    public float RemainingTime
    {
        get => _remainingTime;
        set
        {
            _remainingTime = value;

            if (_remainingTime <= 0)
                return;

            SetMinuts(_remainingTime);
            SetSeconds(_remainingTime);
        }
    }
    public float RemainingMinuts { get; private set; }
    public float ClampedSeconds { get; private set; }

    /// <summary>
    /// Update the current number of minuts in the remaining time
    /// </summary>
    /// <param name="time"> Remaining time </param>
    public void SetMinuts(float time)
    {
        RemainingMinuts = (int)TimeFunctions.GetConvertedTime(time, TimeFunctions.TimeUnit.Minuts);
    }

    /// <summary>
    /// Update the current seconds by minuts
    /// </summary>
    /// <param name="time"> Remaining time </param>
    public void SetSeconds(float time)
    {
        ClampedSeconds = time % 60f;
    }
}
#endregion