using UnityEngine;
using Sirenix.OdinInspector;
using _Scripts.Hitboxs_Triggers.Hitboxs;

namespace _Scripts.GameplayFeatures.IA
{
    public class SwordAndShieldEnemy : ChasingEnemy
    {
        [TitleGroup("Equipment")]
        [SerializeField] private EnemyWeaponHitbox sword;
        [SerializeField] private float defendRatio = 0.5f;

        public void EnableCollider(int state)
        {
            sword.Collider.enabled = state > 0;
        }

        public bool ShouldDefend()
        {
            return defendRatio <= Random.Range(0f, 1f);
        }
    }
}

