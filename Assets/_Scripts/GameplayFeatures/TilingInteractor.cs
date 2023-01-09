using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using _Scripts.TrapSystem;

namespace _Scripts.GameplayFeatures
{
    [RequireComponent(typeof(BoxCollider))]
    [RequireComponent(typeof(Rigidbody))]
    public class TilingInteractor : MonoBehaviour
    {
        #region Variables
        [ShowInInspector] private List<Tile> _tiles = new List<Tile>();
        private BoxCollider _collider;

        public List<Tile> Tiles => _tiles;
        #endregion

        #region Builts_In
        private void Awake()
        {
            _collider = GetComponent<BoxCollider>();
        }

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

        #region Methods
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

        #region Tiles Interactions
        /// <summary>
        /// Add a tile to the list
        /// </summary>
        private void AddToTilesList(Tile tile)
        {
            if (Tiles.Contains(tile))
                return;

            Tiles.Add(tile);
            SetTileNewState(tile, Tile.TileState.Selected);
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
        /// Set all available tiles to free and clear the tiles list
        /// </summary>
        public void RefreshInteractor()
        {
            _collider.enabled = false;

            SetAllTiles(Tile.TileState.Free);
            Tiles.Clear();

            _collider.enabled = true;
        }
        #endregion

        #endregion
    }
}
