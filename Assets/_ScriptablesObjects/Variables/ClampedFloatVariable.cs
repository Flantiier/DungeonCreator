using UnityEngine;
using Sirenix.OdinInspector;

[CreateAssetMenu(fileName = "New Float", menuName = "Variables/Clamped Float")]
[InlineEditor]
public class ClampedFloatVariable : FloatVariable
{
    public float maxValue = 10f;
}
