using System.Collections;
using UnityEngine;
using UnityEngine.VFX;
using Sirenix.OdinInspector;
using _Scripts.Hitboxs_Triggers.Hitboxs;
using _Scripts.GameplayFeatures.PhysicsAdds;
using _ScriptableObjects.Traps;

namespace _Scripts.GameplayFeatures.Traps
{
    public class BulbBehaviour : DestructibleTrap
    {
        #region Variables
        [FoldoutGroup("References")]
        [SerializeField] private AfflictionHitbox hitbox;
        [FoldoutGroup("References")]
        [SerializeField] private DetectionBox detectionBox;
        [FoldoutGroup("References")]
        [SerializeField] private VisualEffect fx;

        [BoxGroup("Stats")]
        [Required, SerializeField] private BulbDatas datas;

        private bool _IsAttacking;
        #endregion

        #region Builts_In
        private void Awake()
        {
            hitbox.affliction = datas.affliction;
        }

        private void Update()
        {
            HandleBulbBehaviour();
        }
        #endregion

        #region Inherited Methods
        protected override void InitializeTrap()
        {
            if (!ViewIsMine())
                return;

            SetTrapHealth(datas.health);
            SyncAnimator(0);
        }
        #endregion

        #region Methods
        /// <summary>
        /// Handle the behaviour of the bulb
        /// </summary>
        private void HandleBulbBehaviour()
        {
            if (!detectionBox || !detectionBox.IsDetecting())
                return;

            if (_IsAttacking)
                return;

            _IsAttacking = true;
            Animator.SetTrigger("Spores");
        }

        /// <summary>
        /// Start the coroutine to ejects the spores
        /// </summary>
        [ContextMenu("Attack")]
        public void BulbAttack()
        {
            StartCoroutine("SporesRoutine");
        }

        /// <summary>
        /// Ejects spores and enable collider
        /// </summary>
        private IEnumerator SporesRoutine()
        {
            fx.Play();
            hitbox.EnableCollider(true);
            yield return new WaitForSecondsRealtime(datas.sporesDuration);
            hitbox.EnableCollider(false);
            fx.Stop();

            yield return new WaitForSecondsRealtime(datas.waitTime);
            _IsAttacking = false;
        }
        #endregion
    }
}