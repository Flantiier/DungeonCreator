using UnityEngine;

namespace Tiling
{
    public class TilingGenerator : MonoBehaviour
    {
        #region Tiling Variables
        [Header("Tiling Variables")]
        [SerializeField, Tooltip("Tiling Informations")]
        private TilingSO tiling;

        [SerializeField, Tooltip("Resetting or not the position at 0")]
        private bool resetAt0;

        [SerializeField, Tooltip("Tiles amount on X Axis")]
        private int xAmount;

        [SerializeField, Tooltip("Tiles amount on Z Axis")]
        private int yAmount;

        [SerializeField, Tooltip("Position offset on the World Y Axis")]
        private float yOffset;
        #endregion

        #region Tiling Creation Methods
        /// <summary>
        /// Creating a tiling of x and y amount of tiles
        /// </summary>
        public void CreateTiling()
        {
            if(resetAt0)
                //Resetting parent position
                transform.position = Vector3.zero;

            //Resetting tiling
            Transform[] childs = transform.GetComponentsInChildren<Transform>();

            //Destroy each tile
            if (childs.Length > 1)
                for (int i = 1; i < childs.Length; i++)
                    DestroyImmediate(childs[i].gameObject);

            //Create a tiling of YAmount on XAmount
            for (int i = 0; i < xAmount; i++)
            {
                for (int j = 0; j < yAmount; j++)
                {
                    GameObject newTile = Instantiate(tiling.tilePrefab, transform);
                    newTile.transform.position = transform.position + new Vector3((tiling.tilePrefab.transform.localScale.x + tiling.xOffset) * i, yOffset,
                                                        (tiling.tilePrefab.transform.localScale.y + tiling.yOffset) * j);
                }
            }
        }
        #endregion
    }
}
