using System;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class TemporaryRooms : MonoBehaviourPunCallbacks
{
    #region Variables
    public enum GameEntity { Warrior, Wizard, Bowman, DungeonMaster }

    [Header("Connections infos")]
    [SerializeField] private GameEntity myEntity = GameEntity.Warrior;

    [Header("DungeonMaster infos")]
    [SerializeField] private GameObject masterPrefab;
    [SerializeField] private GameObject masterUI;
    [SerializeField] private Transform spawnPositionMaster;

    [Header("Adventurer infos")]
    [SerializeField] private SpawnAdventurerInfo warriorSpawn;
    [SerializeField] private SpawnAdventurerInfo wizardSpawn;
    [SerializeField] private SpawnAdventurerInfo bowmanSpawn;

    public static event Action<GameObject> OnEntityCreated;
    #endregion

    #region Callbacks
    public override void OnJoinedLobby()
    {
        //PhotonNetwork.JoinRandomOrCreateRoom();
    }

    public override void OnJoinedRoom()
    {
        InstantiateEntity();
    }

    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }
    #endregion

    #region Methods
    public void RaiseEntityEvent(GameObject obj)
    {
        OnEntityCreated?.Invoke(obj);
    }

    /// <summary>
    /// Instantiate the selected entity
    /// </summary>
    private void InstantiateEntity()
    {
        switch (myEntity)
        {
            case GameEntity.Warrior:
                InstantiateAdventurer(warriorSpawn);
                break;

            case GameEntity.Wizard:
                InstantiateAdventurer(wizardSpawn);
                break;

            case GameEntity.Bowman:
                InstantiateAdventurer(bowmanSpawn);
                break;

            case GameEntity.DungeonMaster:
                InstantiateMaster();
                break;
        }
    }

    /// <summary>
    /// Instantiate a new DungeonMaster over the network
    /// </summary>
    public void InstantiateMaster()
    {
        if (!masterPrefab)
        {
            Debug.LogError("DungeonMaster prefab missing !");
            return;
        }

        GameObject instance = PhotonNetwork.Instantiate(masterPrefab.name, GetSpawnPosition(spawnPositionMaster), Quaternion.identity);
        RaiseEntityEvent(instance);

        if(masterUI)
            Instantiate(masterUI);
    }

    /// <summary>
    /// Instantiate a new adventurer prefab over the network
    /// </summary>
    public void InstantiateAdventurer(SpawnAdventurerInfo spawnInfo)
    {
        if (!spawnInfo.prefab)
        {
            Debug.LogError("Adventurer prefab missing !");
            return;
        }

        GameObject instance = PhotonNetwork.Instantiate(spawnInfo.prefab.name, GetSpawnPosition(spawnInfo.position), Quaternion.identity);
        RaiseEntityEvent(instance);
    }

    /// <summary>
    /// Check if the transform target isn't null, if null return this gameObject position
    /// </summary>
    private Vector3 GetSpawnPosition(Transform target)
    {
        if (target)
            return target.position;

        return transform.position;
    }
    #endregion
}


#region SpawnAdventurer_Class
[System.Serializable]
public struct SpawnAdventurerInfo
{
    public GameObject prefab;
    public Transform position;
}
#endregion