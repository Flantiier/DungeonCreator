using System.Collections.Generic;
using UnityEngine;
using _Scripts.TrapSystem;
using _Scripts.Characters.DungeonMaster;

namespace _Scripts.GameplayFeatures
{
    [RequireComponent(typeof(BoxCollider))]
    [RequireComponent(typeof(Rigidbody))]
    public class TilingInteractor : MonoBehaviour
    {
        #region Variables/Properties
        private List<Tile> _tiles = new List<Tile>();
        private BoxCollider _collider;

        public int Amount { get; set; }
        public List<Tile> Tiles => _tiles;
        #endregion

        #region Builts_In
        private void Awake()
        {
            _collider = GetComponent<BoxCollider>();
            DisableCollider();
        }

        private void OnEnable()
        {
            DMController.Instance.OnStartDrag += EnableCollider;
            DMController.Instance.OnEndDrag += DisableCollider;
        }

        private void OnDisable()
        {
            DMController.Instance.OnStartDrag -= EnableCollider;
            DMController.Instance.OnEndDrag -= DisableCollider;
        }

        private void LateUpdate()
        {
            if (!_collider.enabled)
                return;

            RefreshTiling();
        }

        #region Trigger collisions
        private void OnTriggerEnter(Collider other)
        {
            if (!other.TryGetComponent(out Tile tile))
                return;

            if (tile.transform.up != transform.up)
            {
                Debug.Log("Not oriented correctly");
                return;
            }

            AddToTilesList(tile);
        }

        private void OnTriggerExit(Collider other)
        {
            if (!other.TryGetComponent(out Tile tile))
                return;

            RemoveFromTilesList(tile);
        }
        #endregion

        #endregion

        #region Methods
        /// <summary>
        /// Checks if there are such tiles in the tiles list
        /// </summary>
        public bool EnoughTilesAmount()
        {
            return Tiles.Count >= Amount;
        }

        /// <summary>
        /// Indicates if all the tiles detected are free
        /// </summary>
        public bool IsTilingFree()
        {
            foreach (Tile tile in Tiles)
            {
                if (tile.CurrentTileState != Tile.TileState.Used)
                    continue;

                return false;
            }

            return true;
        }

        #region Collider Methods
        /// <summary>
        /// Set the box collider bounds on X and Z
        /// </summary>
        /// <param name="sizeX"> size on X </param>
        /// <param name="sizeZ"> size on Z </param>
        public void SetColliderSize(float sizeX, float sizeZ)
        {
            if (!_collider)
                return;

            _collider.size = new Vector3(sizeX, _collider.size.y, sizeZ);
        }

        /// <summary>
        /// Enable the collider
        /// </summary>
        private void EnableCollider()
        {
            if (!_collider)
                return;

            _collider.enabled = true;
        }

        /// <summary>
        /// Disable the collider
        /// </summary>
        private void DisableCollider()
        {
            if (!_collider)
                return;

            _collider.enabled = false;
        }
        #endregion

        #region Tiles Interactions
        /// <summary>
        /// Add a tile to the list
        /// </summary>
        private void AddToTilesList(Tile tile)
        {
            if (Tiles.Contains(tile))
                return;

            Tiles.Add(tile);
        }

        /// <summary>
        /// Remove a tile from the list
        /// </summary>
        private void RemoveFromTilesList(Tile tile)
        {
            if (!Tiles.Contains(tile))
                return;

            SetTileNewState(tile, Tile.TileState.Free);
            Tiles.Remove(tile);
        }

        /// <summary>
        /// Refresh all tiles in the list
        /// </summary>
        public void RefreshTiling()
        {
            if (!EnoughTilesAmount() || !IsTilingFree())
                SetAllTiles(Tile.TileState.Waiting);
            else
                SetAllTiles(Tile.TileState.Selected);
        }

        /// <summary>
        /// Set the tile state if its not already used
        /// </summary>
        /// <param name="tile"> Tile to set state </param>
        /// <param name="state"> New tile state </param>
        private void SetTileNewState(Tile tile, Tile.TileState state)
        {
            if (tile.IsUsed())
                return;

            tile.NewTileState(state);
        }

        /// <summary>
        /// Set the state of all the states currently in the list
        /// </summary>
        /// <param name="state"> New states </param>
        public void SetAllTiles(Tile.TileState state)
        {
            foreach (Tile tile in Tiles)
            {
                if (!tile || tile.IsUsed())
                    continue;

                tile.NewTileState(state);
            }
        }

        /// <summary>
        /// Reset completely the tiles list
        /// </summary>
        public void RefreshTilesList()
        {
            SetAllTiles(Tile.TileState.Free);
            Tiles.Clear();
        }

        /// <summary>
        /// Set all available tiles to free and clear the tiles list
        /// </summary>
        public void RefreshInteractor()
        {
            _collider.enabled = false;
            RefreshTilesList();
            _collider.enabled = true;
        }
        #endregion

        #endregion
    }
}
