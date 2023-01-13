using UnityEngine;

#region TrapSO
namespace _Scripts.TrapSystem.Datas
{
    [CreateAssetMenu(fileName = "New Trap", menuName = "Scriptables/Trap")]
    public class TrapSO : ScriptableObject
    {
        [Header("Mesh properties")]
        public GameObject trapPrefab;
        public Material defaultMaterial;
        public Material previewMaterial;

        [Header("Trap informations")]
        public string trapName = "New trap";
        public float damage;
        public int manaCost;
        public bool isStaminaLow;

        [TextArea(10, 10)] public string description = "New description";
        [Range(1, 10)] public int xAmount = 5;
        [Range(1, 10)] public int yAmount = 5;

        /// <summary>
        /// Get the preview material
        /// </summary>
        /// <returns></returns>
        public Material GetPreviewMaterial()
        {
            return GetMaterial(previewMaterial);
        }

        /// <summary>
        /// Get the preview material
        /// </summary>
        /// <returns></returns>
        public Material GetDefaultMaterial()
        {
            return GetMaterial(defaultMaterial);
        }

        /// <summary>
        /// Return the given material or a new instance if reference is missing
        /// </summary>
        /// <param name="mat"> Returned material </param>
        private Material GetMaterial(Material mat)
        {
            if (!mat)
            {
                Material instance = new Material(Shader.Find("Universal Render Pipeline/Lit"));
                instance.color = Color.red;
                return instance;
            }

            return mat;
        }
    }
}
#endregion
