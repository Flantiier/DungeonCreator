using UnityEngine;
using Photon.Pun;
using Sirenix.OdinInspector;
using _Scripts.Multi.Connexion;
using _Scripts.UI.Interfaces;
using _Scripts.Characters;

namespace _Scripts.Managers
{
    public class PlayersManager : MonoBehaviourPunCallbacks
    {
        #region Variables
        [TitleGroup("Off Connexion Role")]
        [SerializeField] private ListPlayersRoom.Roles backUpRole = ListPlayersRoom.Roles.Warrior;

        [TitleGroup("Spawn Properties")]
        [SerializeField] private CharactersList characters;
        [TitleGroup("Spawn Properties")]
        [SerializeField] private Transform[] adventurersSpawn;
        [TitleGroup("Spawn Properties")]
        [SerializeField] private Transform masterSpawn;
        [TitleGroup("Spawn Properties")]
        [SerializeField] private Transform bossSpawn;

        [TitleGroup("Variables")]
        [SerializeField] private Vector3Variable spawnPosition;
        #endregion

        #region Properties
        public static ListPlayersRoom.Roles Role { get; private set; }
        #endregion

        #region Builts_In
        private void Awake()
        {
            //Not connected to network
            if (PhotonNetwork.IsConnectedAndReady)
                InstantiateCharacter();
        }

        public override void OnJoinedRoom()
        {
            //Spawn character
            InstantiateCharacter();
        }
        #endregion

        #region Characters Initialization
        /// <summary>
        /// Instantiates any character based on the selected role
        /// </summary>
        private void InstantiateCharacter()
        {
            ListPlayersRoom.Roles role;

            try
            {
                role = (ListPlayersRoom.Roles)PhotonNetwork.LocalPlayer.CustomProperties["role"];
            }
            catch
            {
                role = backUpRole;
            }

            Role = role;

            switch (role)
            {
                case ListPlayersRoom.Roles.DM:
                    Instantiate(characters.dungeonMaster.prefab, masterSpawn.position, masterSpawn.rotation);
                    Instantiate(characters.dungeonMaster.UI);
                    InstantiateBoss();
                    spawnPosition.value = masterSpawn.position;
                    break;

                case ListPlayersRoom.Roles.Warrior:
                    Character warrior = PhotonNetwork.Instantiate(characters.warrior.prefab.name, adventurersSpawn[0].position, adventurersSpawn[0].rotation).GetComponent<Character>();
                    InstantiateCharacterHUD(characters.warrior.UI, warrior);
                    spawnPosition.value = adventurersSpawn[0].position;
                    break;

                case ListPlayersRoom.Roles.Archer:
                    Character archer = PhotonNetwork.Instantiate(characters.archer.prefab.name, adventurersSpawn[1].position, adventurersSpawn[1].rotation).GetComponent<Character>();
                    InstantiateCharacterHUD(characters.archer.UI, archer);
                    spawnPosition.value = adventurersSpawn[1].position;
                    break;

                case ListPlayersRoom.Roles.Wizard:
                    Character wizard = PhotonNetwork.Instantiate(characters.wizard.prefab.name, adventurersSpawn[2].position, adventurersSpawn[2].rotation).GetComponent<Character>();
                    InstantiateCharacterHUD(characters.wizard.UI, wizard);
                    spawnPosition.value = adventurersSpawn[2].position;
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

        /// <summary>
        /// Instantiate a character UI and reference the instance of it
        /// </summary>
        /// <param name="ui"> UI prefab </param>
        /// <param name="character"> Character reference </param>
        private void InstantiateCharacterHUD(GameObject ui, Character character)
        {
            Instantiate(ui).GetComponent<PlayerHUD>().SetTargetCharacter(character);
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
