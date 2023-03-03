using UnityEngine;
using Sirenix.OdinInspector;

namespace _ScriptableObjects.Traps
{
    [CreateAssetMenu(fileName = "New Spikes Datas", menuName = "Scriptables/Traps/Spikes Datas")]
    [InlineEditor]
    public class PikesDatas : TrapSO
    {
        #region Variables
        [FoldoutGroup("Properties")]
        [BoxGroup("Properties/Stats"), LabelWidth(100), Range(10, 150), GUIColor(0, 1.5f, 2)]
        public int damages = 25;
        [TitleGroup("Properties/Defuse")]
        [BoxGroup("Properties/Stats"), LabelWidth(100), Range(1, 12), GUIColor(1, 2, 3)]
        public float defuseDuration = 5f;
        #endregion
    }
}