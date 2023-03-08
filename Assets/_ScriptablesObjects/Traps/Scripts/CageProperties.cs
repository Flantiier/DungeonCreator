using UnityEngine;
using Sirenix.OdinInspector;

namespace _ScriptableObjects.Traps
{
    [CreateAssetMenu(fileName = "New Cage Properties", menuName = "Traps/Cage Properties"), InlineEditor]
    public class CageProperties : TrapSO
	{
        #region Variables
        [FoldoutGroup("Properties"), LabelWidth(100), Range(20, 400), GUIColor(0, 2, 0.5f)]
        public int health = 50;
        #endregion
    }
}
