using UnityEngine;
using Sirenix.OdinInspector;
using _ScriptableObjects.Traps;

namespace _Scripts.GameplayFeatures.Traps
{
    public class SawBehaviour : DefusableTrap
    {
        #region Variables
        [BoxGroup("Properties")]
        [Required, SerializeField] private SawPropeerties datas;
        #endregion

        #region Builts_In
        private void Awake()
        {
            DefuseDuration = datas.defuseDuration;
        }
        #endregion

        #region Inherited Methods
        protected override void InitializeTrap()
        {
            SetHitboxDamages(datas.damages);

            if (!ViewIsMine())
                return;

            SyncAnimator(0);
        }
        #endregion
    }
}