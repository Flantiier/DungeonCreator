using System.Collections;
using UnityEngine;
using UnityEngine.VFX;
using Sirenix.OdinInspector;
using _Scripts.Interfaces;
using _Scripts.Hitboxs_Triggers.Hitboxs;
using _Scripts.GameplayFeatures.PhysicsAdds;
using _ScriptableObjects.Afflictions;

namespace _Scripts.GameplayFeatures.Traps
{
    public class BulbBehaviour : DestructibleTrap
    {
        #region Variables
        [TitleGroup("Bulb Properties")]
        [SerializeField] private AfflictionSetup afflictionSetup;
        [SerializeField] private DetectionBox detectionBox;
        [SerializeField] private VisualEffect fx;
        [SerializeField] private float sporesDuration = 5f;
        [SerializeField] private float sporesDelay = 0.5f;

        private bool _IsAttacking;
        #endregion

        #region Builts_In
        private void Awake()
        {
            afflictionSetup.Initialize();
        }

        public override void OnEnable()
        {
            base.OnEnable();
            afflictionSetup.hitbox.EnableCollider(false);

            if (!ViewIsMine())
                return;

            SyncAnimator(0);
        }

        private void Update()
        {
            HandleBulbBehaviour();
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
            afflictionSetup.hitbox.EnableCollider(true);
            yield return new WaitForSecondsRealtime(sporesDuration);
            afflictionSetup.hitbox.EnableCollider(false);
            fx.Stop();

            yield return new WaitForSecondsRealtime(sporesDelay);
            _IsAttacking = false;
        }
        #endregion
    }
}

#region AfflictionPack_Class
namespace _Scripts.GameplayFeatures
{
    [System.Serializable]
    public class AfflictionSetup
    {
        public AfflictionHitbox hitbox;
        public AfflictionStatus affliction;

        public void Initialize()
        {
            hitbox.affliction = affliction;
        }
    }
}
#endregion