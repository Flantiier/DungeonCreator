using UnityEngine;

namespace _Scripts.Characters.Animations.StateMachines
{
	public class AttackState : NetworkStateMachine
	{
        #region Variables
        [Header("Actions Cooldown")]
        [SerializeField] protected float attackCooldown = 0.7f;
        [SerializeField] protected float dodgeCooldown = 0.7f;
        #endregion

        #region Inherited Methods
        protected override void StateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            MyCharacter.PlayerSM.CanAttack = false;
            MyCharacter.PlayerSM.CanDodge = false;
        }

        protected override void StateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            DisableActions(stateInfo.normalizedTime);
        }
        #endregion

        #region Methods
        /// <summary>
        /// Disable combo during the beginning of the attack
        /// </summary>
        protected void DisableActions(float time)
        {
            MyCharacter.PlayerSM.CanAttack = time >= attackCooldown ? true : false;
            MyCharacter.PlayerSM.CanDodge = time >= dodgeCooldown ? true : false;
        }
        #endregion
    }
}
