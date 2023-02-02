using System.Collections;
using UnityEngine;
using Sirenix.OdinInspector;
using _ScriptableObjects.Traps;

namespace _Scripts.GameplayFeatures.Traps
{
    public class PikesBehaviour : DamagingTrap
    {
        #region Variables
        [BoxGroup("Properties")]
        [Required, SerializeField] private PikesDatas datas;
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

        #region Defuse Interaction
        [ContextMenu("Defused")]
        public void IsDefused()
        {
            StartCoroutine("GetDefused");
        }

        protected override IEnumerator GetDefused()
        {
            float baseSpeed = Animator.speed;

            Animator.speed = 0f;
            hitbox.EnableCollider(false);
            yield return new WaitForSecondsRealtime(datas.defuseDuration);
            Animator.speed = baseSpeed;
            hitbox.EnableCollider(true);
        }
        #endregion
    }
}