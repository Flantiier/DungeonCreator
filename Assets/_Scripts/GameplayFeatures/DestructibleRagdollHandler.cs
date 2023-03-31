using UnityEngine;
using Sirenix.OdinInspector;

namespace _Scripts.GameplayFeatures
{
    public class DestructibleRagdollHandler : MonoBehaviour
    {
        [SerializeField] private bool enableOnStart;
        [SerializeField] private DestructibleRagdollPart[] ragdollParts;

        private void Awake()
        {
            if (enableOnStart)
                return;

            ResetRagdoll();
        }

        /// <summary>
        /// Enable all the ragdoll parts
        /// </summary>
        [Button("Activate Ragdoll", ButtonSizes.Medium)]
        public void EnableRagdoll()
        {
            if (ragdollParts.Length <= 0)
                return;

            foreach (DestructibleRagdollPart ragdoll in ragdollParts)
            {
                ragdoll.gameObject.SetActive(true);
                ragdoll.SetRagdollToBone();
                ragdoll.IsPhysic();
            }
        }

        /// <summary>
        /// Disable all the ragdoll parts
        /// </summary>
        [Button("Reset Ragdoll", ButtonSizes.Medium)]
        public void ResetRagdoll()
        {
            if (ragdollParts.Length <= 0)
                return;

            foreach (DestructibleRagdollPart ragdoll in ragdollParts)
            {
                ragdoll.SetRagdollToBone();
                ragdoll.IsKinematic();
                ragdoll.gameObject.SetActive(false);
            }
        }

        /// <summary>
        /// Get ragdoll parts in children
        /// </summary>
        [Button("Get Ragdoll Parts", ButtonSizes.Medium)]
        private void GetRagdollParts()
        {
            if (transform.childCount <= 0)
                return;

            ragdollParts = transform.GetComponentsInChildren<DestructibleRagdollPart>();
        }

        /// <summary>
        /// Enable or disable all the children objects
        /// </summary>
        [Button("Enable/Disable Parts", ButtonSizes.Medium)]
        private void EnableChildren()
        {
            if (transform.childCount <= 0)
                return;

            bool state = transform.GetChild(0).gameObject.activeSelf;

            foreach (Transform child in transform)
            {
                child.gameObject.SetActive(!state);
            }
        }
    }
}
