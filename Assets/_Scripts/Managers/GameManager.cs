using static Utils.Utilities.Time;
using TMPro;
using System.Collections;
using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;
using Cinemachine;
using Sirenix.OdinInspector;
using _ScriptableObjects.GameManagement;
using _Scripts.Characters.DungeonMaster;
using _Scripts.Characters;

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

        #region Game Steps
        [FoldoutGroup("Global Properties")]
        [SerializeField] private GameProperties gameProperties;
        [FoldoutGroup("Global Properties")]
        [SerializeField] private FloatVariable timeVariable;
        #endregion

        #region Boss Fight
        [FoldoutGroup("Boss Fight")]
        [SerializeField] private GameObject bossUI;
        [FoldoutGroup("Boss Fight")]
        [SerializeField] private GameObject advBossUI;
        [FoldoutGroup("Boss Fight")]
        [SerializeField] private GameEvent startBossFightEvent;
        [FoldoutGroup("Boss Fight")]
        [SerializeField] private Transform[] spawnPositions;

        private Character[] _adventurers;
        private BossController _boss;
        #endregion

        #region EndGame
        [FoldoutGroup("EndGame UI")]
        [SerializeField] private string menuScene = "MainScreen";
        [FoldoutGroup("EndGame UI")]
        [SerializeField] private GameObject endPanel;
        [FoldoutGroup("EndGame UI")]
        [SerializeField] private Color winColor = Color.white, loseColor = Color.red;

        private TextMeshProUGUI _title;
        private TextMeshProUGUI _descrip;
        #endregion

        #endregion

        #region Properties
        public GameProperties Properties => gameProperties;
        public bool BossFightStarted { get; private set; }
        #endregion

        #region Builts_In
        private void Start()
        {
            //Game Launch
            if (PhotonNetwork.IsConnected)
            {
                if (PhotonNetwork.IsMasterClient)
                    StartCoroutine(StartPhaseRoutine(gameProperties.startPhase));
            }

            //Disable Cursor
            EnableCursor(PlayersManager.Role == Role.Master);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.F1))
                StartBossFight();
        }

        private void LateUpdate()
        {/*
            if (!_checkEndGame || _ended)
                return;

            if(_boss.CurrentHealth <= 0)
            {
                EndGameRPC(EndGameReason.AdventurerWin);
                _ended = true;
            }
            else if (AdventurersDefeated())
            {
                EndGameRPC(EndGameReason.MasterWin);
                _ended = true;
            }*/
        }
        #endregion

        #region Methods
        /// <summary>
        /// Enable or disable the mouse cursor
        /// </summary>
        public void EnableCursor(bool visible)
        {
            Cursor.visible = visible;
        }

        #region GameSteps
        /// <summary>
        /// Routine that update the time variable on all clients
        /// </summary>
        private IEnumerator StartPhaseRoutine(GameStep step)
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
            RPCCall("StartPhaseEventRPC", RpcTarget.AllBuffered);
        }

        /// <summary>
        /// HAndle teh duration of the game
        /// </summary>
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

            RPCCall("EndGameRPC", RpcTarget.All, EndGameReason.TimeLeft);
        }

        [PunRPC]
        public void SetGameTimeRPC(float value)
        {
            timeVariable.value = value;
        }

        [PunRPC]
        public void StartPhaseEventRPC()
        {
            gameProperties.startPhase.gameEvent.Raise();

            if (PhotonNetwork.IsConnected && PhotonNetwork.IsMasterClient)
                StartCoroutine(GameRoutine(gameProperties.game));
        }

        [PunRPC]
        public void GameEventRPC()
        {
            gameProperties.game.gameEvent.Raise();
        }
        #endregion

        #region EndGame Methods
        /// <summary>
        /// Differents endgame methods
        /// </summary>
        [PunRPC]
        public void EndGameRPC(EndGameReason reason)
        {
            StopAllCoroutines();

            if (!endPanel)
                return;

            _title = endPanel.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
            _descrip = endPanel.transform.GetChild(1).GetComponent<TextMeshProUGUI>();

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

        /// <summary>
        /// Leave the room and go back to the menu
        /// </summary>
        private IEnumerator EndGameRoutine()
        {
            yield return new WaitForSecondsRealtime(5f);

            PhotonNetwork.LeaveRoom();
            SceneManager.LoadScene(menuScene);
        }

        /// <summary>
        /// Endgame method when ther's no time left
        /// </summary>
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

        /// <summary>
        /// Endgame method when the adventurers win
        /// </summary>
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

        /// <summary>
        /// Endgame method when the DM win
        /// </summary>
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

        #region BossFight Methods
        /// <summary>
        /// Teleport player if its an adventurer / Take the control of the boss for the DM
        /// </summary>
        [ContextMenu("Start Boss Fight")]
        public void StartBossFight()
        {
            Role role = PlayersManager.Role;

            switch (role)
            {
                case Role.Master:
                    SwicthDMToBoss();
                    Instantiate(bossUI);
                    EnableCursor(false);
                    break;
                default:
                    TeleportAdventurers(role);
                    Instantiate(advBossUI);
                    break;
            }

            //Find all players
            _adventurers = FindObjectsOfType<Character>();
            _boss = FindObjectOfType<BossController>();

            //Disable respawn
            GetComponent<RespawnManager>().enabled = false;

            //Unload map

            BossFightStarted = true;
            startBossFightEvent.Raise();
        }

        /// <summary>
        /// Swicth DM controller to Boss controller
        /// </summary>
        private void SwicthDMToBoss()
        {
            //Disable DM
            DMController dm = FindObjectOfType<DMController>();
            dm.DisableCharacter();

            //Enable Boss
            BossController boss = FindObjectOfType<BossController>();
            boss.EnableBoss();
        }

        /// <summary>
        /// Teleport the player in the boss area
        /// </summary>
        private void TeleportAdventurers(Role role)
        {
            Transform spawn = role == Role.Warrior ? spawnPositions[0] : role == Role.Archer ? spawnPositions[1] : spawnPositions[2];
            Character player = GetLocalPlayer();

            if (player)
            {
                //Reset la cam au cas ou
                CinemachineVirtualCamera vcam = player.MainCamera.parent.GetComponentInChildren<CinemachineVirtualCamera>();
                vcam.LookAt = player.LookAt;
                vcam.Follow = player.LookAt;
                //Teleport player
                player.TeleportPlayer(spawn);
            }
        }

        /// <summary>
        /// Return the local character
        /// </summary>
        private Character GetLocalPlayer()
        {
            Character[] characters = FindObjectsOfType<Character>();

            foreach (Character character in characters)
            {
                if (!character || !character.ViewIsMine())
                    continue;
                else
                    return character;
            }

            return null;
        }

        /// <summary>
        /// Look if all the adventurers are defeated
        /// </summary>
        private bool AdventurersDefeated()
        {
            foreach (Character item in _adventurers)
            {
                if (item.CurrentHealth > 0)
                    return false;
                else
                    continue;
            }

            return true;
        }
        #endregion

        #endregion

        #region Multiplayer Methods
        public void LeaveGame()
        {
            PhotonNetwork.LeaveRoom();
        }

        public override void OnLeftRoom()
        {
            SceneManager.LoadScene("BackToMainMenu");
        }
        #endregion
    }
}