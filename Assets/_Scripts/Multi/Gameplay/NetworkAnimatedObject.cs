using UnityEngine;
using Sirenix.OdinInspector;
using Photon.Pun;

namespace _Scripts.NetworkScript
{
    public class NetworkAnimatedObject : NetworkMonoBehaviour
    {
        #region Properties
        [TitleGroup("References")]
        [SerializeField] private Animator animator;

        public Animator Animator => animator;
        #endregion

        #region Methods

        #region AnimatorRPCs
        /// <summary>
        /// Set the trigger state over by sending an rpc
        /// P : string ParamName, bool state
        /// </summary>
        /// <param name="targetType"> Rpc target type </param>
        /// <param name="parameters"> string ParamName, bool state </param>
        public void RPCAnimatorTrigger(RpcTarget targetType, params object[] parameters)
        {
            RPCCall("TriggerRPC", targetType, parameters);
        }

        /// <summary>
        /// Synchronize the given animator all over the network
        /// P : Animator animator, int stateHash (0 == current), int layer, float timeValue
        /// </summary>
        public void RPCSynchronizeAnimator(RpcTarget targetType, params object[] parameters)
        {
            RPCCall("SyncAnimatorRPC", targetType, parameters);
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
                Debug.LogWarning("Animator Rpc called but animator property is missing.");
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
        /// Synchronize a given animator
        /// </summary>
        [PunRPC]
        public void SyncAnimatorRPC(int stateHash, int layer, float normalizeTime)
        {
            if (!animator)
                return;

            animator.Play(stateHash, layer, normalizeTime);
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
        }
        #endregion

        #endregion
    }
}
