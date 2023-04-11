using UnityEngine;
using Sirenix.OdinInspector;

namespace _ScriptableObjects.Traps
{
    [CreateAssetMenu(fileName = "New Enemy Properties", menuName = "Traps/Enemies/Default"), InlineEditor]
    public class EnemyProperties : ScriptableObject
    {
        [TitleGroup("Enemy")]
        public float health = 100;
        public float damages = 25;

        [TitleGroup("Chasing")]
        public float smoothRotation = 0.1f;
        [TitleGroup("Chasing")]
        public float chaseSpeed = 3f;
        [TitleGroup("Chasing")]
        public float stoppingDistance = 1.5f;
        
        [TitleGroup("Patroling")]
        public float patrolSpeed = 1f;
        [TitleGroup("Patroling")]
        public float returnSpeed = 2f;
        [TitleGroup("Patroling")]
        public float patrolRadius = 3f;
        [TitleGroup("Patroling")]
        public float minPatrolWait = 5f;
        [TitleGroup("Patroling")]
        public float maxPatrolWait = 8f;
        [TitleGroup("Patroling")]
        public float pointDistance = 0.5f;
        
        [TitleGroup("FOV")]
        public float radius = 10f;
        [TitleGroup("FOV"), Range(0, 360)]
        public float angle = 160;
        [TitleGroup("FOV")]
        public LayerMask targetMask;
        [TitleGroup("FOV")]
        public LayerMask obstructionMask;
    }
}