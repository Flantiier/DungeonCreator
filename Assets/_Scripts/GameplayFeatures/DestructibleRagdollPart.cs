using UnityEngine;

namespace _Scripts.GameplayFeatures
{
    public class DestructibleRagdollPart : MonoBehaviour
    {
        [SerializeField] private SkinnedMeshRenderer boneReference;
        [SerializeField] private MeshFilter filter;
        [SerializeField] private MeshCollider meshCollider;
        public Rigidbody rb;

        public SkinnedMeshRenderer Bone { get => boneReference; set => boneReference = value; }

        /// <summary>
        /// Set the object position/rotation to the referenced bone position/rotation
        /// </summary>
        public virtual void SetRagdollToBone()
        {
            Mesh mesh = new Mesh();
            boneReference.BakeMesh(mesh);

            filter.mesh = mesh;
            meshCollider.sharedMesh = mesh;

            transform.position = boneReference.transform.position;
            transform.rotation = boneReference.transform.rotation;
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