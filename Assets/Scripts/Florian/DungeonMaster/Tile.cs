using UnityEngine;

public class Tile : MonoBehaviour
{
    public Material baseMaterial;
    public Material selectedMaterial;
    private Renderer _renderer;

    public bool IsUsed { get; private set; }

    private void Awake()
    {
        _renderer = GetComponent<Renderer>();
    }

    public void OnSelected()
    {
        _renderer.material = selectedMaterial;
        Debug.Log("Is Selectionned");
    }

    public void OnDeselected() 
    {
        _renderer.material = baseMaterial;
    }
}