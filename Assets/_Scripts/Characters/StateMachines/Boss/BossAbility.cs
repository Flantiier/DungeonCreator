using UnityEngine;
using Sirenix.OdinInspector;
using _Scripts.Characters.DungeonMaster;

namespace _Scripts.Characters.Animations.StateMachines.Boss
{
	public class BossAbility : BossStateMachine
    {
        #region Variables
        [TitleGroup("Ability properties")]
        [SerializeField] private int index = 0;
        #endregion

        #region Inherited_Methods
        protected override void StateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            //Reset velocity
            Boss.ResetCharacterVelocity();

            //Attack
            Boss.SetBossState(BossController.BossState.Attack);
            Boss.CanAttack = false;
            //Set the curve & rotations
            Boss.SetMeshOrientation(Boss.Orientation.forward);

            //Use ability
            Boss.AbilityUsed(index);
        }
        #endregion
    }
}
