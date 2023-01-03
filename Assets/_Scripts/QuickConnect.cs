using UnityEngine;
using TMPro;
using Photon.Pun;
using ExitGames.Client.Photon;
using _Scripts.Multi.Connexion;

public class QuickConnect : MonoBehaviourPunCallbacks
{
    public string scene = "Florian"; 
    public GameObject UI;
    public TextMeshProUGUI text;

    private Hashtable _hash;

    private void Awake()
    {
        UI.SetActive(false);
        _hash = new Hashtable();
        PhotonNetwork.SetPlayerCustomProperties(_hash);
    }

    public override void OnJoinedRoom()
    {
        UI.SetActive(true);
    }

    public void SelectRole(int index)
    {
        ListPlayersRoom.Roles role = index == 0 ? ListPlayersRoom.Roles.DM : ListPlayersRoom.Roles.Adventurer;

        _hash["role"] = role;
        PhotonNetwork.SetPlayerCustomProperties(_hash);
    }

    public void SelectCharacter(int index)
    {
        _hash["character"] = index;
        PhotonNetwork.SetPlayerCustomProperties(_hash);

        text.text = $"Role : {_hash["role"]}, Character : {_hash["character"]}";
    }

    public void StartGame()
    {
        if (!PhotonNetwork.IsMasterClient)
            return;

        PhotonNetwork.LoadLevel(scene);
    }
}
