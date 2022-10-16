using UnityEngine;

[CreateAssetMenu(fileName = "New TrapsData", menuName = "Scriptables/Datas/Trap")]
public class TrapsData : ScriptableObject
{
    [Header("Trap Info")]
    public float damage;
}
