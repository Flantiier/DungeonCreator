using UnityEngine;
using Photon.Pun;
using Sirenix.OdinInspector;
using _Scripts.UI.Interfaces;
using _Scripts.Characters;
using _ScriptableObjects.GameManagement;

namespace _Scripts.Managers
{
    public class PlayersManager : MonoBehaviourPunCallbacks
    {
        #region Variables
        [TitleGroup("Spawn Properties")]
        [SerializeField] private GameProperties properties;
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
        public static Role Role { get; private set; }
        #endregion

        #region Builts_In
        private void Awake()
        {
            Role = properties.role;

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
            switch (Role)
            {
                case Role.Master:
                    Instantiate(characters.dungeonMaster.prefab, masterSpawn.position, masterSpawn.rotation);
                    Instantiate(characters.dungeonMaster.UI);
                    PhotonNetwork.Instantiate(characters.boss.prefab.name, bossSpawn.position, bossSpawn.rotation);
                    spawnPosition.value = masterSpawn.position;
                    break;

                case Role.Warrior:
                    Character warrior = PhotonNetwork.Instantiate(characters.warrior.prefab.name, adventurersSpawn[0].position, adventurersSpawn[0].rotation).GetComponent<Character>();
                    InstantiateCharacterHUD(characters.warrior.UI, warrior);
                    spawnPosition.value = adventurersSpawn[0].position;
                    break;

                case Role.Archer:
                    Character archer = PhotonNetwork.Instantiate(characters.archer.prefab.name, adventurersSpawn[1].position, adventurersSpawn[1].rotation).GetComponent<Character>();
                    InstantiateCharacterHUD(characters.archer.UI, archer);
                    spawnPosition.value = adventurersSpawn[1].position;
                    break;

                case Role.Wizard:
                    Character wizard = PhotonNetwork.Instantiate(characters.wizard.prefab.name, adventurersSpawn[2].position, adventurersSpawn[2].rotation).GetComponent<Character>();
                    InstantiateCharacterHUD(characters.wizard.UI, wizard);
                    spawnPosition.value = adventurersSpawn[2].position;
                    break;
            }
        }

        /// <summary>
        /// Instantiate a character UI and reference the instance of it
        /// </summary>
        /// <param name="ui"> UI prefab </param>
        /// <param name="character"> Character reference </param>
        private void InstantiateCharacterHUD(GameObject ui, Character character)
        {
            GameObject instance = Instantiate(ui);
            PlayerHUD hud = instance.GetComponent<PlayerHUD>();
            instance.SetActive(false);
            hud.SetTargetCharacter(character);
            instance.SetActive(true);
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
