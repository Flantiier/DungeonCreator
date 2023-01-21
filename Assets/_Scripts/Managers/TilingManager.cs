using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using _Scripts.TrapSystem;

namespace _Scripts.Managers
{
	public class TilingManager : MonoBehaviour
	{
		#region Variables
		[TitleGroup("References")]
		[SerializeField] private List<Tile> groundTiles = new List<Tile>();
		[SerializeField] private List<Tile> wallTiles = new List<Tile>();

		public List<Tile> GroundTiles => groundTiles;
		public List<Tile> WallTiles => wallTiles;
		#endregion

		#region Methods
		[ButtonGroup("Buttons", ButtonHeight = 20)]
		[Button("Get Tiles")]
		private void GetTiles()
		{
			groundTiles.Clear();
			wallTiles.Clear();

			Tile[] tiles = FindObjectsOfType<Tile>();

			foreach (Tile tile in tiles)
			{
				if (tile.TileType == Tile.TilingType.Wall)
					wallTiles.Add(tile);
				else if (tile.TileType == Tile.TilingType.Ground)
					groundTiles.Add(tile);
				else
				{
					groundTiles.Add(tile);
					wallTiles.Add(tile);
				}
            }
        }
        #endregion
    }
}
