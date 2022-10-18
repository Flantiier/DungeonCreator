using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "New AbilitiesLayout", menuName = "Scriptables/AbilitiesLayout")]
public class PlayerAbilities : ScriptableObject
{
    public Ability[] currentAbilities = new Ability[2];
}
