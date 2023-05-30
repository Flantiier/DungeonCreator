using UnityEngine;
using Sirenix.OdinInspector;

[CreateAssetMenu(fileName = "New Tiling Properties", menuName = "SO/Game Management/Tiling")]
public class TilingSO : ScriptableObject
{
    [TitleGroup("Tiling Variables")]
    public GameObject tilePrefab;
    public float lengthX = 3f, lengthY = 3f;
}
