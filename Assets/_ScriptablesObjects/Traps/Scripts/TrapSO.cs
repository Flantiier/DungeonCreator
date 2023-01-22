using UnityEngine;
using Sirenix.OdinInspector;
using _Scripts.TrapSystem;

namespace _ScriptablesObjects.Traps
{
    [CreateAssetMenu(fileName = "New Default Trap", menuName = "Scriptables/Traps/Default")]
    public class TrapSO : ScriptableObject
    {
        #region Variables
        //INFORMATIONS
        [TitleGroup("Informations")]
        public Sprite image;
        [TitleGroup("Informations")]
        public string trapName = "New trap";
        [TitleGroup("Informations")]
        [TextArea(3, 3)] public string description = "New description";

        //MESH AND MATERIALS
        [TitleGroup("Mesh properties")]
        public GameObject trapPrefab;
        [TitleGroup("Mesh properties")]
        public GameObject previewPrefab;

        //PROPERTIES
        [TitleGroup("Tiles")]
        [Range(1, 3)] public int xAmount = 1;
        [TitleGroup("Tiles")]
        [Range(1, 3)] public int yAmount = 1;

        [TitleGroup("Properties")]
        public Tile.TilingType type;
        [TitleGroup("Properties")]
        public float manaCost = 10f;
        [TitleGroup("Default Properties")]
        public float damages = 30f;
        #endregion
    }
}
