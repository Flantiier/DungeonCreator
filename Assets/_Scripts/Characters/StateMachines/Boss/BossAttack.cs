using UnityEngine;
using _ScriptableObjects.Characters;
using _Scripts.Characters.DungeonMaster;

namespace _Scripts.Characters.Animations.StateMachines.Boss
{
    public class BossAttack : BossStateMachine
    {
        #region Variables
        [Header("Attack properties")]
        [SerializeField] protected float attackCooldown = 0.7f;
        [SerializeField] protected CombatCurve combatCurves;
        [SerializeField] protected int curveIndex;

        protected AnimationCurve _attackCurve;
        #endregion

        #region Inherited_Methods
        protected override void StateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            //Attack
            Boss.SetBossState(BossController.BossState.Attack);
            Boss.CanAttack = false;
            //Set the curve & rotations
            SetAttackCurve();
            Boss.SetMeshOrientation(Boss.Orientation.forward);
        }

        protected override void StateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            //Disable boss actions
            DisableActions(Boss, stateInfo.normalizedTime);
            //Combat momentum
            Boss.UpdateCharacterSpeed(GetCombatMomentum(stateInfo.normalizedTime));
            Boss.MoveForwards();
        }
        #endregion

        #region Methods
        /// <summary>
        /// Smooth momentum when starting the attack
        /// </summary>
        protected void SetAttackCurve()
        {
            if (!combatCurves)
                return;

            _attackCurve = combatCurves.curves[curveIndex];
        }

        /// <summary>
        /// Read the combat curve to get a motion speed
        /// </summary>
        protected float GetCombatMomentum(float normalizedTime)
        {
            if (_attackCurve == null)
                return 0f;

            return _attackCurve.Evaluate(normalizedTime);
        }

        /// <summary>
        /// Disable combo during the beginning of the attack
        /// </summary>
        protected void DisableActions(BossController boss, float time)
        {
            boss.CanAttack = time >= attackCooldown ? true : false;
        }
        #endregion
    }
}