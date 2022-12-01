using UnityEngine;
using Photon.Pun;

namespace _Scripts.NetworkScript
{
    public class NetworkAnimatedObject : NetworkMonoBehaviour
    {
        #region Properties
        [SerializeField] private Animator animator;

        public Animator Animator => animator;
        #endregion

        #region Methods

        #region AnimatorRPCs
        /// <summary>
        /// Set the trigger state over by sending an rpc
        /// </summary>
        /// <param name="targetType"> Rpc target type </param>
        /// <param name="parameters"> Rpc method parameters </param>
        public void RPCAnimatorTrigger(RpcTarget targetType, params object[] parameters)
        {
            RPCCall("TriggerRPC", targetType, parameters);
        }

        /// <summary>
        /// Trigger rpc callback
        /// </summary>
        /// <param name="paramName"> Animator parameter name </param>
        /// <param name="state"> True => Set, False => Reset </param>
        [PunRPC]
        public void TriggerRPC(string paramName, bool state)
        {
            if (!Animator)
            {
                Debug.LogWarning("Animator Rpc called but animator pproperty is missing.");
                return;
            }

            if (state)
            {
                Animator.SetTrigger(paramName);
                return;
            }

            Animator.ResetTrigger(paramName);
        }

        /// <summary>
        /// Reset animator parameters and update delta time to 0
        /// </summary>
        [PunRPC]
        public void ResetAnimatorRPC()
        {
            if (!Animator)
                return;

            Animator.Rebind();
            Animator.Update(0f);
        }
        #endregion

        #endregion
    }
}
