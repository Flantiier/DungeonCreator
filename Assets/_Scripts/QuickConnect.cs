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
        ListPlayersRoom.Roles role = GetRole(index);

        _hash["role"] = role;
        PhotonNetwork.SetPlayerCustomProperties(_hash);
        text.text = role.ToString();
    }

    private ListPlayersRoom.Roles GetRole(int index)
    {
        switch (index)
        {
            case 0:
                return ListPlayersRoom.Roles.Warrior;
            case 1:
                return ListPlayersRoom.Roles.Archer;
            case 2:
                return ListPlayersRoom.Roles.Wizard;
            case 3:
                return ListPlayersRoom.Roles.DM;
            default:
                return ListPlayersRoom.Roles.Warrior;
        }
    }

    public void StartGame()
    {
        if (!PhotonNetwork.IsMasterClient)
            return;

        PhotonNetwork.LoadLevel(scene);
    }
}
