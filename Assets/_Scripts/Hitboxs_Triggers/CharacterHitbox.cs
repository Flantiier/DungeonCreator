using UnityEngine;
using Sirenix.OdinInspector;
using _Scripts.Characters;
using _Scripts.Interfaces;
using _Scripts.GameplayFeatures.Weapons;

namespace _Scripts.Hitboxs_Triggers.Hitboxs
{
    public class CharacterHitbox : Hitbox
    {
        #region Variables
        [TitleGroup("Properties")]
        [SerializeField] private bool IsMainAttack = true;
        [HideInInspector] public Character character;
        #endregion

        #region Builts_In
        public override void Awake()
        {
            base.Awake();
            Collider.enabled = false;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out EnemyShield shield))
            {
                Debug.Log("Shield");
                Collider.enabled = false;
                return;
            }

            if (!other.TryGetComponent(out IDamageable player))
                return;

            float damages = character.CharacterDatas.GetAttackDamages(IsMainAttack);
            player.Damage(damages);
        }
        #endregion
    }
}
