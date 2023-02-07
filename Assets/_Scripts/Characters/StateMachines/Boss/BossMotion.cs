using UnityEngine;

namespace _Scripts.Characters.Animations.StateMachines.Boss
{
	public class BossMotion : BossStateMachine
	{
		protected override void StateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{
			Boss.SetBossState(DungeonMaster.BossController.BossState.Walk);
			Boss.CanAttack = true;
		}
	}
}
