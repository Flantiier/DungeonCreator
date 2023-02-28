using UnityEngine;
using Sirenix.OdinInspector;

namespace _ScriptableObjects.Traps
{
    [CreateAssetMenu(fileName = "New FlameThrower Datas", menuName = "Scriptables/Traps/FlameThrower Datas")]
    [InlineEditor]
    public class FlameThrowerDatas : TrapSO
    {
        #region Variables
        [FoldoutGroup("Properties"), LabelWidth(100)]
        [Range(10, 50), GUIColor(2, 0.5f, 0.3f)]
        public int damages = 10;
        [BoxGroup("Properties/Stats"), LabelWidth(100)]
        [Range(3f, 10f), GUIColor(3, 3, 0.5f)]
        public float sprayDuration = 5f;
        [BoxGroup("Properties/Stats"), LabelWidth(100)]
        [Range(3f, 10f), GUIColor(0.5f, 2, 1)]
        public float waitTime = 5f;
        [BoxGroup("Properties/Stats"), LabelWidth(100)]
        [Range(1f, 5f), GUIColor(3, 3, 1)]
        public float sprayDistance = 2.5f;
        [BoxGroup("Properties/Stats"), LabelWidth(100)]
        [Range(0.1f, 3f), GUIColor(0.5f, 3, 3)]
        public float sprayRadius = 0.5f;
        #endregion
    }
}