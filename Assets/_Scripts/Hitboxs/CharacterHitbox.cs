using UnityEngine;
using _Scripts.Characters;
using _Scripts.Interfaces;

namespace _Scripts.Hitboxs
{
    public class CharacterHitbox : Hitbox
    {
        #region Variables
        [SerializeField] private bool IsMainAttack = true;

        private Characters.Character _character;
        #endregion

        #region Builts_In
        public override void Awake()
        {
            base.Awake();

            Collider.enabled = false;

            if(ViewIsMine())
                _character = GetComponentInParent<Character>();

        }

        protected override void TriggerEnter(Collider other)
        {
            if (!other.TryGetComponent(out IDamageable player))
                return;

            Debug.Log("Collide");
            player.Damage(_character.CharacterDatas.GetAttackDamages(IsMainAttack));
        }
        #endregion
    }
}
