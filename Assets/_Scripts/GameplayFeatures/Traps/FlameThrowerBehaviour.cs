using System.Collections;
using UnityEngine;
using UnityEngine.VFX;
using Sirenix.OdinInspector;
using _ScriptableObjects.Traps;

namespace _Scripts.GameplayFeatures.Traps
{
    public class FlameThrowerBehaviour : DefusableTrap
    {
        #region Variables
        [FoldoutGroup("References")]
        [SerializeField] private Transform rayStart;
        [FoldoutGroup("References")]
        [SerializeField] private VisualEffect fx;

        [FoldoutGroup("Audio")]
        [SerializeField] private AudioClip fireClip;
        [FoldoutGroup("Audio")]
        [SerializeField] private AudioClip fireEffect;

        [BoxGroup("Properties")]
        [SerializeField] private LayerMask rayMask;
        [BoxGroup("Properties")]
        [Required, SerializeField] private FlameThrowerProperties datas;

        private float _currentRayLength;
        #endregion

        #region Builts_In
        protected override void Awake()
        {
            base.Awake();
            DefuseDuration = datas.defuseDuration;
            _audioSource.clip = fireClip;
        }

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

        protected override IEnumerator DefusedTrapRoutine()
        {
            HighlightTrap(false);

            //Stop the flammes
            StopCoroutine("FlamesRoutine");
            fx.Stop();

            //Base routine
            hitbox.EnableCollider(false);
            IsDisabled = true;
            yield return new WaitForSecondsRealtime(DefuseDuration);
            IsDisabled = false;
            hitbox.EnableCollider(true);

            //Restart the flames
            StartCoroutine("FlamesRoutine");
        }
        #endregion

        #region Methods
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
            HandleAudio(state);

            //False
            if (!state)
            {
                fx.Stop();
                return;
            }

            //True
            fx.Play();
        }

        /// <summary>
        /// Play a fire audio effect
        /// </summary>
        private void HandleAudio(bool state)
        {
            if (state)
            {
                _audioSource.PlayOneShot(fireEffect);
                _audioSource.Play();
            }
            else
                _audioSource.Stop();
        }
        #endregion
    }
}
