using UnityEngine;
using Sirenix.OdinInspector;
using _Scripts.GameplayFeatures.PhysicsAdds;

namespace _Scripts.GameplayFeatures.Traps
{
	public class BallistaBehaviour : DestructibleTrap
	{
		#region Variables
		[TitleGroup("References")]
		[SerializeField] private SphericalFOV fov;
		[TitleGroup("References")]
        [SerializeField] private Transform verticalPart;
        [TitleGroup("References")]
        [SerializeField] private Transform horizontalPart;

        [TitleGroup("Properties")]
		[SerializeField, Range(0f, 0.2f)] private float smoothRotate = 0.1f;
		[TitleGroup("Properties")]
		[SerializeField] private float fireRate = 3f;

        private Coroutine _shotsRoutine;
		#endregion

		#region Builts_In
		private void Update()
		{
			HandleBehaviour();
		}
		#endregion

		#region Methods
		/// <summary>
		/// Rotate and shot if there is atleast one reachable target
		/// </summary>
		private void HandleBehaviour()
		{
			if (!fov || !fov.IsDetecting())
				return;

			Transform target = fov.GetTargetInFOV();

			if (!target)
				return;

			Quaternion targetRotation = Quaternion.LookRotation(target.position - transform.position);
			horizontalPart.rotation = Quaternion.Euler(0f, 0f, targetRotation.z);
			verticalPart.rotation = Quaternion.Euler(0f, targetRotation.y, 0f);
        }
		#endregion
	}
}
