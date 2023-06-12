using UnityEngine;
using System.Collections;
using _Scripts.Interfaces;

namespace _Scripts.GameplayFeatures.Traps
{
    public class DefusableTrap : DamagingTrap, IDefusable
    {
        #region Variables/Properties
        private const string HIGHLIGHT_PROP = "_highlight";

        public float DefuseDuration { get; set; }
        public bool IsDisabled { get; set; }
        #endregion

        #region Methods
        /// <summary>
        /// Method to trigger the defuse routine
        /// </summary>
        public void DefuseTrap()
        {
            StartCoroutine("DefusedTrapRoutine");
        }

        /// <summary>
        /// Disable trap routine
        /// </summary>
        /// <returns></returns>
        protected virtual IEnumerator DefusedTrapRoutine()
        {
            float baseSpeed = Animator.speed;
            HighlightTrap(false);

            IsDisabled = true;
            Animator.speed = 0f;
            hitbox.EnableCollider(false);
            yield return new WaitForSecondsRealtime(DefuseDuration);

            IsDisabled = false;
            Animator.speed = baseSpeed;
            hitbox.EnableCollider(true);
        }

        /// <summary>
        /// Set a property on the material to hightlight the object
        /// </summary>
        /// <param name="enabled"></param>
        public void HighlightTrap(bool enabled)
        {
            if (IsDisabled || !_sharedMaterial.HasInt(HIGHLIGHT_PROP))
                return;

            _sharedMaterial.SetInt(HIGHLIGHT_PROP, enabled ? 1 : 0);
        }
        #endregion
    }
}