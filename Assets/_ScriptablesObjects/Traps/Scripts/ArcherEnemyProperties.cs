using UnityEngine;
using Sirenix.OdinInspector;
using _Scripts.GameplayFeatures.Projectiles;

namespace _ScriptableObjects.Traps
{
    [CreateAssetMenu(fileName = "Archer Enemy", menuName = "Traps/Enemies/Archer Enemy"), InlineEditor]
    public class ArcherEnemyProperties : EnemyProperties
    {
        [TitleGroup("Archer properties")]
        public float shootingDistance = 20f;
        public LayerMask shootObstruction;

        public EnemiesProjectile projectile;
        public float throwForce = 10f;
    }
}