using UnityEngine;
using _Scripts.NetworkScript;

namespace _Scripts.Characters.DungeonMaster
{
    public class BossAnimator : NetworkMonoBehaviour
    {
        #region Properties
        public BossController Boss { get; private set; }
        #endregion

        #region Built_In
        private void Awake()
        {
            Boss = GetComponentInParent<BossController>();
        }
        #endregion

        #region Methods
        public void UseAbility(int index)
        {
            Boss.AbilityUsed(index);
        }
        #endregion
    }
}