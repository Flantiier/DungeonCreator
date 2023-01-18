using UnityEngine;
using _Scripts.Interfaces;
using _ScriptableObjects.Afflictions;

namespace _Scripts.Hitboxs_Triggers.Hitboxs
{
    public class AfflictionHitbox : Hitbox
    {
        #region Properties
        public AfflictionStatus affliction { get; set; }
        #endregion

        #region Builts_In
        private void OnTriggerEnter(Collider other)
        {
            if (!other.TryGetComponent(out IPlayerAfflicted player))
                return;

            player.TouchedByAffliction(affliction);
        }
        #endregion
    }
}