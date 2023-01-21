using UnityEngine;
using Sirenix.OdinInspector;

namespace _Scripts.GameplayFeatures.PhysicsAdds
{
	public class DetectionBox : PhysicsDetection
	{
		#region Variables
		[TitleGroup("Physics properties")]
		[SerializeField] private Vector3 extends =  Vector3.one;
		[SerializeField] private Vector3 offset = Vector3.zero;
		[SerializeField] private LayerMask mask;
		#endregion

		#region Methods
		private void OnDrawGizmosSelected()
		{
			if (!enableHelpers)
				return;

			Gizmos.color = helpersColor;
			Gizmos.DrawCube(GetPositionOffset(), extends * 2f);
		}

		public override bool IsDetecting()
		{
			if (!Enabled)
				return false;

			return Physics.CheckBox(GetPositionOffset(), extends, Quaternion.identity, mask);
		}

		private Vector3 GetPositionOffset()
		{
			return transform.position + offset;
		}
		#endregion
	}
}
