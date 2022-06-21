using UnityEngine;

public class Tile : MonoBehaviour
{
    public Material baseMaterial;
    public Material selectedMaterial;
    private Renderer _renderer;

    private void Awake()
    {
        _renderer = GetComponent<Renderer>();
    }

    public void OnSelected()
    {
        Debug.Log("Is Selectionned");
    }
}