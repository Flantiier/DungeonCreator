using UnityEngine;
using Photon.Pun;
using System.Collections;
using _Scripts.Interfaces;

namespace _Scripts.GameplayFeatures.Traps
{
    public class DefusableTrap : DamagingTrap, IDefusable
    {
        #region Variables/Properties
        private const string HIGHLIGHT_PROP = "_highlight";
        private float _baseSpeed;

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
            RPCCall("DisableTrap", RpcTarget.All);
            yield return new WaitForSecondsRealtime(DefuseDuration);
            RPCCall("EnableTrap", RpcTarget.All);
        }

        [PunRPC]
        protected void DisableTrap()
        {
            _baseSpeed = Animator.speed;
            HighlightTrap(false);

            IsDisabled = true;
            Animator.speed = 0f;
            hitbox.EnableCollider(false);
        }

        [PunRPC]
        protected void EnableTrap()
        {
            IsDisabled = false;
            Animator.speed = _baseSpeed;
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