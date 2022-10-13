using UnityEngine;

[CreateAssetMenu(fileName = "New AdventurerDatas", menuName = "Scriptables/Datas/Adventurer")]
public class AdventurerData : ScriptableObject
{
    [Header("Player Info")]
    public float health = 10f;
    public float stamina = 2f;

    [Header("Movements Info")]
    public float walkSpeed = 5f;
    public float runSpeed = 8f;
}
