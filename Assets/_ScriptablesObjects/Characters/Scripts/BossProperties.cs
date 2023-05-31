using UnityEngine;
using Sirenix.OdinInspector;

namespace _ScriptableObjects.Characters
{
    [CreateAssetMenu(fileName = "New Boss Properties", menuName = "SO/Characters/Boss Properties"), InlineEditor]
    public class BossProperties : ScriptableObject
    {
        [TitleGroup("Smoothing"), LabelWidth(125), Range(0, 0.2f), GUIColor(2, 1, 0)]
        public float inputSmoothing = 0.1f;
        [TitleGroup("Smoothing"), LabelWidth(125), Range(0, 0.2f), GUIColor(2, 1, 0)]
        public float rotationSmoothing = 0.1f;
        [TitleGroup("Motion"), LabelWidth(125), Range(1, 20), GUIColor(0, 1.5f, 2)]
        public float walkSpeed = 5;

        [TitleGroup("Skills"), Range(100, 5000), GUIColor(0, 2, 0.5f)]
        public float health = 500f;
        [TitleGroup("Skills"), Range(5, 60), GUIColor(0, 2, 0.5f)]
        public float firstAbilityRecovery = 15f;
        [TitleGroup("Skills"), Range(5, 60), GUIColor(0, 2, 0.5f)]
        public float secondAbilityRecovery = 15f;
    }
}