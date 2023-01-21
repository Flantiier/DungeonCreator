using UnityEngine;

namespace _Scripts.GameplayFeatures.Traps
{
    public class SpikesBehaviour : DamagingTrap
    {
        #region Builts_In
        public override void OnEnable()
        {
            base.OnEnable();

            if (!ViewIsMine())
                return;

            SyncAnimator(0);
        }
        #endregion
    }
}