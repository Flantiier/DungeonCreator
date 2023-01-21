using System.Collections;
using UnityEngine;
using Sirenix.OdinInspector;
using _Scripts.NetworkScript;
using _Scripts.TrapSystem;
using _Scripts.Interfaces;

namespace _Scripts.GameplayFeatures.Traps
{
	public class TrapClass1 : NetworkAnimatedObject, IDetectable
	{
        #region Variables/Properties
        [TitleGroup("Default properties")]
		[SerializeField] protected Tile.TilingType type;
        #endregion

        #region Properties
        public Tile.TilingType Type => type;
        #endregion

        #region Detection Interaction
        public void GetDetected(float duration)
        {
            //StartCoroutine("DetectionRoutine");
        }

        protected virtual IEnumerator DetectionRoutine(float duration)
        {
            yield return new WaitForSecondsRealtime(duration);
        }
        #endregion

        #region Methods
        public virtual void EnableHitbox(int value) { }

        /// <summary>
        /// Synchronize animator current animation over the network
        /// </summary>
        protected void SyncAnimator(int layer)
        {
            float time = Animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
            int hash = Animator.GetCurrentAnimatorStateInfo(0).fullPathHash;
            RPCSynchronizeAnimator(Photon.Pun.RpcTarget.OthersBuffered, hash, layer, time);
        }

        protected virtual IEnumerator DetectedRoutine(float duration)
        {
            yield return new WaitForSeconds(duration);
        }
        #endregion
    }
}
