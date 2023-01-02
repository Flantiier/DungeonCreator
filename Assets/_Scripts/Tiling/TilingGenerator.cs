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
        [SerializeField] private int xAmount = 5;
        [BoxGroup("Properties")]
        [SerializeField] private int yAmount = 5;
        [BoxGroup("Properties")]
        [SerializeField] private float yOffset = .01f;

        #endregion

        #region Tiling Creation Methods
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

            //Loops to create tiling
            for (int i = 0; i < xAmount; i++)
            {
                for (int j = 0; j < yAmount; j++)
                {
                    GameObject tile = Instantiate(tiling.tilePrefab, transform);
                    Vector3 _tiling = new Vector3(tiling.lengthX * i, yOffset, tiling.lengthY * j);
                    tile.transform.position = transform.right * _tiling.x + transform.up * _tiling.y + transform.forward * _tiling.z;
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
        #endregion
    }
}
