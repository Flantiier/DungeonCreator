using UnityEngine;
using Personnal.Florian;
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
                    PersonnalUtilities.Layers.ShowLayer(groundLayer);
                    PersonnalUtilities.Layers.ShowLayer(wallLayer);
                    break;
				case Tile.TilingType.Ground:
                    PersonnalUtilities.Layers.ShowLayer(groundLayer);
                    break;
				case Tile.TilingType.Wall:
                    PersonnalUtilities.Layers.ShowLayer(wallLayer);
                    break;

            }
		}

		/// <summary>
		/// Disable the ground and wall layer
		/// </summary>
		private void DisableAll()
        {
            PersonnalUtilities.Layers.HideLayer(groundLayer);
            PersonnalUtilities.Layers.HideLayer(wallLayer);
        }
        #endregion
    }
}
