using UnityEngine;
using Sirenix.OdinInspector;
using _Scripts.Hitboxs_Triggers.Hitboxs;

namespace _Scripts.GameplayFeatures.Traps
{
    public class DamagingTrap : TrapClass1
    {
        #region Variables
        [FoldoutGroup("References")]
        [Required, SerializeField] protected TrapHitbox hitbox;
        #endregion

        #region Methods
        /// <summary>
        /// Enabled or disable the hitbox
        /// </summary>
        /// <param name="value"> 0 => disabled, 1 => enabled </param>
        public override void EnableHitbox(int value)
        {
            if (!hitbox)
                return;

            bool state = value <= 0 ? false : true;
            hitbox.EnableCollider(state);
        }

        /// <summary>
        /// Set the damages of the hitbox
        /// </summary>
        protected void SetHitboxDamages(float damages)
        {
            if (!hitbox)
            {
                Debug.LogWarning("Missing hitbow reference");
                return;
            }

            hitbox.Damages = damages;
        }
        #endregion
    }
}
