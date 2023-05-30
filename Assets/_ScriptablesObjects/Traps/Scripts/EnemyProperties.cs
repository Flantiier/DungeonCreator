using UnityEngine;
using Sirenix.OdinInspector;

namespace _ScriptableObjects.Traps
{
    [CreateAssetMenu(fileName = "New Enemy Properties", menuName = "SO/Traps/Enemies/Default"), InlineEditor]
    public class EnemyProperties : ScriptableObject
    {
        [TitleGroup("Enemy"), LabelWidth(125), Range(50, 400), GUIColor(0, 2, 0.5f)]
        public float health = 100;
        [TitleGroup("Enemy"), LabelWidth(125), Range(10, 150), GUIColor(0, 1.5f, 2)]
        public float damages = 25;

        [TitleGroup("Chase properties"), LabelWidth(125), Range(0f, 0.1f), GUIColor(2, 1, 0)]
        public float smoothRotation = 0.1f;
        [TitleGroup("Chase properties"), LabelWidth(125), Range(1f, 10f), GUIColor(2, 1, 0)]
        public float chaseSpeed = 3f;
        [TitleGroup("Chase properties"), LabelWidth(125), Range(0f, 8f), GUIColor(2, 1, 0)]
        public float stoppingDistance = 1.5f;

        [TitleGroup("Patrol properties"), LabelWidth(125), Range(1, 15), GUIColor(0, 1.5f, 0f)]
        public float patrolSpeed = 1f;
        [TitleGroup("Patrol properties"), LabelWidth(125), Range(1, 15), GUIColor(0, 1.5f, 0f)]
        public float returnSpeed = 2f;
        [TitleGroup("Patrol properties"), LabelWidth(125), Range(1, 20), GUIColor(0, 1.5f, 0f)]
        public float patrolRadius = 3f;
        [TitleGroup("Patrol properties"), LabelWidth(125), Range(0, 15), GUIColor(0, 1.5f, 0f)]
        public float minPatrolWait = 5f;
        [TitleGroup("Patrol properties"), LabelWidth(125), Range(0, 15), GUIColor(0, 1.5f, 0f)]
        public float maxPatrolWait = 8f;
        [TitleGroup("Patrol properties"), LabelWidth(125), Range(0, 3), GUIColor(0, 1.5f, 0f)]
        public float pointDistance = 0.5f;
        
        [TitleGroup("FOV"), LabelWidth(125), Range(1, 30), GUIColor(0, 1.5f, 2)]
        public float radius = 10f;
        [TitleGroup("FOV"), LabelWidth(125), Range(0, 360), GUIColor(0, 1.5f, 2)]
        public float angle = 160;
        [TitleGroup("FOV"), LabelWidth(125), GUIColor(0, 1.5f, 2)]
        public LayerMask targetMask;
        [TitleGroup("FOV"), LabelWidth(125), GUIColor(0, 1.5f, 2)]
        public LayerMask obstructionMask;
    }
}