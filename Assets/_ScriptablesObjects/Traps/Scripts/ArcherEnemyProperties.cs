using UnityEngine;
using Sirenix.OdinInspector;
using _Scripts.GameplayFeatures.Projectiles;

namespace _ScriptableObjects.Traps
{
    [CreateAssetMenu(fileName = "Archer Enemy", menuName = "Traps/Enemies/Archer Enemy"), InlineEditor]
    public class ArcherEnemyProperties : EnemyProperties
    {
        [TitleGroup("Archer properties"), LabelWidth(125), GUIColor(1, 2, 3)]
        public EnemiesProjectile projectile;
        [TitleGroup("Archer properties"), LabelWidth(125), Range(1, 20), GUIColor(1, 2, 3)]
        public float throwForce = 10f;
        [TitleGroup("Archer properties"), LabelWidth(125), Range(5, 100), GUIColor(1, 2, 3)]
        public float shootingDistance = 20f;
        [TitleGroup("Archer properties"), LabelWidth(125), GUIColor(1, 2, 3)]
        public LayerMask shootObstruction;
    }
}