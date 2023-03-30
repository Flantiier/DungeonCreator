using UnityEngine;

namespace _Scripts.GameplayFeatures
{
    public class RagdollHandler : MonoBehaviour
    {
        [SerializeField] private bool enableOnStart;
        [SerializeField] private RagdollObject[] ragdollParts;

        private void Awake()
        {
            if (enableOnStart)
                return;

            DisableRagdoll();
        }

        /// <summary>
        /// Enable all the ragdoll parts
        /// </summary>
        [ContextMenu("Enable")]
        public void EnableRagdoll()
        {
            foreach (RagdollObject ragdoll in ragdollParts)
            {
                ragdoll.gameObject.SetActive(true);
                ragdoll.SetRagdollToBone();
                ragdoll.IsPhysic();
            }
        }
        
        /// <summary>
        /// Disable all the ragdoll parts
        /// </summary>
        [ContextMenu("Disable")]
        public void DisableRagdoll()
        {
            foreach (RagdollObject ragdoll in ragdollParts)
            {
                ragdoll.SetRagdollToBone();
                ragdoll.IsKinematic();
                ragdoll.gameObject.SetActive(false);
            }
        }

        /// <summary>
        /// Get ragdoll parts in children
        /// </summary>
        [ContextMenu("Get Ragdoll")]
        private void GetRagdollParts()
        {
            ragdollParts = transform.GetComponentsInChildren<RagdollObject>();
        }
    }
}
