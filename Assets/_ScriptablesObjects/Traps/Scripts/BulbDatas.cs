using UnityEngine;
using Sirenix.OdinInspector;
using _ScriptableObjects.Afflictions;

namespace _ScriptableObjects.Traps
{
    [CreateAssetMenu(fileName = "New Bulbe Datas", menuName = "Scriptables/Traps/Bulbe Datas")]
    [InlineEditor]
    public class BulbDatas : TrapSO
    {
        #region Variables
        [FoldoutGroup("Properties")]
        [BoxGroup("Properties/Stats"), LabelWidth(100)]
        [Range(20, 150), GUIColor(1, 3, 1)]
        public int health = 50;
        [BoxGroup("Properties/Stats"), LabelWidth(100)]
        [Range(2f, 8f), GUIColor(3, 1, 0.3f)]
        public float sporesDuration = 5.5f;
        [BoxGroup("Properties/Stats"), LabelWidth(100)]
        [Range(0.5f, 5f), GUIColor(3, 3, 2)]
        public float waitTime = 2f;
        [BoxGroup("Properties/Stats"), LabelWidth(100)]
        public AfflictionStatus affliction;
        #endregion
    }
}