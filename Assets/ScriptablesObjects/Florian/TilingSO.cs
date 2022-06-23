using UnityEngine;

[CreateAssetMenu(fileName = "New Tiling", menuName = "Scriptables/Tiling")]
public class TilingSO : ScriptableObject
{
    [Header("Tiling Variables")]
    public GameObject tilePrefab;
    public float xOffset;
    public float yOffset;
}
