using UnityEngine;
using Photon.Pun;
using _ScriptablesObjects.GameManagement;
using _Scripts.Utilities.Florian;
using UnityEditor.AnimatedValues;

namespace _Scripts.Managers
{
    public class GameManager : MonoBehaviourSingleton<GameManager>
    {
        #region Variables
        [Header("Game properties")]
        [SerializeField] private GameSettings gameSettings;

        [Header("Temporary")]
        [SerializeField] private int requiredPlayerNumber = 1;
        #endregion

        #region Properties
        public GameObject PlayerInstance { get; set; }
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

        public override void OnEnable()
        {
            TemporaryRooms.OnEntityCreated += ctx => PlayerInstance = ctx.gameObject;
        }

        public override void OnDisable()
        {
            TemporaryRooms.OnEntityCreated -= ctx => PlayerInstance = ctx.gameObject;
        }

        private void Update()
        {
            if (!PhotonNetwork.IsConnected || !PhotonNetwork.IsMasterClient)
                return;

            UpdateGameValidity();

            if (!ValidGame)
                return;

            UpdateGameTime();
        }
        #endregion

        #region Methods
        private void UpdateGameValidity()
        {
            if (GameStatement.IsStatementOf(GameStatements.Statements.Over))
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
        public void InitializeGameTime(float time)
        {
            StartGame();

            GameTime.RemainingTime = time;
            RPCCall("SetGameTimeRPC", RpcTarget.Others, GameTime.RemainingTime);
        }

        private void UpdateGameTime()
        {
            if (GameTime.RemainingTime <= 0)
            {
                GameEnded();
                return;
            }

            GameTime.RemainingTime -= Time.deltaTime * 5f;
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
        private void StartGame()
        {
            Debug.Log("The game begin !");
            RPCCall("SetGameStateRPC", RpcTarget.AllBuffered, GameStatements.Statements.InGame);
        }

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

        #endregion
    }
}

#region GameStatements_Class
[System.Serializable]
public class GameStatements
{
    public enum Statements { Connecting, Waiting, InGame, Over }
    public Statements CurrentState = Statements.Connecting;

    public bool IsStatementOf(Statements targetState)
    {
        return CurrentState == targetState;
    }
}
#endregion

#region GlobalGameTime_Class
[System.Serializable]
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