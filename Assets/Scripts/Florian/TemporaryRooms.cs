using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class TemporaryRooms : MonoBehaviourPunCallbacks
{
    public GameObject masterPrefab;
    public GameObject adventurerPrefab;

    public override void OnJoinedLobby()
    {
        PhotonNetwork.JoinRandomOrCreateRoom();
    }

    public override void OnJoinedRoom()
    {
        if (PhotonNetwork.MasterClient.IsLocal)
        {
            Debug.Log("Master client"); 
            PhotonNetwork.Instantiate(masterPrefab.name, Vector3.zero, Quaternion.identity);
        }
        else
        {
            InstantiateAdventurer();
            Debug.Log("Client");
        }
    }

    public void InstantiateAdventurer()
    {
        PhotonNetwork.Instantiate(adventurerPrefab.name, new Vector3(0f, 5f, 0f), Quaternion.identity);
    }

    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }
}
