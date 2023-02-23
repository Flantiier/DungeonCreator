using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using Sirenix.OdinInspector;
using _Scripts.Multi.Connexion;

namespace _Scripts.Managers
{
    public class PlayersManager : MonoBehaviourPunCallbacks
    {
        #region Variables
        [SerializeField] private CharactersList characters;
        [TitleGroup("Spawn Properties")]
        [SerializeField] private Transform[] adventurersSpawn;
        [SerializeField] private Transform masterSpawn;
        [SerializeField] private Transform bossSpawn;

        [TitleGroup("Off Connexion Role")]
        [SerializeField] private ListPlayersRoom.Roles backUpRole = ListPlayersRoom.Roles.Warrior;
        #endregion

        #region Properties
        public static ListPlayersRoom.Roles Role { get; private set; }
        #endregion

        #region Builts_In
        private void Awake()
        {
            //Not connected to network
            if (!PhotonNetwork.IsConnected)
                return;

            //Spawn character
            InstantiateCharacter();
        }

        public override void OnJoinedRoom()
        {
            //Not connected to network
            if (!PhotonNetwork.IsConnected)
                return;

            //Spawn character
            InstantiateCharacter();
        }
        #endregion

        #region Methods
        /// <summary>
        /// Instantiates any character based on the selected role
        /// </summary>
        private void InstantiateCharacter()
        {
            ListPlayersRoom.Roles role;

            try {
                role = (ListPlayersRoom.Roles)PhotonNetwork.LocalPlayer.CustomProperties["role"];
            }
            catch {
                role = backUpRole;
            }

            Role = role;

            switch (role)
            {
                case ListPlayersRoom.Roles.DM:
                    Instantiate(characters.dungeonMaster.prefab, masterSpawn.position, masterSpawn.rotation);
                    Instantiate(characters.dungeonMaster.UI);
                    InstantiateBoss();
                    break;

                case ListPlayersRoom.Roles.Warrior:
                    PhotonNetwork.Instantiate(characters.warrior.prefab.name, adventurersSpawn[0].position, adventurersSpawn[0].rotation);
                    Instantiate(characters.warrior.UI);
                    break;

                case ListPlayersRoom.Roles.Archer:
                    PhotonNetwork.Instantiate(characters.archer.prefab.name, adventurersSpawn[1].position, adventurersSpawn[1].rotation);
                    Instantiate(characters.archer.UI);
                    break;

                case ListPlayersRoom.Roles.Wizard:
                    PhotonNetwork.Instantiate(characters.wizard.prefab.name, adventurersSpawn[2].position, adventurersSpawn[2].rotation);
                    Instantiate(characters.wizard.UI);
                    break;
            }
        }

        /// <summary>
        /// Spwan the boss prefab
        /// </summary>
        private void InstantiateBoss()
        {
            GameObject instance = PhotonNetwork.Instantiate(characters.boss.prefab.name, bossSpawn.position, Quaternion.identity);
            instance.SetActive(false);
        }
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
