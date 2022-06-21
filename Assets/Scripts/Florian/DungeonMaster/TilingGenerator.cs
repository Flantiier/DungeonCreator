using UnityEngine;

namespace Tiling
{
    public class TilingGenerator : MonoBehaviour
    {
        #region Tiling Variables
        [Header("Tiling Variables")]
        [SerializeField] private TileGroup tiling;
        #endregion

        public void CreateTiling()
        {
            ResetTiling();

            for (int i = 0; i < tiling.xAmount; i++)
            {
                for (int j = 0; j < tiling.yAmount; j++)
                {
                    InstantiateNewTile(i, j);
                }
            }
        }

        private void InstantiateNewTile(int x, int y)
        {
            //Tile newTile = Instantiate
            GameObject newTile = Instantiate(tiling.tilePrefab, transform);
            newTile.transform.position = transform.position + new Vector3((x * tiling.tilePrefab.transform.localScale.x) 
                                            + (x * tiling.xOffset), 0f, (y * tiling.tilePrefab.transform.localScale.y) + (y * tiling.yOffset));
        }

        public void ResetTiling()
        {
            Transform[] childs = transform.GetComponentsInChildren<Transform>();

            if (childs.Length <= 1)
                return;

            for (int i = 1; i < childs.Length; i++)
            {
                DestroyImmediate(childs[i].gameObject);
            }
        }
    }

    [System.Serializable]
    public struct TileGroup
    {
        public GameObject tilePrefab;

        public int xAmount;
        public int yAmount;
        public float xOffset;
        public float yOffset;
    }
}
