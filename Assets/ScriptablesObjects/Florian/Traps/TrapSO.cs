using UnityEngine;

[CreateAssetMenu(fileName = "New Trap", menuName = "Scriptables/Trap")]
public class TrapSO : ScriptableObject
{
    public Trap trapInstance;
}


[System.Serializable]
public struct Trap
{
    [Tooltip("Trap prefab to instantiate")]
    public GameObject trapPrefab;

    [Range(1, 10), Tooltip("XAmount of required tiles")]
    public int xAmount;

    [Range(1, 10), Tooltip("YAmount of required tiles")]
    public int yAmount;
}
