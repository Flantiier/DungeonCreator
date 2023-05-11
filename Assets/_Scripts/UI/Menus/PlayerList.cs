using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;
using Photon.Realtime;
using Sirenix.OdinInspector;
using UnityEngine.SceneManagement;

namespace _Scripts.UI.Menus
{
    public class PlayerList : MonoBehaviourPunCallbacks
    {
        #region Variables
        [Header("References")]
        [SerializeField] private PhotonView view;
        [SerializeField] private PlayerInfos GUIprefab;
        [SerializeField] private Transform content;

        [Header("GUI Elements")]
        [SerializeField] private TextMeshProUGUI errorText;
        [SerializeField] private Image readyButtonImage;
        [SerializeField] private Button readyButton;
        [SerializeField] private Color readyColor, notReadyColor;

        [Header("Scenes infos")]
        [SerializeField] private string lobbyScene = "Lobby";
        [SerializeField] private string sceneName = "LoadingGame";
        [SerializeField] private float timeBeforeStarting = 5f;

        [Header("Events")]
        [SerializeField] private GameEvent loadGameEvent;

        [ShowInInspector] private List<PlayerProperties> _players = new List<PlayerProperties>();
        private readonly List<PlayerInfos> _guiElements = new List<PlayerInfos>();

        public static System.Action<Player, Role> OnRoleUpdated;
        public static System.Action<Player, bool> OnPlayerReady;

        public bool LocalPlayerReady { get; private set; } = false;
        #endregion

        #region Builts_In
        private void Awake()
        {
            readyButtonImage.color = notReadyColor;

            if (!PhotonNetwork.IsMasterClient)
                return;

            foreach (Player player in PhotonNetwork.PlayerList)
                AddPlayer(player);
        }

        public override void OnEnable()
        {
            base.OnEnable();
            OnRoleUpdated += CharacterRoleListener;
            OnPlayerReady += PlayerReadyListener;
        }

        public override void OnDisable()
        {
            base.OnDisable();
            OnRoleUpdated -= CharacterRoleListener;
            OnPlayerReady -= PlayerReadyListener;
        }

        private void Update()
        {
            bool start = CheckStartConditions();
            errorText.gameObject.SetActive(!start);

            if (LocalPlayerReady)
                return;
            else
                readyButton.interactable = start;
        }
        #endregion

        #region Methods
        public void LeaveRoom()
        {
            PhotonNetwork.LeaveRoom();
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
            int index = _guiElements.FindIndex(x => x.MyPlayer.player == player);
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
            readyButtonImage.color = player.isReady ? readyColor : notReadyColor;
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
            errorText.text = "To start the game, one player should play Dungeon Master.";
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
                        errorText.text = "Can't be two players or more with the same character selected.";
                        Debug.Log("ERROR : Duplicate role");
                        return true;
                    }
                }
            }
            return false;
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

            StartCoroutine("StartGameRoutine");
        }

        private IEnumerator StartGameRoutine()
        {
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
            _guiElements.ElementAt(i).MyPlayer.role = role;
            _guiElements.ElementAt(i).SetRoleInfos(role);
        }

        private void PlayerReadyListener(Player player, bool value)
        {
            view.RPC("PlayerReadyRPC", RpcTarget.All, player, value);
            LocalPlayerReady = value;
        }

        [PunRPC]
        private void PlayerReadyRPC(Player player, bool value)
        {
            int i = _players.FindIndex(x => x.player == player);
            _players.ElementAt(i).isReady = value;
            _guiElements.ElementAt(i).MyPlayer.isReady = value;

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
            PhotonNetwork.JoinLobby();
        }
        #endregion
    }
}

public enum Role
{
    Warrior,
    Archer,
    Wizard,
    Master
}

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
