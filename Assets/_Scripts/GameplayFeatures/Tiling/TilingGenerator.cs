using UnityEngine;
using UnityEditor;
using Sirenix.OdinInspector;

namespace _Scripts.TrapSystem
{
    public class TilingGenerator : MonoBehaviour
    {
        #region Tiling Variables
        [BoxGroup("Tiling")]
        [Required, SerializeField] private TilingSO tiling;

        [BoxGroup("Properties")]
        [SerializeField, Min(1f)] private int xAmount = 5;
        [BoxGroup("Properties")]
        [SerializeField, Min(1f)] private int yAmount = 5;
        [BoxGroup("Properties")]
        [SerializeField, Min(0.01f)] private float yOffset = .01f;
        [BoxGroup("Properties")]
        [SerializeField] private Tile.TilingType type;
        [BoxGroup("Properties")]
        [SerializeField] private string groundLayer = "GroundTiling";
        [BoxGroup("Properties")]
        [SerializeField] private string wallLayer = "WallTiling";

        #endregion

        #region Tiling Creation Methods

#if UNITY_EDITOR
        /// <summary>
        /// Creating tiling
        /// </summary>
        [BoxGroup("Interactives")]
        [Button("Create Tiling", ButtonSizes.Medium)]
        private void CreateTiling()
        {
            if (!tiling)
                return;

            //Destroying children
            Transform[] children = transform.GetComponentsInChildren<Transform>();

            if (children != null)
            {
                for (int i = 1; i < children.Length; i++)
                    DestroyImmediate(children[i].gameObject);
            }

            //Reset parent rotation
            Quaternion baseRot = transform.rotation;
            transform.rotation = Quaternion.identity;

            //Get the middle of the grid                                        //Minus the half of the tile length
            Vector3 gridX = transform.right * ((xAmount / 2f * tiling.lengthX)) - transform.right * tiling.lengthX / 2f;
            Vector3 gridY = transform.forward * ((yAmount / 2f * tiling.lengthY)) - transform.forward * tiling.lengthY / 2f;
            Vector3 startPos = transform.position - (gridX + gridY);

            //Loops to create tiling
            for (int i = 0; i < xAmount; i++)
            {
                for (int j = 0; j < yAmount; j++)
                {
                    GameObject tile = PrefabUtility.InstantiatePrefab(tiling.tilePrefab, transform) as GameObject;
                    Vector3 _tiling = new Vector3(tiling.lengthX * i, yOffset, tiling.lengthY * j);

                    //Set position/rotations
                    tile.transform.position = startPos + transform.right * _tiling.x + transform.up * _tiling.y + transform.forward * _tiling.z;
                    tile.transform.rotation = Quaternion.identity;

                    //Set layer and type
                    tile.layer = type == Tile.TilingType.Ground ? LayerMask.NameToLayer(groundLayer) : LayerMask.NameToLayer(wallLayer);
                    tile.GetComponent<Tile>().TileType = type;
                }
            }

            //Return parent rotation
            transform.rotation = baseRot;
        }

        //GENERATEUR DE PANNEAUX SOLAIRES
        /*for (int i = 0; i < xAmount; i++)
        {
            for (int j = 0; j < yAmount; j++)
            {
                GameObject newTile = Instantiate(tiling.tilePrefab, transform);
                newTile.transform.position = transform.position + new Vector3(tiling.lengthX * i, yOffset, tiling.lengthY * j);
            }
        }*/
#endif

        #endregion
    }
}
