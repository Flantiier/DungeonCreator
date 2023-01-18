using UnityEngine;
using Sirenix.OdinInspector;
using _Scripts.Characters;
using _Scripts.Interfaces;

namespace _Scripts.Hitboxs
{
    public class CharacterHitbox : Hitbox
    {
        #region Variables
        [TitleGroup("Properties")]
        [SerializeField] private bool IsMainAttack = true;
        [HideInInspector] public Character character;
        #endregion

        #region Builts_In
        public void Awake()
        {
            Collider.enabled = false;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!other.TryGetComponent(out IDamageable player))
                return;

            player.Damage(character.CharacterDatas.GetAttackDamages(IsMainAttack));
        }
        #endregion
    }
}
