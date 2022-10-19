using UnityEngine;
using Photon.Pun;

public class TrapManager : MonoBehaviourPunCallbacks
{
    [Header("Trap Info")]
    public TrapsData trapSO;
    public TrapsDestructibleData trapDestructibleSO;

    // Start is called before the first frame update
    void Start()
    {
        gameObject.tag = "Trap";

        if(!trapSO) return;
        if(!trapDestructibleSO) return;
    }
}
