using UnityEngine;

[CreateAssetMenu(fileName = "New AdventurerDatas", menuName = "Scriptables/Stats/Adventurer")]
public class AdventurerDatas : ScriptableObject
{
    public float health = 100f;
    public float stamina = 100f;
    public float attack = 20f;
    public float critAttack = 0.25f;
    public float defense = 25f;

    public float GetAttackDamages()
    {
        return attack;
    }
}
