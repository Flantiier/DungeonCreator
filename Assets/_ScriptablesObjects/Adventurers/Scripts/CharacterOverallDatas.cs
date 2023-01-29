using UnityEngine;
using Sirenix.OdinInspector;

namespace _ScriptableObjects.Characters
{
    [CreateAssetMenu(fileName = "New Character Datas", menuName = "Scriptables/Characters/Character Datas")]
    [InlineEditor]
    public class CharacterOverallDatas : ScriptableObject
    {
        #region Variables
        [BoxGroup("Movements")]
        [TitleGroup("Movements/Smoothing"), LabelWidth(125)]
        [Range(0f, 0.2f), GUIColor(3, 0.5f, 0.5f)]
        public float inputSmoothing = 0.1f;
        [TitleGroup("Movements/Smoothing"), LabelWidth(125)]
        [Range(0f, 0.2f), GUIColor(2, 2, 0.5f)]
        public float speedSmoothing = 0.15f;
        [TitleGroup("Movements/Smoothing"), LabelWidth(125)]
        [Range(0f, 0.2f), GUIColor(0.3f, 2, 0.5f)]
        public float rotationSmoothing = 0.1f;

        [TitleGroup("Movements/Motion"), LabelWidth(125)]
        [Range(2f, 8f), GUIColor(0.3f, 0.8f, 3)]
        public float walkSpeed = 5f;
        [TitleGroup("Movements/Motion"), LabelWidth(125)]
        [Range(5f, 15f), GUIColor(1.5f, 0.5f, 3)]
        public float runSpeed = 8f;
        [TitleGroup("Movements/Motion"), LabelWidth(125)]
        [Range(0.3f, 3f), GUIColor(0.3f, 3, 1)]
        public float aimSpeed = 1f;
        [TitleGroup("Movements/Motion"), LabelWidth(125)]
        [Range(5f, 15f), GUIColor(3, 0.8f, 0.3f)]
        public float dodgeSpeed = 10f;
        [TitleGroup("Movements/Motion"), LabelWidth(125)]
        public AnimationCurve dodgeCurve;

        [BoxGroup("Stats"), TitleGroup("Stats/Health"), LabelWidth(125)]
        [Range(4, 12), GUIColor(0.8f, 3, 0.8f)]
        public float healthRecup = 7f;
        [BoxGroup("Stats"), TitleGroup("Stats/Health"), LabelWidth(125)]
        [Range(1, 6), GUIColor(0.3f, 3, 1.5f)]
        public float healthRecupDelay = 3f;
        [BoxGroup("Stats"), TitleGroup("Stats/Stamina"), LabelWidth(125)]
        [Range(2f, 10f), GUIColor(3, 2, 0.5f)]
        public float staminaRecup = 5f;
        [BoxGroup("Stats"), TitleGroup("Stats/Stamina"), LabelWidth(125)]
        [Range(0.1f, 3f), GUIColor(2, 2, 0.8f)]
        public float staminaToRun = 1f;
        [BoxGroup("Stats"), TitleGroup("Stats/Stamina"), LabelWidth(125)]
        [Range(10f, 30f), GUIColor(2, 1, 0.8f)]
        public float staminaToDodge = 20f;
        #endregion
    }
}
