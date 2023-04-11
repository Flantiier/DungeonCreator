using UnityEngine;
using Sirenix.OdinInspector;
using _Scripts.Hitboxs_Triggers.Hitboxs;

namespace _Scripts.GameplayFeatures.IA
{
    public class SpearEnemy : ChasingEnemy
    {
        [FoldoutGroup("References")]
        [SerializeField] private EnemyHitbox spearHitbox;

        protected override void InitializeEnemy()
        {
            spearHitbox.Damages = properties.damages;

            base.InitializeEnemy();
        }

        public void EnableCollider(int index)
        {
            if (!spearHitbox)
                return;

            spearHitbox.Collider.enabled = index <= 0 ? false : true;
        }
    }
}
