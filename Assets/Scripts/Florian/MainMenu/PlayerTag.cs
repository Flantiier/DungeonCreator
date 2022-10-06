using UnityEngine;
using Photon.Realtime;

public class PlayerTag : MonoBehaviour
{
    [Header("Referencing the text to display the playerInfo")]
    [SerializeField] private TMPro.TextMeshProUGUI playerInfo;

    /// <summary>
    /// Instance of the playerInstance for this player
    /// </summary>
    public PlayerInstance playerInstance { get; set; }

    private void Awake()
    {
        ChangeTagState(false);
    }

    /// <summary>
    /// Setting info on the tag
    /// </summary>
    /// <param name="info">Text informations</param>
    public void SetPlayerInfo(PlayerInstance instance, string info)
    {
        playerInstance = instance;
        playerInfo.SetText(info);
        ChangeTagState(true);
    }

    public void ResetTag()
    {
        playerInstance = null;
        playerInfo.SetText("Empty");
        ChangeTagState(false);
    }

    public void ChangeTagState(bool state)
    {
        gameObject.SetActive(state);
    }
}


[System.Serializable]
public class PlayerInstance
{
    /// <summary>
    /// ActorNumber of this player
    /// </summary>
    public int ActorNumber { get; private set; }

    /// <summary>
    /// Player index of this player
    /// </summary>
    public int PlayerIndex { get; private set; }
    
    /// <summary>
    /// Instance of the Player class for this player
    /// </summary>
    public Player player { get; private set; }

    //Referencing Player class and actorNumber at creation of a new instance of playerInstance
    public PlayerInstance(Player _player, int _ActorNumber)
    {
        player = _player;
        ActorNumber = _ActorNumber;
    }

    /// <summary>
    /// Setting player index
    /// </summary>
    /// <param name="index">Player index</param>
    public void SetIndex(int index)
    {
        PlayerIndex = index;
    }
}
