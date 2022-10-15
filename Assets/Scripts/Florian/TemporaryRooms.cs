using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class TemporaryRooms : MonoBehaviourPunCallbacks
{
    #region Variables
    public enum GameEntity { Adventurer, DungeonMaster }

    [Header("Connections infos")]
    [SerializeField] private GameEntity myEntity = GameEntity.Adventurer; 

    [Header("DungeonMaster infos")]
    [SerializeField] private GameObject masterPrefab;
    [SerializeField] private GameObject masterUI;
    [SerializeField] private Transform spawnPositionMaster;

    [Header("Adventurer infos")]
    [SerializeField] private GameObject adventurerPrefab;
    [SerializeField] private Transform spawnPositionAdventurer;
    #endregion

    #region Callbacks
    public override void OnJoinedLobby()
    {
        PhotonNetwork.JoinRandomOrCreateRoom();
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
    /// <summary>
    /// Instantiate the selected entity
    /// </summary>
    private void InstantiateEntity()
    {
        switch (myEntity)
        {
            case GameEntity.Adventurer:
                InstantiateAdventurer();
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

        PhotonNetwork.Instantiate(masterPrefab.name, SpawnPosition(spawnPositionMaster), Quaternion.identity);
        //Instantiate(masterUI, null);
    }

    /// <summary>
    /// Instantiate a new Adventurer over the network
    /// </summary>
    public void InstantiateAdventurer()
    {
        if (!adventurerPrefab)
        {
            Debug.LogError("Adventurer prefab missing !");
            return;
        }

        PhotonNetwork.Instantiate(adventurerPrefab.name, SpawnPosition(spawnPositionAdventurer), Quaternion.identity);
    }

    /// <summary>
    /// Return transform position if missing reference
    /// </summary>
    private Vector3 SpawnPosition(Transform target)
    {
        if (target)
            return target.position;

        return transform.position;
    }
    #endregion
}
