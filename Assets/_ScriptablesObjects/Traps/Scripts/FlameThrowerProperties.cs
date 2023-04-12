using UnityEngine;
using Sirenix.OdinInspector;

namespace _ScriptableObjects.Traps
{
    [CreateAssetMenu(fileName = "New FlameThrower Properties", menuName = "Traps/FlameThrower Properties"), InlineEditor]
    public class FlameThrowerProperties : TrapSO
    {
        #region Variables
        [BoxGroup("Properties"), LabelWidth(100), Range(3, 15), GUIColor(1, 2, 3)]
        public float sprayDuration = 5f;
        [BoxGroup("Properties"), LabelWidth(100), Range(3, 15), GUIColor(1, 2, 3)]
        public float waitTime = 5f;
        [BoxGroup("Properties"), LabelWidth(100), Range(1, 5), GUIColor(1, 2, 3)]
        public float sprayDistance = 2.5f;
        [BoxGroup("Properties"), LabelWidth(100), Range(0.1f, 5), GUIColor(1, 2, 3)]
        public float sprayRadius = 0.5f;
        #endregion
    }
}