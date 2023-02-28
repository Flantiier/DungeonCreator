using UnityEngine;
using Sirenix.OdinInspector;

namespace _ScriptableObjects.Traps
{
    [CreateAssetMenu(fileName = "New Cage Datas", menuName = "Scriptables/Traps/Cage Datas")]
    [InlineEditor]
    public class CageDatas : TrapSO
	{
        #region Variables
        [FoldoutGroup("Properties")]
        [BoxGroup("Properties/Stats"), LabelWidth(100)]
        [Range(20, 150), GUIColor(1, 3, 1)]
        public int health = 50;
        #endregion
    }
}
