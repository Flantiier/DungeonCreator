using UnityEngine;

[CreateAssetMenu(fileName = "New AdventurerDatas", menuName = "Scriptables/Adventurers/BasicAdventurer")]
public class AdventurerDatas : ScriptableObject
{
    #region Variables
    [Header("Adventurer properties")]
    public float health = 100f;
    public float stamina = 100f;

    [Header("Adventurer combat stats")]
    public float attack = 20f;
    public float critAttack = 0.25f;
    public float defense = 25f;
    #endregion

    #region Methods
    public float GetAttackDamages()
    {
        return attack;
    }
    #endregion
}
