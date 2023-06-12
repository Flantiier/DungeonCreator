using UnityEngine;
using Sirenix.OdinInspector;

namespace _Scripts.GameplayFeatures.PhysicsAdds
{
    public class SphericalFOV : PhysicsDetection
    {
        #region Variables
        [TitleGroup("Properties")]
        [SerializeField] private float radius = 5f;
        [SerializeField] private LayerMask targetMask;
        [SerializeField] private LayerMask obstructionMask;
        #endregion

        #region Builts_In
        private void Update()
        {
            if (!IsDetecting())
                return;

            Transform target = GetTargetInFOV();

            if (target)
                Debug.DrawLine(transform.position, target.position, Color.red);
        }

        private void OnDrawGizmosSelected()
        {
            if (!enableHelpers)
                return;

            Gizmos.DrawWireSphere(transform.position, radius);
        }
        #endregion

        #region Methods
        /// <summary>
        /// Get the first reachable target 
        /// </summary>
        public Transform GetTargetInFOV()
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position, radius, targetMask);

            //No targets
            if (colliders.Length <= 0)
                return null;

            foreach (Collider collider in colliders)
            {
                Transform target = collider.transform;
                Vector3 direction = (target.position - transform.position).normalized;
                float distance = Vector3.Distance(transform.position, target.position);

                //Is not visible / Loop through each collider
                if (Physics.Raycast(transform.position, direction, distance, obstructionMask))
                    continue;
                //Is visible
                else
                    return target;
            }

            return null;
        }

        /// <summary>
        /// First check before executing the loop
        /// </summary>
        public override bool IsDetecting()
        {
            if (!Enabled)
                return false;

            return Physics.CheckSphere(transform.position, radius, targetMask);
        }
        #endregion
    }
}