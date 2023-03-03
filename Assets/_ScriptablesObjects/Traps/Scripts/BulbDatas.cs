using UnityEngine;
using Sirenix.OdinInspector;
using _ScriptableObjects.Afflictions;

namespace _ScriptableObjects.Traps
{
    [CreateAssetMenu(fileName = "New Bulbe Datas", menuName = "Traps/Bulbe Properties")]
    [InlineEditor]
    public class BulbDatas : TrapSO
    {
        #region Variables
        [FoldoutGroup("Properties")]
        [BoxGroup("Properties/Stats"), LabelWidth(100), Range(20, 400), GUIColor(0, 2, 0.5f)]
        public int health = 50;
        [BoxGroup("Properties/Stats"), LabelWidth(100), Range(1, 15), GUIColor(1, 2, 3)]
        public float sporesDuration = 5.5f;
        [BoxGroup("Properties/Stats"), LabelWidth(100), Range(0.5f, 6), GUIColor(1, 2, 3)]
        public float waitTime = 2f;
        [BoxGroup("Properties/Stats"), LabelWidth(100)]
        public AfflictionStatus affliction;
        #endregion
    }
}