using UnityEngine;
using Sirenix.OdinInspector;
using _Scripts.Interfaces;

namespace _Scripts.Hitboxs_Triggers.Hitboxs
{
    public class TrapHitbox : EnemyHitbox
    {
        #region Variables
        [TitleGroup("Properties")]
        [SerializeField] private bool shouldDamageOnStay = false;
        #endregion

        #region Builts_In
        public override void OnTriggerEnter(Collider other)
        {
            DealDamages(other);
        }

        public void OnTriggerStay(Collider other)
        {
            if (!shouldDamageOnStay)
                return;

            DealDamages(other);
        }
        #endregion

        #region Methods
        /// <summary>
        /// Function to deal damages with the trap
        /// </summary>
        /// <param name="damageable"> Damageable trap object </param>
        private void DealDamages(Collider other)
        {
            if (!other.TryGetComponent(out ITrapDamageable damageable))
                return;

            damageable.TrapDamages(Damages);
        }
        #endregion
    }
}