using UnityEngine;
using Sirenix.OdinInspector;

[CreateAssetMenu(fileName = "New Int", menuName = "SO/Variables/Int")]
[InlineEditor]
public class IntVariable : ScriptableObject
{
    public int value = 0;
}
