using UnityEngine;

[CreateAssetMenu(fileName = "New Tiling", menuName = "Scriptables/Tiling")]
public class TilingSO : ScriptableObject
{
    [Header("Tiling Variables")]
    public GameObject tilePrefab;
    public float lengthX = 3f, lengthY = 3f;
}
