using UnityEngine;

namespace _Scripts.TrapSystem
{
    public class Tile : MonoBehaviour
    {
        #region Tile Variables
        [Header("Tile Variables")]
        [SerializeField] private Material baseMaterial;
        [SerializeField] private Material selectedMaterial;
        [SerializeField] private Material usedMaterial;
        private Renderer _renderer;

        public enum TileState { Deselected, Selected, Waiting, Used }
        public TileState CurrentTileState { get; private set; }
        public bool IsUsed { get; private set; }
        #endregion

        #region Builts-In
        private void Awake()
        {
            IsUsed = false;
            _renderer = GetComponent<Renderer>();
            NewTileState(TileState.Deselected);
        }
        #endregion

        #region Tile State Methods
        public void NewTileState(TileState newState)
        {
            CurrentTileState = newState;

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

        /// <summary>
        /// Reset the tile to base state
        /// </summary>
        public void ResetTile()
        {
            DeselectingState();
            IsUsed = false;
        }

        /// <summary>
        /// If the tile is occuped by a trap
        /// </summary>
        private void UsingState()
        {
            _renderer.material = usedMaterial;
            IsUsed = true;
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
        #endregion
    }
}