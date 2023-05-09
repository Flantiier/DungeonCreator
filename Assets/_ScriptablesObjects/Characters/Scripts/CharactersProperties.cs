using UnityEngine;
using Sirenix.OdinInspector;

namespace _ScriptableObjects.Characters
{
    [CreateAssetMenu(fileName = "New Character Properties", menuName = "SO/Characters/Characters Properties"), InlineEditor]
    public class CharactersProperties : ScriptableObject
    {
        #region Variables
        [BoxGroup("Movements")]
        [TitleGroup("Movements/Smoothing"), LabelWidth(125), Range(0f, 0.2f), GUIColor(2, 1, 0)]
        public float inputSmoothing = 0.1f;
        [TitleGroup("Movements/Smoothing"), LabelWidth(125), Range(0f, 0.2f), GUIColor(2, 1, 0)]
        public float speedSmoothing = 0.15f;
        [TitleGroup("Movements/Smoothing"), LabelWidth(125), Range(0f, 0.2f), GUIColor(2, 1, 0)]
        public float rotationSmoothing = 0.1f;

        [TitleGroup("Movements/Motion"), LabelWidth(125), Range(2f, 8f), GUIColor(0, 1.5f, 2)]
        public float walkSpeed = 5f;
        [TitleGroup("Movements/Motion"), LabelWidth(125), Range(5f, 15f), GUIColor(0, 1.5f, 2)]
        public float runSpeed = 8f;
        [TitleGroup("Movements/Motion"), LabelWidth(125), Range(0.3f, 3f), GUIColor(0, 1.5f, 2)]
        public float aimSpeed = 1f;
        [TitleGroup("Movements/Motion"), LabelWidth(125), Range(5f, 15f), GUIColor(0, 1.5f, 2)]
        public float dodgeSpeed = 10f;
        [TitleGroup("Movements/Motion"), LabelWidth(125)]
        public AnimationCurve dodgeCurve;

        [BoxGroup("Stats"), TitleGroup("Stats/Health"), LabelWidth(125), Range(1, 20), GUIColor(0, 2, 0.5f)]
        public float healthRecup = 7f;
        [BoxGroup("Stats"), TitleGroup("Stats/Health"), LabelWidth(125), Range(1, 20), GUIColor(0, 2, 0.5f)]
        public float healthRecupDelay = 3f;
        [BoxGroup("Stats"), TitleGroup("Stats/Stamina"), LabelWidth(125), Range(1, 20), GUIColor(3, 2, 0.5f)]
        public float staminaRecup = 5f;
        [BoxGroup("Stats"), TitleGroup("Stats/Stamina"), LabelWidth(125), Range(0.1f, 3f), GUIColor(3, 2, 0.5f)]
        public float staminaToRun = 1f;
        [BoxGroup("Stats"), TitleGroup("Stats/Stamina"), LabelWidth(125), Range(1, 100), GUIColor(3, 2, 0.5f)]
        public float staminaToDodge = 20f;
        #endregion
    }
}
