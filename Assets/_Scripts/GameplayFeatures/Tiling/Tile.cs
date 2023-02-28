using UnityEngine;

namespace _Scripts.TrapSystem
{
    public class Tile : MonoBehaviour
    {
        #region Tile Variables
        public enum TilingType { Ground, Wall, Both }

        [Header("Tile Variables")]
        [SerializeField] private Material baseMaterial;
        [SerializeField] private Material selectedMaterial;
        [SerializeField] private Material usedMaterial;
        [SerializeField] private TilingType tileType;

        private Renderer _renderer;
        #endregion

        #region Properties
        public enum TileState { Free, Selected, Waiting, Used }
        public TileState CurrentTileState { get; set; }
        public TilingType TileType
        {
            get => tileType;
            set { tileType = value; }
        }
        #endregion

        #region Builts-In
        private void Awake()
        {
            _renderer = GetComponent<Renderer>();
            NewTileState(TileState.Free);
        }
        #endregion

        #region Tile State Methods
        public void NewTileState(TileState newState)
        {
            CurrentTileState = newState;

            switch (newState)
            {
                case TileState.Selected:
                    SetTileMaterial(selectedMaterial);
                    break;
                case TileState.Used:
                    SetTileMaterial(usedMaterial);
                    break;
                case TileState.Waiting:
                    SetTileMaterial(usedMaterial);
                    break;
                default:
                    SetTileMaterial(baseMaterial);
                    break;
            }
        }

        /// <summary>
        /// Return if the tile is in used state
        /// </summary>
        /// <returns></returns>
        public bool IsUsed()
        {
            return CurrentTileState == TileState.Used;
        }

        [ContextMenu("Use Tile")]
        private void UseTile()
        {
            NewTileState(TileState.Used);
        }

        /// <summary>
        /// Set the material to the given one
        /// </summary>
        private void SetTileMaterial(Material mat)
        {
            _renderer.material = mat;
        }
        #endregion
    }
}