using UnityEngine;
using Photon.Pun;
using _ScriptablesObjects.GameManagement;

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
        public GameSettings GameSettings => gameSettings;
        public GameStatements GameStatement { get; private set; } = new GameStatements();
        public bool ValidGame { get; private set; } = false;
        public float RemainingGameTime { get; private set; } = 0f;
        #endregion

        #region Builts_In
        public override void Awake()
        {
            base.Awake();

            RemainingGameTime = gameSettings.duration.GetMyTime();
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

            RemainingGameTime = time;
            RPCCall("SetGameTimeRPC", RpcTarget.Others, RemainingGameTime);
        }

        private void UpdateGameTime()
        {
            if (RemainingGameTime <= 0)
            {
                GameEnded();
                return;
            }

            RemainingGameTime -= Time.deltaTime;
            Mathf.Clamp(RemainingGameTime, 0f, Mathf.Infinity);
            RPCCall("SetGameTimeRPC", RpcTarget.Others, RemainingGameTime);
        }

        [PunRPC]
        public void SetGameTimeRPC(float updatedTime)
        {
            RemainingGameTime = updatedTime;
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