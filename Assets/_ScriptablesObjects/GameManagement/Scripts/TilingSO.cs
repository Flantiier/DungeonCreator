using UnityEngine;
using Sirenix.OdinInspector;

[CreateAssetMenu(fileName = "New Tiling Properties", menuName = "Traps/Tiling Properties")]
public class TilingSO : ScriptableObject
{
    [TitleGroup("Tiling Variables")]
    public GameObject tilePrefab;
    public float lengthX = 3f, lengthY = 3f;
}
