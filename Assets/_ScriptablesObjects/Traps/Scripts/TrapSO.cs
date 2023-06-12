using UnityEngine;
using Sirenix.OdinInspector;
using _Scripts.TrapSystem;

namespace _ScriptableObjects.Traps
{
    public class TrapSO : ScriptableObject
    {
        //INFORMATIONS
        [BoxGroup("Basic Informations"), HorizontalGroup("Basic Informations/G01", 75)]
        [PreviewField(75), HideLabel]
        public Sprite image;
        [BoxGroup("Basic Informations"), VerticalGroup("Basic Informations/G01/G02")]
        [LabelText("Trap Name"), LabelWidth(75)]
        public string trapName = "New trap";
        [BoxGroup("Basic Informations"), VerticalGroup("Basic Informations/G01/G02"), TextArea(3, 3)]
        public string description = "New description";

        //MESHES
        [FoldoutGroup("Gameplay")]
        [BoxGroup("Gameplay/Meshes"), LabelWidth(100)]
        public GameObject trapPrefab;
        [BoxGroup("Gameplay/Meshes"), LabelWidth(100)]
        public GameObject previewPrefab;

        //TILING PROPERTIES
        [BoxGroup("Gameplay/Tiles"), LabelWidth(100), GUIColor(0, 2, 0.5f)]
        [Range(1, 3)] public int xAmount = 1;
        [BoxGroup("Gameplay/Tiles"), LabelWidth(100), GUIColor(0, 2, 0.5f)]
        [Range(1, 3)] public int yAmount = 1;

        //PROPERTIES
        [BoxGroup("Gameplay/Type property"), LabelWidth(100), LabelText("Trap type")]
        public Tile.TilingType type = Tile.TilingType.Ground;
        [BoxGroup("Gameplay/Type property"), LabelWidth(100), Range(0, 50), GUIColor(3, 1, 2)]
        public int manaCost = 25;
        [BoxGroup("Properties"), LabelWidth(100), Range(10, 150), GUIColor(0, 1.5f, 2)]
        public float damages = 25;
    }
}
