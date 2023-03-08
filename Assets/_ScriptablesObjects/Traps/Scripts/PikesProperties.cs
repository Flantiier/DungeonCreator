using UnityEngine;
using Sirenix.OdinInspector;

namespace _ScriptableObjects.Traps
{
    [CreateAssetMenu(fileName = "New Pikes Properties", menuName = "Traps/Pikes Properties"), InlineEditor]
    public class PikesProperties : TrapSO
    {
        #region Variables
        [FoldoutGroup("Properties"), LabelWidth(100), Range(10, 150), GUIColor(0, 1.5f, 2)]
        public int damages = 25;
        [FoldoutGroup("Properties"), LabelWidth(100), Range(1, 12), GUIColor(1, 2, 3)]
        public float defuseDuration = 5f;
        #endregion
    }
}