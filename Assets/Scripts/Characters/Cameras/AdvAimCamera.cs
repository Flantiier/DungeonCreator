using UnityEngine;
using Cinemachine;

namespace _Scripts.Characters.Cameras
{
	public class AdvAimCamera : AdvSimpleCamera
	{
		#region Variables

		[SerializeField] private CinemachineFreeLook aimFreelook;

		#endregion

		#region Methods

		public override void SetCamera(Transform target)
		{
			base.SetCamera(target);

			aimFreelook.Follow = target;
			aimFreelook.LookAt = target;
		}

		public override void SwicthToAim(int mainPriority, int aimPriority)
		{
			mainFreelook.Priority = mainPriority;
			aimFreelook.Priority = aimPriority;
		}

		#endregion
	}
}
