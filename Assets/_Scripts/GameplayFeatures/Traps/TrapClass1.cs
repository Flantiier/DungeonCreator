using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using _Scripts.NetworkScript;
using _Scripts.Interfaces;
using UnityEngine.Tilemaps;

namespace _Scripts.GameplayFeatures.Traps
{
	public class TrapClass1 : NetworkAnimatedObject, IDetectable
	{
        #region Properties
        public List<Tile> OccupedTiles { get; set; }
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
        #endregion
    }
}
