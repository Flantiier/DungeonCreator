using UnityEngine;
using Sirenix.OdinInspector;

[CreateAssetMenu(fileName = "New Float", menuName = "Variables/Float")]
[InlineEditor]
public class FloatVariable : ScriptableObject
{
    public float value = 0f;
}