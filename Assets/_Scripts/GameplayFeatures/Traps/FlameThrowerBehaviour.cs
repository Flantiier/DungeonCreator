using System.Collections;
using UnityEngine;
using UnityEngine.VFX;
using Sirenix.OdinInspector;
using _ScriptableObjects.Traps;

namespace _Scripts.GameplayFeatures.Traps
{
    public class FlameThrowerBehaviour : DamagingTrap
    {
        #region Variables
        [FoldoutGroup("References")]
        [SerializeField] private Transform rayStart;
        [FoldoutGroup("References")]
        [SerializeField] private VisualEffect fx;

        [BoxGroup("Properties")]
        [SerializeField] private LayerMask rayMask;
        [BoxGroup("Properties")]
        [Required, SerializeField] private FlameThrowerDatas datas;

        private float _currentRayLength;
        #endregion

        #region Builts_In
        private void Start()
        {
            StartCoroutine("FlamesRoutine");
        }
        #endregion

        #region Inherited Methods
        protected override void InitializeTrap()
        {
            base.InitializeTrap();
            SetHitboxDamages(datas.damages);
        }
        #endregion

        #region Methods
        /// <summary>
        /// Shoot a ray and detect is smth is colliding towards the flame point
        /// </summary>
        private void ShootRayTowards()
        {
            float radius = datas.sprayRadius;
            Vector3 start = rayStart.position - rayStart.forward * radius;
            float finalDistance = datas.sprayDistance + radius;

            Ray ray = new Ray(start, rayStart.forward);
            Debug.DrawRay(rayStart.position, ray.direction * finalDistance, Color.cyan);

            if (Physics.SphereCast(ray, radius, out RaycastHit hit, finalDistance, rayMask, QueryTriggerInteraction.Collide))
            {
                _currentRayLength = Vector3.Distance(rayStart.position, hit.point);
                Debug.DrawRay(rayStart.position, ray.direction * _currentRayLength, Color.red);
            }
        }

        /// <summary>
        /// Throwing flames routine
        /// </summary>
        private IEnumerator FlamesRoutine()
        {
            ThrowFlames(true);
            yield return new WaitForSecondsRealtime(datas.sprayDuration);

            ThrowFlames(false);
            yield return new WaitForSecondsRealtime(datas.waitTime);

            StartCoroutine("FlamesRoutine");
        }

        /// <summary>
        /// Start vfx and enable the hitbox
        /// </summary>
        private void ThrowFlames(bool state)
        {
            hitbox.EnableCollider(state);

            //False
            if (!state)
            {
                fx.Stop();
                return;
            }

            //True
            fx.Play();
        }
        #endregion
    }
}
