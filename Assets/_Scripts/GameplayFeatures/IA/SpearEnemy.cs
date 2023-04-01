using UnityEngine;
using Sirenix.OdinInspector;
using _Scripts.Hitboxs_Triggers.Hitboxs;

namespace _Scripts.GameplayFeatures.IA
{
    public class SpearEnemy : ChasingEnemy
    {
        [TitleGroup("Equipment")]
        [SerializeField] private EnemyHitbox spearHitbox;

        public void EnableCollider(int index)
        {
            if (!spearHitbox)
                return;

            spearHitbox.Collider.enabled = index <= 0 ? false : true;
        }
    }
}
