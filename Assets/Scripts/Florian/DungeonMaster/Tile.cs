using UnityEngine;

public class Tile : MonoBehaviour
{
    #region Tile Variables
    [Header("Tile Variables")]
    [SerializeField, Tooltip("Base material of the Tile")]
    private Material baseMaterial;

    [SerializeField, Tooltip("Selected material of the Tile")]
    private Material selectedMaterial;

    [SerializeField, Tooltip("Used material of the Tile")]
    private Material usedMaterial;

    [SerializeField, Tooltip("Renderer Component of the Tile")]
    private Renderer _renderer;

    public enum TileState
    {
        Deselected, Selected, Waiting, Used,
    }

    /// <summary>
    /// State of the Tile
    /// </summary>
    public TileState currentTileState { get; private set; }
    /// <summary>
    /// Indicates of the tile is used or not
    /// </summary>
    public bool IsUsed { get; private set; }
    #endregion

    #region Builts-In
    private void Awake()
    {
        NewTileState(TileState.Deselected);
    }
    #endregion

    #region Tile State Methods
    public void NewTileState(TileState newState)
    {
        currentTileState = newState;

        switch (newState)
        {
            case TileState.Deselected:
                DeselectingState();
                break;
            case TileState.Selected:
                SelectingState();
                break;
            case TileState.Waiting:
                WaitingState();
                break;
            case TileState.Used:
                UsingState();
                break;
        }
    }

    public bool TileUsed()
    {
        if (currentTileState != TileState.Used)
            return false;

        return true;
    }

    private void DeselectingState()
    {
        _renderer.material = baseMaterial;
    }

    private void SelectingState()
    {
        _renderer.material = selectedMaterial;
    }
    private void WaitingState()
    {
        _renderer.material = usedMaterial;
    }

    private void UsingState()
    {
        _renderer.material = usedMaterial;
        IsUsed = true;
    }
    #endregion
}