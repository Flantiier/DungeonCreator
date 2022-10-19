using UnityEngine;

namespace _Scripts.TrapSystem
{
    public class TilingGenerator : MonoBehaviour
    {
#if UNITY_EDITOR
        #region Tiling Variables
        [Header("Tiling Variables")]
        [SerializeField] private TilingSO tiling;
        [SerializeField] private bool resetPosition = true;
        [SerializeField] private int xAmount = 5;
        [SerializeField] private int yAmount = 5;
        [SerializeField] private float yOffset = 0.1f;
        #endregion

        #region Tiling Creation Methods
        /// <summary>
        /// Creating tiling
        /// </summary>
        public void CreateTiling()
        {
            if (resetPosition)
                transform.localPosition = Vector3.zero;

            Transform[] childs = transform.GetComponentsInChildren<Transform>();

            if (childs != null)
            {
                for (int i = 1; i < childs.Length; i++)
                    DestroyImmediate(childs[i].gameObject);
            }

            for (int i = 0; i < xAmount; i++)
            {
                for (int j = 0; j < yAmount; j++)
                {
                    GameObject newTile = Instantiate(tiling.tilePrefab, transform);
                    newTile.transform.position = transform.position + new Vector3(tiling.lengthX * i, yOffset, tiling.lengthY * j);
                }
            }
        }
        #endregion
#endif
    }
}
