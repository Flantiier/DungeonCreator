using UnityEngine;
using Sirenix.OdinInspector;
using _Scripts.Hitboxs_Triggers.Hitboxs;
using _ScriptableObjects.Traps;

namespace _Scripts.GameplayFeatures.IA
{
    public class SwordAndShieldEnemy : ChasingEnemy
    {
        [FoldoutGroup("References")]
        [SerializeField] private EnemyWeaponHitbox sword;

        [TitleGroup("Properties")]
        [OnValueChanged("GetProperties")]
        [SerializeField] private SnsEnemyProperties classProperties;

        protected override void Awake()
        {
            base.Awake();
            
            if (properties)
                return;

            GetProperties();
        }

        protected override void InitializeEnemy()
        {
            sword.Damages = classProperties.damages;
            base.InitializeEnemy();
        }

        public void EnableCollider(int state)
        {
            sword.Collider.enabled = state > 0;
        }

        public bool ShouldDefend()
        {
            return classProperties.defendRatio <= Random.Range(0f, 1f);
        }

        private void GetProperties()
        {
            properties = classProperties;
        }
    }
}

