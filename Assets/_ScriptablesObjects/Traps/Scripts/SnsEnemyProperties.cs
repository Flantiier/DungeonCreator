using UnityEngine;
using _ScriptableObjects.Traps;
using Sirenix.OdinInspector;

namespace _ScriptableObjects.Traps
{
    [CreateAssetMenu(fileName = "Sns Enemy", menuName = "Traps/Enemies/Sns Enemy"), InlineEditor]
    public class SnsEnemyProperties : EnemyProperties
    {
        [TitleGroup("Enemy"), LabelWidth(125), Range(0f, 1f), GUIColor(1, 2, 3)]
        public float defendRatio = 0.5f;
    }
}