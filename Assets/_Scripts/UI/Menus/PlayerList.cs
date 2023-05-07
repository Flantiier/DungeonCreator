using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using Sirenix.OdinInspector;

namespace _Scripts.UI.Menus
{
    public class PlayerList : MonoBehaviourPunCallbacks
    {
        [SerializeField] private PhotonView view;
        [SerializeField] private PlayerInfos GUIprefab;
        [SerializeField] private Transform content;

        [ShowInInspector] private readonly List<PlayerProperties> _players = new List<PlayerProperties>();
        private readonly List<PlayerInfos> _guiElements = new List<PlayerInfos>();

        public static System.Action<Player, Role> OnRoleUpdated;

        #region Builts_In
        private void Awake()
        {
            //Add player currently in the room
            foreach (Player player in PhotonNetwork.PlayerList)
                AddPlayer(player);
        }

        public override void OnEnable()
        {
            base.OnEnable();
            OnRoleUpdated += UpdateCharacterRole;
        }

        public override void OnDisable()
        {
            base.OnDisable();
            OnRoleUpdated -= UpdateCharacterRole;
        }
        #endregion

        #region Methods
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
        #endregion

        #region RPC
        private void UpdateCharacterRole(Player player, Role role)
        {
            view.RPC("UpdateCharacterRoleRPC", RpcTarget.Others, player, role);
        }

        [PunRPC]
        private void UpdateCharacterRoleRPC(Player player, Role role)
        {
            int i = _players.FindIndex(x => x.player == player);
            _players.ElementAt(i).role = role;
            _guiElements.ElementAt(i).MyPlayer.role = role;
            _guiElements.ElementAt(i).SetRoleInfos(role);
        }
        #endregion

        #region Callbacks
        public override void OnPlayerEnteredRoom(Player newPlayer)
        {
            AddPlayer(newPlayer);
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

    public PlayerProperties(Player m_player)
    {
        player = m_player;
        role = Role.Warrior;
    }
}
