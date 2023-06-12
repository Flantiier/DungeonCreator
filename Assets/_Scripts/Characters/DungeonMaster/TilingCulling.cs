using UnityEngine;
using Sirenix.OdinInspector;
using _Scripts.TrapSystem;
using static Utils.Utilities.Layers;

namespace _Scripts.Characters.DungeonMaster
{
    public class TilingCulling : MonoBehaviour
    {
        #region Variables
        [TitleGroup("Layers")]
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
        #endregion

        #region Methods
        /// <summary>
        /// Enable the culling mask layer based on the given tiling type
        /// </summary>
        public void EnableLayers()
        {
            Tile.TilingType type = DMController.SelectedCard.TrapReference.type;

            switch (type)
            {
                default:
                    ShowLayer(groundLayer);
                    ShowLayer(wallLayer);
                    break;
                case Tile.TilingType.Ground:
                    ShowLayer(groundLayer);
                    break;
                case Tile.TilingType.Wall:
                    ShowLayer(wallLayer);
                    break;

            }
        }

        /// <summary>
        /// Disable the ground and wall layer
        /// </summary>
        public void DisableAll()
        {
            HideLayer(groundLayer);
            HideLayer(wallLayer);
        }
        #endregion
    }
}
