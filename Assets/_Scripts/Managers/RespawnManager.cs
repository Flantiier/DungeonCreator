using System.Collections;
using UnityEngine;
using Cinemachine;
using Photon.Pun;
using Utils;
using _Scripts.Characters;
using _ScriptableObjects.GameManagement;
using TMPro;
using Photon.Realtime;

namespace _Scripts.Managers
{

    public class RespawnManager : MonoBehaviour
    {
        #region Variables
        [Header("References")]
        [SerializeField] private GameObject respawnCanvas;
        [SerializeField] private GameEvent respawnStart, respawnEnd; 

        [Header("Properties")]
        [SerializeField] private GameProperties properties;
        [SerializeField] private FloatVariable timeVariable;
        [SerializeField] private FloatVariable respawnTime;
        [SerializeField] private Vector3Variable respawnPosition;

        private Transform[] _players;
        private PhotonView[] _view;
        private CinemachineVirtualCamera _cam;
        private TextMeshProUGUI _nameField;
        private TextMeshProUGUI _timeField;
        private int _index;
        #endregion

        #region Builts_In
        private void Start()
        {
            _nameField = respawnCanvas.transform.GetChild(1).GetComponent<TextMeshProUGUI>();
            _timeField = respawnCanvas.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        }

        private void OnEnable()
        {
            Character.OnCharacterDeath += StartRespawnDelay;
        }

        private void OnDisable()
        {
            Character.OnCharacterDeath -= StartRespawnDelay;
        }
        #endregion

        #region Respawn methods
        /// <summary>
        /// Start a coroutine to respawn the player
        /// </summary>
        public void StartRespawnDelay(Character character)
        {
            respawnStart.Raise();
            _cam = Camera.main.transform.parent.GetComponentInChildren<CinemachineVirtualCamera>();

            EnableCanvas(true);
            GetPlayers();
            SwicthPlayer(0);
            StartCoroutine(RespawnRoutine(character));
        }

        /// <summary>
        /// Set the player position to a target position
        /// </summary>
        private void RespawnPlayer(Character character)
        {
            EnableCanvas(false);
            ResetCameraOnPlayer();

            if (!character)
                return;

            character.gameObject.SetActive(false);
            character.transform.position = respawnPosition.value;
            character.transform.rotation = Quaternion.identity;
            character.gameObject.SetActive(true);
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

            respawnTime.value = 0;
            RespawnPlayer(character);
            respawnEnd.Raise();
        }

        [ContextMenu("Get time")]
        private float GetRespawnTime()
        {
            float time = 5f;
            foreach (RespawnUnit unit in properties.respawnInfos)
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

            Debug.Log("Respawn time : " + time);
            return time;
        }
        #endregion

        #region GUI Methods
        /// <summary>
        /// Enable respawn canvas 
        /// </summary>
        private void EnableCanvas(bool enabled)
        {
            respawnCanvas.SetActive(enabled);
        }

        /// <summary>
        /// Get players to watch
        /// </summary>
        private void GetPlayers()
        {
            Character[] temp = FindObjectsOfType<Character>();
            _players = new Transform[temp.Length];
            _view = new PhotonView[temp.Length];

            for (int i = 0; i < temp.Length; i++)
            {
                _players[i] = temp[i].transform;
                _view[i] = temp[i].GetComponent<PhotonView>();
            }
        }

        /// <summary>
        /// Reset camera on player
        /// </summary>
        private void ResetCameraOnPlayer()
        {
            foreach (Transform player in _players)
            {
                if (!player.GetComponent<PhotonView>().IsMine)
                    return;

                _cam.LookAt = player;
                _cam.Follow = player;
                break;
            }
        }

        /// <summary>
        /// Look at another player
        /// </summary>
        public void SwicthPlayer(int value)
        {
            if (_players.Length <= 0)
                return;

            int index = _index + value >= _players.Length ? 0 : _index + value < 0 ? _players.Length - 1 : _index + value;
            _index = index;
            Transform target = _players[index];

            _cam.LookAt = target;
            _cam.Follow = target;

            //Set text
            _nameField.text = GetPlayerName(target.GetComponent<PhotonView>());
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
    }
}
