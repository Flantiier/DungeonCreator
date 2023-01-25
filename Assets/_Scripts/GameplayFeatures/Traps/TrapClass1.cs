using System.Collections;
using UnityEngine;
using _Scripts.NetworkScript;
using _Scripts.Interfaces;
using _Scripts.TrapSystem;

namespace _Scripts.GameplayFeatures.Traps
{
    public class TrapClass1 : NetworkAnimatedObject, IDetectable
    {
        #region Properties
        public Tile[] OccupedTiles { get; set; }
        #endregion

        #region Built_In
        public override void OnDisable()
        {
            base.OnDisable();

            if (!ViewIsMine())
                return;

            ResetOccupedTiles();
        }
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

        /// <summary>
        /// Set the state of the tiles in the array to free
        /// </summary>
        protected virtual void ResetOccupedTiles()
        {
            if (OccupedTiles == null || OccupedTiles.Length <= 0)
                return;

            foreach (Tile tile in OccupedTiles)
            {
                if (!tile)
                    return;

                tile.NewTileState(Tile.TileState.Free);
            }

            OccupedTiles = new Tile[0];
        }
        #endregion
    }
}
