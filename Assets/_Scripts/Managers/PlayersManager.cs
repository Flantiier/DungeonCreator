using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using _Scripts.Multi.Connexion;
using _Scripts.Characters;

namespace _Scripts.Managers
{
    public class PlayersManager : MonoBehaviourSingleton<PlayersManager>
    {
        #region Variables
        [Header("Spawn properties")]
        [SerializeField] private CharacterSpawnInfo[] characters;
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
            base.Awake();
            Adventurers = new List<Player>();

            SetGameRoleOnJoined((ListPlayersRoom.Roles)PhotonNetwork.LocalPlayer.CustomProperties["role"], PhotonNetwork.LocalPlayer);
        }

        private void Start()
        {
            SpawnCharacter((int)PhotonNetwork.LocalPlayer.CustomProperties["character"]);

            if (!PhotonNetwork.IsMasterClient)
                return;

            SpawnBoss();
        }
        #endregion

        #region Methods
        /// <summary>
        /// Indicates if there is atleast one adventurer in the room 
        /// </summary>
        /// <returns></returns>
        public bool IsOneAdventurer()
        {
            return Adventurers.Count <= 0;
        }

        #region Spawn Methods
        /// <summary>
        /// Instantiates a prefab of character set in the array
        /// </summary>
        /// <param name="index"> Prefab index to instantiate </param>
        private void SpawnCharacter(int index)
        {
            GameObject playerToSpawn = characters[index].prefab;
            Vector3 spawnPoint = !characters[index].position ? Vector3.zero : characters[index].position.position;
            PlayerInstance = playerToSpawn;

            PlayerInstance = PhotonNetwork.Instantiate(playerToSpawn.name, spawnPoint, Quaternion.identity);
        }

        /// <summary>
        /// Spwan the boss prefab
        /// </summary>
        private void SpawnBoss()
        {
            if (!bossInfo.prefab)
                return;

            GameObject instance = bossInfo.prefab;
            Vector3 point = bossInfo.position.position;
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
    public Transform position;
}
#endregion
