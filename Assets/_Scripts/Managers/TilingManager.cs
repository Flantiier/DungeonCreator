using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using _Scripts.TrapSystem;
using _Scripts.Characters.DungeonMaster;
using System.Xml.Schema;

namespace _Scripts.Managers
{
    public class TilingManager : MonoBehaviour
    {
        #region Variables
        [TitleGroup("References")]
        [SerializeField] private Tile[] groundTiles;
        [SerializeField] private Tile[] wallTiles;
        private Tile.TilingType _lastTileType;

        public Tile[] GroundTiles => groundTiles;
        public Tile[] WallTiles => wallTiles;
        #endregion

        #region Builts_In
        private void Awake()
        {
            EnableAll(false);
        }

        private void OnEnable()
        {
            DMController_Test.Instance.OnSelectedCard += EnableTiles;
            DMController_Test.Instance.OnEndDrag += DisablePreviousTiles;

        }

        private void OnDisable()
        {
            DMController_Test.Instance.OnSelectedCard -= EnableTiles;
            DMController_Test.Instance.OnEndDrag -= DisablePreviousTiles;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Show the tiles based on the selected trap type
        /// </summary>
        /// <param name="type"> Tiling type of the trap </param>
        private void EnableTiles(Tile.TilingType type)
        {
            _lastTileType = type;

            switch (type)
            {
                default:
                    EnableAll(true);
                    break;

                case Tile.TilingType.Ground:
                    LoopThroughList(groundTiles, true);
                    break;

                case Tile.TilingType.Wall:
                    LoopThroughList(WallTiles, true);
                    break;
            }
        }

        /// <summary>
        /// Disable the tiles previously visible
        /// </summary>
        private void DisablePreviousTiles()
        {
            switch (_lastTileType)
            {
                default:
                    EnableAll(false);
                    break;

                case Tile.TilingType.Ground:
                    LoopThroughList(groundTiles, false);
                    break;

                case Tile.TilingType.Wall:
                    LoopThroughList(WallTiles, false);
                    break;
            }
        }

        /// <summary>
        /// Disable all tiles
        /// </summary>
        /// <param name="enabled"> Enabled or disabled </param>
        private void EnableAll(bool enabled)
        {
            LoopThroughList(groundTiles, enabled);
            LoopThroughList(wallTiles, enabled);
        }

        /// <summary>
        /// Loop through a list to enable all the tiles into
        /// </summary>
        /// <param name="list"> List to loop through </param>
        /// <param name="enabled"> Enabled or disabled </param>
        private void LoopThroughList(Tile[] list, bool enabled)
        {
            foreach (Tile tile in list)
            {
                if (!tile)
                    continue;

                tile.gameObject.SetActive(enabled);
            }
        }

        [ButtonGroup("Buttons", ButtonHeight = 20)]
        [Button("Get Tiles")]
        private void GetTiles()
        {
            Tile[] tiles = FindObjectsOfType<Tile>();
            List<Tile> temp = new List<Tile>();
            List<Tile> temp2 = new List<Tile>();

            foreach (Tile tile in tiles)
            {
                if (tile.TileType == Tile.TilingType.Ground)
                    temp.Add(tile);
                else
                    temp2.Add(tile);
            }

            groundTiles = temp.ToArray();
            wallTiles = temp2.ToArray();
        }
        #endregion
    }
}
