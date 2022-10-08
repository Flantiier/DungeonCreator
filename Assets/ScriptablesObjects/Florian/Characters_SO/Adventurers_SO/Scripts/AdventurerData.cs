using UnityEngine;

[CreateAssetMenu(fileName = "New AdventurerDatas", menuName = "Scriptables/Datas/Adventurer")]
public class AdventurerData : ScriptableObject
{
    [Header("Movements Info")]
    public float moveSpeed = 5f;
    public float runSpeed = 8f;
}
