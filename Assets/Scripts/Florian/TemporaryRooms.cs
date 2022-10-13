using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class TemporaryRooms : MonoBehaviourPunCallbacks
{
    public GameObject masterPrefab;
    public GameObject masterUI;
    public Transform spawnMasterPosition;

    public GameObject adventurerPrefab;
    public Transform spawnAdventurerPosition;

    public override void OnJoinedLobby()
    {
        PhotonNetwork.JoinRandomOrCreateRoom();
    }

    public override void OnJoinedRoom()
    {
        /*if (PhotonNetwork.MasterClient.IsLocal)
        {
            InstantiateMasterRPC();
        }
        else
        {*/
            InstantiateAdventurerRPC();
        //}
    }
    public void InstantiateMasterRPC()
    {
        PhotonNetwork.Instantiate(masterPrefab.name, spawnMasterPosition.position, Quaternion.identity);
        Instantiate(masterUI, null);
    }

    public void InstantiateAdventurerRPC()
    {
        PhotonNetwork.Instantiate(adventurerPrefab.name, spawnAdventurerPosition.position, Quaternion.identity);
    }

    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }
}
