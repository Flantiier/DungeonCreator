using UnityEngine;
using Sirenix.OdinInspector;

namespace _ScriptableObjects.Traps
{
    [CreateAssetMenu(fileName = "New Pikes Properties", menuName = "Traps/Pikes Properties"), InlineEditor]
    public class PikesProperties : TrapSO
    {
        #region Variables
        [BoxGroup("Properties"), LabelWidth(100), Range(1, 12), GUIColor(1, 2, 3)]
        public float defuseDuration = 5f;
        #endregion
    }
}