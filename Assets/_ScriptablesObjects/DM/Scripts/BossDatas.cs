using UnityEngine;
using Sirenix.OdinInspector;

namespace _ScriptableObjects.DM
{
    [CreateAssetMenu(fileName = "New Boss Datas", menuName = "Scriptables/DM/Boss")]
    [InlineEditor]
    public class BossDatas : ScriptableObject
    {
        [FoldoutGroup("Movements")]
        [TitleGroup("Movements/Smoothing"), LabelWidth(125)]
        [Range(0f, 0.2f), GUIColor(3, 0.5f, 0.5f)]
        public float inputSmoothing = 0.1f;
        [TitleGroup("Movements/Smoothing"), LabelWidth(125)]
        [Range(0f, 0.2f), GUIColor(0.3f, 2, 0.5f)]
        public float rotationSmoothing = 0.1f;

        [TitleGroup("Movements/Motion"), LabelWidth(125)]
        [Range(2f, 8f), GUIColor(0.3f, 0.8f, 3)]
        public float walkSpeed = 5f;
    }
}