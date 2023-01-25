using UnityEngine;
using Sirenix.OdinInspector;

namespace _Scripts.GameplayFeatures.PhysicsAdds
{
	public class DetectionSphere : PhysicsDetection
	{
		#region Variables
		[TitleGroup("Properties")]
		[SerializeField] private float radius = 5f;
		[SerializeField] private LayerMask layerMask;
        #endregion

        #region Builts_In
        private void OnDrawGizmosSelected()
        {
            if (!enableHelpers)
                return;

            Gizmos.color = helpersColor;
            Gizmos.DrawSphere(transform.position, radius);
        }
        #endregion

        #region Methods
        public override bool IsDetecting()
        {
            return Physics.CheckSphere(transform.position, radius, layerMask);
        }
        #endregion
    }
}
