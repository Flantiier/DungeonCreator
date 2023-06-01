using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using Photon.Pun;
using Photon.Realtime;
using Sirenix.OdinInspector;
using _ScriptableObjects.GameManagement;

public enum Role
{
    Warrior,
    Archer,
    Wizard,
    Master
}

namespace _Scripts.UI.Menus
{
    public class PlayerList : MonoBehaviourPunCallbacks
    {
        #region Variables
        [TitleGroup("References")]
        [SerializeField] private GameProperties properties;
        [TitleGroup("References")]
        [SerializeField] private PlayerInfos GUIprefab;
        [TitleGroup("References")]
        [SerializeField] private Transform content;

        #region GUI
        [FoldoutGroup("GUI Elements")]
        [SerializeField] private TextMeshProUGUI errorText;
        [FoldoutGroup("GUI Elements")]
        [SerializeField] private Image readyButtonImage;
        [FoldoutGroup("GUI Elements")]
        [SerializeField] private Button readyButton;
        [FoldoutGroup("GUI Elements")]
        [SerializeField] private Button[] roleButtons;
        [FoldoutGroup("GUI Elements")]
        [SerializeField] private GameObject[] switchElements;

        [FoldoutGroup("Scenes infos")]
        [SerializeField] private string lobbyScene = "Lobby";
        [FoldoutGroup("Scenes infos")]
        [SerializeField] private string sceneName = "LoadingGame";
        [FoldoutGroup("Scenes infos")]
        [SerializeField] private float timeBeforeStarting = 5f;
        private int _lastRole;
        #endregion

        #region Error Texts
        [FoldoutGroup("Error properties")]
        [SerializeField, TextArea(2, 2)] private string missingPlayersText = "Can't start the game with only one player";
        [FoldoutGroup("Error properties")]
        [SerializeField, TextArea(2, 2)] private string missingDMText = "To start the game, one player should play Dungeon Master.";
        [FoldoutGroup("Error properties")]
        [SerializeField, TextArea(2, 2)] private string duplicateRoleText = "Can't be two players or more with the same character selected.";
        #endregion

        [FoldoutGroup("Events")]
        [SerializeField] private GameEvent loadGameEvent;
        public static System.Action<Player, bool> OnPlayerReady;

        private PhotonView view;
        private List<PlayerProperties> _players = new List<PlayerProperties>();
        private readonly List<PlayerInfos> _guiElements = new List<PlayerInfos>();
        #endregion

        #region Properties
        public bool LocalPlayerReady { get; private set; } = false;
        #endregion

        #region Builts_In
        private void Awake()
        {
            view = GetComponent<PhotonView>();

            errorText.text = missingPlayersText;
            roleButtons[0].interactable = false;

            if (!PhotonNetwork.IsMasterClient)
                return;

            foreach (Player player in PhotonNetwork.PlayerList)
                AddPlayer(player);
        }

        private void Start()
        {
            SetRole(0);
        }

        public override void OnEnable()
        {
            base.OnEnable();
            OnPlayerReady += PlayerReadyListener;
        }

        public override void OnDisable()
        {
            base.OnDisable();
            OnPlayerReady -= PlayerReadyListener;
        }

        private void Update()
        {
            bool start = CheckStartConditions();
            errorText.gameObject.SetActive(!start);
        }
        #endregion

        #region Methods
        public void LeaveRoom()
        {
            PhotonNetwork.LeaveRoom();
        }

        public void SetRole(int value)
        {
            value = Mathf.Clamp(value, 0, sizeof(Role) - 1);
            Role role = (Role)System.Enum.ToObject(typeof(Role), value);

            if (_lastRole == value)
                return;

            roleButtons[_lastRole].interactable = true;
            roleButtons[value].interactable = false;

            CharacterRoleListener(PhotonNetwork.LocalPlayer, role);
            _lastRole = value;
            properties.role = (Role)System.Enum.ToObject(typeof(Role), _lastRole);
        }

        #region Listing Players
        /// <summary>
        /// Indicates if the Player List contains a given player
        /// </summary>
        private bool ContainsPlayer(Player player)
        {
            foreach (PlayerProperties infos in _players)
            {
                if (infos.player == player)
                    return true;
            }

            return false;
        }

        /// <summary>
        /// Add a player to the list
        /// </summary>
        /// <param name="newPlayer"></param>
        private void AddPlayer(Player newPlayer)
        {
            if (ContainsPlayer(newPlayer))
                return;

            PlayerProperties playerInstance = new PlayerProperties(newPlayer);
            _players.Add(playerInstance);

            //Update GUI
            PlayerInfos instance = Instantiate(GUIprefab, content);
            instance.SetPlayerGUI(playerInstance);
            _guiElements.Add(instance);
        }

        /// <summary>
        /// Overload the add player to use it withg rpc
        /// </summary>
        private void AddPlayer(Player player, Role role, bool isReady)
        {
            PlayerProperties playerInstance = new PlayerProperties(player);
            playerInstance.role = role;
            playerInstance.isReady = isReady;
            _players.Add(playerInstance);

            //Update GUI
            PlayerInfos instance = Instantiate(GUIprefab, content);
            instance.SetPlayerGUI(playerInstance);
            _guiElements.Add(instance);
        }

        /// <summary>
        /// Remove a player from the list
        /// </summary>
        private void RemovePlayer(Player player)
        {
            if (!ContainsPlayer(player))
                return;

            PlayerProperties target = _players.Find(x => x.player == player);
            _players.Remove(target);

            //Update GUI
            int index = _guiElements.FindIndex(x => x.Infos.player == player);
            Destroy(_guiElements[index].gameObject);
            _guiElements.RemoveAt(index);
        }

        /// <summary>
        /// Indicates if the player is ready or not
        /// </summary>
        public void PlayerReady()
        {
            PlayerProperties player = _players.Find(x => x.player == PhotonNetwork.LocalPlayer);
            PlayerReadyListener(player.player, !player.isReady);
        }
        #endregion

        #region Game Conditions
        private bool CheckStartConditions()
        {
            //Need more player
            if (PhotonNetwork.PlayerList.Length <= 1)
            {
                errorText.text = "The game can't start with only one player.";
                return false;
            }

            //DM Check
            if (!LookingForDM())
                return false;

            //Duplicate role
            if (DuplicateRoleCheck())
                return false;

            Debug.Log("Can start timer to load");
            return true;
        }

        /// <summary>
        /// Check if there's a player with the role Master
        /// </summary>
        private bool LookingForDM()
        {
            foreach (PlayerProperties player in _players)
            {
                if (player.role == Role.Master)
                    return true;
            }

            //Throw error because no DM
            errorText.text = missingDMText;
            Debug.Log("ERROR : No one is playing DM");
            return false;
        }

        /// <summary>
        /// Indicates if two player or more has the same role
        /// </summary>
        private bool DuplicateRoleCheck()
        {
            foreach (PlayerProperties player in _players)
            {
                foreach (PlayerProperties item in _players)
                {
                    if (player.player == item.player)
                        continue;

                    if (player.role == item.role)
                    {
                        //Throw an error because there's a duplicate role
                        errorText.text = duplicateRoleText;
                        Debug.Log("ERROR : Duplicate role");
                        return true;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// Enable or disable UI elements
        /// </summary>
        [ContextMenu("Switch")]
        private void SwitchElements()
        {
            foreach (GameObject obj in switchElements)
                obj.SetActive(!obj.activeInHierarchy);
        }

        /// <summary>
        /// Start the timer before the loadign screen
        /// </summary>
        private void CheckLoading()
        {
            if (!CheckStartConditions())
                return;

            bool allReady = true;
            foreach (PlayerProperties player in _players)
            {
                if (player.isReady)
                    continue;

                allReady = false;
                break;
            }

            if (!allReady)
                return;

            SwitchElements();
            StartCoroutine("StartGameRoutine");
        }

        private IEnumerator StartGameRoutine()
        {
            properties.role = (Role)System.Enum.ToObject(typeof(Role), _lastRole);

            loadGameEvent.Raise();
            yield return new WaitForSecondsRealtime(timeBeforeStarting);

            Debug.Log("Load Screen");
            PhotonNetwork.LoadLevel(sceneName);
        }
        #endregion

        #endregion

        #region RPC
        private void CharacterRoleListener(Player player, Role role)
        {
            view.RPC("UpdateCharacterRoleRPC", RpcTarget.All, player, role);
        }

        [PunRPC]
        private void UpdateCharacterRoleRPC(Player player, Role role)
        {
            int i = _players.FindIndex(x => x.player == player);
            _players.ElementAt(i).role = role;
            _guiElements.ElementAt(i).Infos.role = role;
            _guiElements.ElementAt(i).SetRoleInfos(role);
        }

        private void PlayerReadyListener(Player player, bool value)
        {
            view.RPC("PlayerReadyRPC", RpcTarget.All, player, value);
            LocalPlayerReady = value;
            readyButton.GetComponent<CanvasGroup>().alpha = !LocalPlayerReady ? 1 : 0.25f;
        }

        [PunRPC]
        private void PlayerReadyRPC(Player player, bool value)
        {
            int i = _players.FindIndex(x => x.player == player);
            _players.ElementAt(i).isReady = value;
            _guiElements.ElementAt(i).SetReady(value);

            CheckLoading();
        }

        private void CreateListRPC(Player newPlayer)
        {
            int size = _players.Count;
            Player[] players = new Player[size];
            int[] roles = new int[size];
            bool[] readyStates = new bool[size];

            for (int i = 0; i < size; i++)
            {
                PlayerProperties properties = _players[i];
                players[i] = properties.player;
                roles[i] = (int)properties.role;
                readyStates[i] = properties.isReady;
            }

            view.RPC("SendPlayerListRPC", RpcTarget.Others, newPlayer, players, roles, readyStates);
        }

        [PunRPC]
        private void SendPlayerListRPC(Player newPlayer, Player[] players, int[] roles, bool[] readyStates)
        {
            if (PhotonNetwork.LocalPlayer != newPlayer)
                return;

            _players = new List<PlayerProperties>();
            for (int i = 0; i < players.Length; i++)
                AddPlayer(players[i], (Role)System.Enum.ToObject(typeof(Role), roles[i]), readyStates[i]);
        }
        #endregion

        #region Callbacks
        public override void OnPlayerEnteredRoom(Player newPlayer)
        {
            AddPlayer(newPlayer);

            if (PhotonNetwork.IsMasterClient)
                CreateListRPC(newPlayer);
        }

        public override void OnPlayerLeftRoom(Player otherPlayer)
        {
            //Find another master client
            if (otherPlayer.IsMasterClient)
            {
                int random = Random.Range(0, _players.Count);

                if (random >= 0)
                    PhotonNetwork.SetMasterClient(_players[random].player);
            }

            //Remove from list
            if (!ContainsPlayer(otherPlayer))
                return;

            RemovePlayer(otherPlayer);
        }

        public override void OnLeftRoom()
        {
            SceneManager.LoadScene(lobbyScene);
        }
        #endregion
    }
}

#region PlayerRoomProperties class
public class PlayerProperties
{
    public Player player;
    public Role role;
    public bool isReady;

    public PlayerProperties(Player m_player)
    {
        player = m_player;
        role = Role.Warrior;
        isReady = false;
    }
}
#endregion
