using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using _Scripts.Multi.Connexion;
using _Scripts.Characters;
using System.Collections;

namespace _Scripts.Managers
{
    public class PlayersManager : NetworkMonoSingleton<PlayersManager>
    {
        #region Variables
        [Header("Spawn properties")]
        [SerializeField] private CharacterSpawnInfo[] characters;
        [SerializeField] private CharacterSpawnInfo dungeonMaster;
        [SerializeField] private CharacterSpawnInfo bossInfo;
        #endregion

        #region Properties
        public Player DungeonMaster { get; private set; }
        public List<Player> Adventurers { get; private set; }
        public GameObject PlayerInstance { get; private set; }
        public GameObject BossInstance { get; private set; }
        public List<Character> AdventurersInstances { get; private set; }
        #endregion

        #region Builts_In
        public override void Awake()
        {
            if (!PhotonNetwork.IsConnected)
            {
                gameObject.SetActive(false);
                return;
            }

            base.Awake();
            Adventurers = new List<Player>();

            SetGameRoleOnJoined((ListPlayersRoom.Roles)PhotonNetwork.LocalPlayer.CustomProperties["role"], PhotonNetwork.LocalPlayer);
        }

        private void Start()
        {
            SpawnCharacter((ListPlayersRoom.Roles)PhotonNetwork.LocalPlayer.CustomProperties["role"]);
        }
        #endregion

        #region Methods
        /// <summary>
        /// Indicates if there is a dungeon master
        /// </summary>
        public bool HasDungeonMaster()
        {
            return DungeonMaster != null;
        }
        /// <summary>
        /// Indicates if there is atleast one adventurer
        /// </summary>
        public bool HasAdventurers()
        {
            return Adventurers.Count > 0;
        }

        #region Spawn Methods
        /// <summary>
        /// Instantiates a prefab of character set in the array
        /// </summary>
        /// <param name="role"> Player role </param>
        private void SpawnCharacter(ListPlayersRoom.Roles role)
        {
            switch (role)
            {
                case ListPlayersRoom.Roles.DM:
                    PlayerInstance = Instantiate(dungeonMaster.prefab, dungeonMaster.spawnPoint.position, Quaternion.identity);
                    SpawnBoss();
                    break;

                default:
                    int index = (int)PhotonNetwork.LocalPlayer.CustomProperties["character"];
                    GameObject playerToSpawn = characters[index].prefab;
                    Vector3 spawnPoint = !characters[index].spawnPoint ? Vector3.zero : characters[index].spawnPoint.position;
                    PlayerInstance = playerToSpawn;
                    PlayerInstance = PhotonNetwork.Instantiate(playerToSpawn.name, spawnPoint, Quaternion.identity);
                    break;
            }
        }

        /// <summary>
        /// Spwan the boss prefab
        /// </summary>
        private void SpawnBoss()
        {
            if (!bossInfo.prefab)
                return;

            GameObject instance = bossInfo.prefab;
            Vector3 point = bossInfo.spawnPoint.position;
            BossInstance = instance;

            PhotonNetwork.Instantiate(instance.name, point, Quaternion.identity);
        }
        #endregion

        #region Roles Initialization
        private void SetGameRoleOnJoined(ListPlayersRoom.Roles role, Player player)
        {
            switch (role)
            {
                case ListPlayersRoom.Roles.DM:
                    RPCCall("SetDungeonMasterPlayer", RpcTarget.AllBuffered, player);
                    break;

                case ListPlayersRoom.Roles.Adventurer:
                    RPCCall("AddAdventurerPlayer", RpcTarget.AllBuffered, player);
                    break;
            }
        }

        [PunRPC]
        public void SetDungeonMasterPlayer(Player player)
        {
            if (DungeonMaster != null)
            {
                Debug.LogWarning("There's already a dungeon master");
                AddAdventurerPlayer(player);
                return;
            }

            DungeonMaster = player;
        }

        [PunRPC]
        public void AddAdventurerPlayer(Player player)
        {
            Adventurers.Add(player);
        }
        #endregion

        #endregion
    }
}

#region SpawnAdventurer_Class
[System.Serializable]
public struct CharacterSpawnInfo
{
    public GameObject prefab;
    public Transform spawnPoint;
}
#endregion
