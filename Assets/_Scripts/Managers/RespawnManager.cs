using TMPro;
using System;
using System.Collections;
using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;
using Photon.Pun;
using Photon.Realtime;
using Sirenix.OdinInspector;
//
using Utils;
using _Scripts.Characters;
using _ScriptableObjects.GameManagement;

namespace _Scripts.Managers
{
    public class RespawnManager : MonoBehaviourSingleton<RespawnManager>
    {
        #region Variables
        [TitleGroup("References")]
        [SerializeField] private GameObject respawnCanvas;
        [TitleGroup("References")]
        [SerializeField] private GameEvent respawnStart, respawnEnd;

        [FoldoutGroup("Variables")]
        [SerializeField] private FloatVariable timeVariable;
        [FoldoutGroup("Variables")]
        [SerializeField] private FloatVariable respawnTime;
        [FoldoutGroup("Variables")]
        [SerializeField] private Vector3Variable respawnPosition;

        [FoldoutGroup("Inputs")]
        [SerializeField] private InputActionMap inputMap;

        //Players
        private Character[] _players;
        private Character _myPlayer;
        private CinemachineVirtualCamera _cam;
        //GUI
        private TextMeshProUGUI _nameField;
        private TextMeshProUGUI _timeField;
        private int _currentIndex;
        #endregion

        #region Properties
        public bool _respawnEnabled { get; set; } = true;
        #endregion

        #region Builts_In
        private void Start()
        {
            EnableRespawn(true);
            EnableInputs(false);

            _timeField = respawnCanvas.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
            _nameField = respawnCanvas.transform.GetChild(1).GetComponentInChildren<TextMeshProUGUI>();
        }

        private void OnEnable()
        {
            Character.OnCharacterDeath += StartRespawn;
            inputMap.FindAction("Next").started += OnNextAction;
            inputMap.FindAction("Previous").started += OnPreviousAction;
        }

        private void OnDisable()
        {
            Character.OnCharacterDeath -= StartRespawn;
            inputMap.FindAction("Next").started += OnNextAction;
            inputMap.FindAction("Previous").started += OnPreviousAction;
        }
        #endregion

        #region Respawn methods
        /// <summary>
        /// Indicates if players can respawn or not
        /// </summary>
        public void EnableRespawn(bool enabled)
        {
            _respawnEnabled = enabled;
        }

        /// <summary>
        /// Start a coroutine to respawn the player
        /// </summary>
        public void StartRespawn(Character character)
        {
            //Spectateur
            EnableInputs(true);
            EnableRespawnCanvas(true);
            GetPlayersList();
            GameManager.Instance.EnableCursor(true);
            _currentIndex = Array.FindIndex(_players, p => p == _myPlayer);
            _cam.LookAt = _myPlayer.LookAt;
            _cam.Follow = _myPlayer.LookAt;

            //Blocking respawn if disabled
            if (!_respawnEnabled)
            {
                _timeField.text = "reapparition impossible.";
                return;
            }

            //Start the respawn coldoown and raise an event
            respawnStart.Raise();
            StartCoroutine(RespawnRoutine(character));

            //Load map
            if (SubsceneLoader.Instance)
                SubsceneLoader.Instance.LoadAll();
        }

        /// <summary>
        /// Set the player position to a target position
        /// </summary>
        private void RespawnPlayer(Character character)
        {
            if (!character)
                return;

            EnableInputs(false);
            EnableRespawnCanvas(false);
            GameManager.Instance.EnableCursor(false);

            //Respawn player
            ResetCameraOnPlayer();
            character.TeleportPlayer(respawnPosition.value);
        }

        /// <summary>
        /// Respawn coroutine
        /// </summary>
        /// <param name="character"> Character to respawn </param>
        private IEnumerator RespawnRoutine(Character character)
        {
            respawnTime.value = GetRespawnTime();

            while (respawnTime.value > 0)
            {
                respawnTime.value -= Time.deltaTime;
                _timeField.text = "Reapparition dans : " + Mathf.Ceil(respawnTime.value).ToString();

                yield return null;
            }

            //Load map start
            if (SubsceneLoader.Instance)
                SubsceneLoader.Instance.LoadMapStart();

            yield return new WaitForSecondsRealtime(0.5f);

            //Reset respawn
            respawnTime.value = 0;
            RespawnPlayer(character);
            respawnEnd.Raise();
        }

        /// <summary>
        /// Get the required time to respawn
        /// </summary>
        private float GetRespawnTime()
        {
            float time = 5f;
            foreach (RespawnUnit unit in GameManager.Instance.Properties.respawnInfos)
            {
                float max = Utilities.Time.ConvertTime(unit.maxTime, unit.timeUnit, Utilities.Time.TimeUnit.Seconds);
                float min = Utilities.Time.ConvertTime(unit.minTime, unit.timeUnit, Utilities.Time.TimeUnit.Seconds);

                if (timeVariable.value > min && timeVariable.value <= max)
                {
                    time = unit.respawnDelay;
                    break;
                }
                else
                    continue;
            }

            return time;
        }
        #endregion

        #region GUI Methods
        /// <summary>
        /// Enable respawn canvas 
        /// </summary>
        private void EnableRespawnCanvas(bool enabled)
        {
            respawnCanvas.SetActive(enabled);
        }

        /// <summary>
        /// Get players to watch
        /// </summary>
        private void GetPlayersList()
        {
            _players = FindObjectsOfType<Character>();

            if (_myPlayer)
                return;

            //Get players transform
            for (int i = 0; i < _players.Length; i++)
            {
                //Check if its the local player
                if (!_players[i].GetComponent<PhotonView>().IsMine)
                    continue;

                _myPlayer = _players[i];
                _cam = _players[i].Camera.VCam;
            }
        }

        /// <summary>
        /// Reset camera on player
        /// </summary>
        private void ResetCameraOnPlayer()
        {
            _cam.Follow = _myPlayer.LookAt;
            _cam.LookAt = _myPlayer.LookAt;
        }

        /// <summary>
        /// Look at another player
        /// </summary>
        public void SwitchPlayer(int value)
        {
            if (_players.Length <= 0)
                return;

            //Get look at target on character
            _currentIndex = _currentIndex + value >= _players.Length ? 0 : _currentIndex + value < 0 ? _players.Length - 1 : _currentIndex + value;
            Character character = _players[_currentIndex];
            Transform lookAt = character.LookAt;

            //Switch camera target
            _cam.LookAt = lookAt;
            _cam.Follow = lookAt;

            //Set text
            _nameField.text = GetPlayerName(character.View);
        }

        /// <summary>
        /// Get spectated player nickName
        /// </summary>
        private string GetPlayerName(PhotonView view)
        {
            foreach (Player player in PhotonNetwork.PlayerList)
            {
                if (view.Controller != player)
                    continue;
                else
                    return player.NickName;
            }

            return "Player";
        }
        #endregion

        #region Callback Events
        private void EnableInputs(bool enable)
        {
            Utilities.Inputs.EnableInputMap(inputMap, enable);
        }

        private void OnNextAction(InputAction.CallbackContext ctx)
        {
            SwitchPlayer(1);
        }

        private void OnPreviousAction(InputAction.CallbackContext ctx)
        {
            SwitchPlayer(-1);
        }
        #endregion
    }
}
