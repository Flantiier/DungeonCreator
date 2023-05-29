using System.Collections;
using UnityEngine;
using Photon.Pun;
using _ScriptableObjects.GameManagement;
using Sirenix.OdinInspector;
using static Utils.Utilities.Time;
using TMPro;
using UnityEngine.SceneManagement;

public enum EndGameReason
{
    TimeLeft,
    AdventurerWin,
    MasterWin
}

namespace _Scripts.Managers
{
    public class GameManager : NetworkMonoSingleton<GameManager>
    {
        #region Variables
        [FoldoutGroup("Game steps")]
        [SerializeField] private GameProperties gameProperties;
        [FoldoutGroup("Game steps")]
        [SerializeField] private FloatVariable timeVariable;

        [Header("EndGame UI")]
        [SerializeField] private GameObject endPanel;
        [SerializeField] private string menuScene = "MainScreen";
        [SerializeField] private Color winColor = Color.white, loseColor = Color.red;
        private TextMeshProUGUI _title;
        private TextMeshProUGUI _descrip;
        #endregion

        #region Builts_In
        public override void Awake()
        {
            base.Awake();

            if (PhotonNetwork.IsConnected)
            {
                if (PhotonNetwork.IsMasterClient)
                    StartCoroutine(StartPhaseRoutine(gameProperties.startPhase, "StartPhaseEventRPC"));
            }
            else
                Debug.LogWarning("No connected the game hasn't started");

            _title = endPanel.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
            _descrip = endPanel.transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        }
        #endregion

        #region Methods

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

        private IEnumerator GameRoutine(GameStep step)
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

            EndGame(EndGameReason.TimeLeft);
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
                StartCoroutine(GameRoutine(gameProperties.game));
        }

        [PunRPC]
        private void GameEventRPC()
        {
            gameProperties.game.gameEvent.Raise();
        }
        #endregion

        #region EndGame Methods
        public void EndGame(EndGameReason reason)
        {
            StopAllCoroutines();

            switch (reason)
            {
                case EndGameReason.TimeLeft:
                    TimesLeft();
                    break;
                case EndGameReason.AdventurerWin:
                    AdventurersWin();
                    break;
                case EndGameReason.MasterWin:
                    MasterWin();
                    break;
            }

            endPanel.SetActive(true);
            StartCoroutine("EndGameRoutine");
        }

        private IEnumerator EndGameRoutine()
        {
            yield return new WaitForSecondsRealtime(5f);

            PhotonNetwork.LeaveRoom();
            SceneManager.LoadScene(menuScene);
        }

        private void TimesLeft()
        {
            if (PlayersManager.Role == Role.Master)
            {
                _title.text = "Victoire !";
                _title.color = winColor;
                _descrip.text = "Vous avez empeche les aventuriers de s'echapper.";
            }
            else
            {
                _title.text = "Defaite...";
                _title.color = loseColor;
                _descrip.text = "Vous n'avez pas reussi a vous echapper du donjon.";
            }
        }

        private void AdventurersWin()
        {
            if (PlayersManager.Role != Role.Master)
            {
                _title.text = "Victoire !";
                _title.color = winColor;
                _descrip.text = "Vous avez vaincu le maitre du donjon ! Echappez vous avec honneur.";
            }
            else
            {
                _title.text = "Defaite...";
                _title.color = loseColor;
                _descrip.text = "Vous n'avez pas reussi a defendre votre donjon... Vous vous etes bien battu";
            }
        }

        private void MasterWin()
        {
            if (PlayersManager.Role == Role.Master)
            {
                _title.text = "Victoire !";
                _title.color = winColor;
                _descrip.text = "Vous avez reussi a defendre votre donjon ! Continuez de faire regner l'ordre dans votre donjon.";
            }
            else
            {
                _title.text = "Defaite...";
                _title.color = loseColor;
                _descrip.text = "Le maitre du donjon ne vous a laisse aucune chance... Vous etiez si pret du but.";
            }
        }
        #endregion

        #endregion
    }
}