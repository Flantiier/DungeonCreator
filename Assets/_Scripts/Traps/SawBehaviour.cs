using UnityEngine;

namespace _Scripts.GameplayFeatures.Traps
{
    public class SawBehaviour : DamagingTrap
    {
        #region Builts_In
        public override  void OnEnable()
        {
            base.OnEnable();

            if (!ViewIsMine())
                return;

            float time = Animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
            RPCSynchronizeAnimator(Photon.Pun.RpcTarget.OthersBuffered, 0, 0, time);
        }
        #endregion
    }
}