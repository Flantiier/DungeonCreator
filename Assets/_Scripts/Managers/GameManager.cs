using System.Collections;
using UnityEngine;
using Photon.Pun;
using _Scripts.NetworkScript;
using _ScriptableObjects.GameManagement;
using Sirenix.OdinInspector;
using static Utils.Utilities.Time;

namespace _Scripts.Managers
{
    public class GameManager : NetworkMonoBehaviour
    {
        #region Variables
        [FoldoutGroup("Game steps")]
        [SerializeField] private GameProperties gameProperties;
        [FoldoutGroup("Game steps")]
        [SerializeField] private FloatVariable timeVariable;
        #endregion

        #region Builts_In
        public void Awake()
        {
            if (PhotonNetwork.IsConnected)
            {
                if (PhotonNetwork.IsMasterClient)
                    StartCoroutine(StartPhaseRoutine(gameProperties.startPhase, "StartPhaseEventRPC"));
            }
            else
                Debug.LogWarning("No connected the game hasn't started");
        }
        #endregion

        #region Methods
        public void StartPhaseTest()
        {
            Debug.Log("Start Phase ended, starting the game now !");
        }

        #region GameSteps
        /// <summary>
        /// Routine that update the time variable on all clients
        /// </summary>
        private IEnumerator StartPhaseRoutine(GameStep step, string RPC)
        {
            //Set the time value
            timeVariable.value = GetTimeInSeconds(step.duration, step.TimeUnit);
            yield return new WaitForSecondsRealtime(0.1f);

            //Loops until its lower than 0
            while (timeVariable.value > 0)
            {
                timeVariable.value -= Time.deltaTime;
                RPCCall("SetGameTimeRPC", RpcTarget.OthersBuffered, timeVariable.value);
                yield return null;
            }

            //Reset value and call step event
            timeVariable.value = 0;
            RPCCall(RPC, RpcTarget.All);
        }

        /// <summary>
        /// Set the time variable RPC
        /// </summary>
        /// <param name="value"> Time value sent over the network </param>
        [PunRPC]
        private void SetGameTimeRPC(float value)
        {
            timeVariable.value = value;
        }

        [PunRPC]
        private void StartPhaseEventRPC()
        {
            gameProperties.startPhase.gameEvent.Raise();

            if (PhotonNetwork.IsConnected && PhotonNetwork.IsMasterClient)
                StartCoroutine(StartPhaseRoutine(gameProperties.game, "GameEventRPC"));
        }

        [PunRPC]
        private void GameEventRPC()
        {
            gameProperties.game.gameEvent.Raise();
        }
        #endregion

        #endregion
    }
}