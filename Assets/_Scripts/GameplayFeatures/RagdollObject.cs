using UnityEngine;

namespace _Scripts.GameplayFeatures
{
    public class RagdollObject : MonoBehaviour
    {
        public Transform referencedBone;
        public Rigidbody rb;

        /// <summary>
        /// Set the object position/rotation to the referenced bone position/rotation
        /// </summary>
        public void SetRagdollToBone()
        {
            transform.position = referencedBone.position;
            transform.rotation = referencedBone.rotation;
        }

        /// <summary>
        /// Enable physics on object
        /// </summary>
        public void IsPhysic()
        {
            rb.isKinematic = false;
            rb.useGravity = true;
        }

        /// <summary>
        /// Enable kinematic on the rb of this object
        /// </summary>
        public void IsKinematic()
        {
            rb.isKinematic = true;
            rb.useGravity = false;
        }
    }
}