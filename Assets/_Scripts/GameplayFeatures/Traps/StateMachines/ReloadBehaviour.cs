using UnityEngine;

namespace _Scripts.GameplayFeatures.Traps
{
	public class ReloadBehaviour : StateMachineBehaviour
	{
		public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{
			animator.SetBool("IsReloaded", true);
		}
	}
}
