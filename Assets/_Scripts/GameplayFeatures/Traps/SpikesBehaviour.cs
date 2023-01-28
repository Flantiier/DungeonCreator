using UnityEngine;
using Sirenix.OdinInspector;
using _ScriptableObjects.Traps;

namespace _Scripts.GameplayFeatures.Traps
{
    public class SpikesBehaviour : DamagingTrap
    {
        #region Variables
        [BoxGroup("Properties")]
        [Required, SerializeField] private SpikesDatas datas;
        #endregion

        #region Inherited Methods
        protected override void InitializeTrap()
        {
            if (!ViewIsMine())
                return;

            SetHitboxDamages(datas.damages);
            SyncAnimator(0);
        }
        #endregion
    }
}