using UnityEngine;
using Sirenix.OdinInspector;

namespace _ScriptableObjects.Traps
{
    [CreateAssetMenu(fileName = "New Spikes Datas", menuName = "Scriptables/Traps/Spikes Datas")]
    [InlineEditor]
    public class SpikesDatas : TrapSO
    {
        #region Variables
        [FoldoutGroup("Properties")]
        [BoxGroup("Properties/Stats"), LabelWidth(100)]
        [Range(10, 50), GUIColor(2, 0.5f, 0.3f)]
        public int damages = 25;

        [TitleGroup("Properties/Defuse")]
        [BoxGroup("Properties/Stats"), LabelWidth(100)]
        [Range(1, 12), GUIColor(2, 3, 0.3f)]
        public float defuseDuration = 5f;
        #endregion
    }
}