using UnityEngine;
using Utils;
using _Scripts.TrapSystem;

namespace _Scripts.Characters.DungeonMaster
{
	public class TilingCulling : MonoBehaviour
	{
		#region Variables
		[Header("Properties")]
		[SerializeField] private bool disabledOnStart = false;
		[SerializeField] private string groundLayer = "GroundTiling";
		[SerializeField] private string wallLayer = "WallTiling";
        #endregion

        #region Builts_In
        private void Start()
		{
			if (!disabledOnStart)
				return;

			DisableAll();
		}

		private void OnEnable()
		{
			DMController.Instance.OnSelectedCard += EnableLayers;
			DMController.Instance.OnEndDrag += DisableAll;
        }

		private void OnDisable()
        {
            DMController.Instance.OnSelectedCard -= EnableLayers;
            DMController.Instance.OnEndDrag -= DisableAll;
        }
        #endregion

        #region Methods
		/// <summary>
		/// Enable the culling mask layer based on the given tiling type
		/// </summary>
		/// <param name="type"></param>
		private void EnableLayers(Tile.TilingType type)
		{
			switch (type)
			{
				default:
                    Utils.Utilities.Layers.ShowLayer(groundLayer);
                    Utils.Utilities.Layers.ShowLayer(wallLayer);
                    break;
				case Tile.TilingType.Ground:
                    Utils.Utilities.Layers.ShowLayer(groundLayer);
                    break;
				case Tile.TilingType.Wall:
                    Utils.Utilities.Layers.ShowLayer(wallLayer);
                    break;

            }
		}

		/// <summary>
		/// Disable the ground and wall layer
		/// </summary>
		private void DisableAll()
        {
            Utils.Utilities.Layers.HideLayer(groundLayer);
            Utils.Utilities.Layers.HideLayer(wallLayer);
        }
        #endregion
    }
}
