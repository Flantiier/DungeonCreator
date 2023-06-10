using UnityEngine;
using Sirenix.OdinInspector;
using _ScriptableObjects.Traps;

namespace _Scripts.GameplayFeatures.Traps
{
    public class PikesBehaviour : DefusableTrap
    {
        #region Variables
        [BoxGroup("Properties")]
        [Required, SerializeField] private PikesProperties datas;
        #endregion

        #region Inherited Methods
        private void Awake()
        {
            DefuseDuration = datas.defuseDuration;
        }

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