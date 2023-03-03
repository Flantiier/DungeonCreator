using UnityEngine;
using _Scripts.Characters.StateMachines;
using _ScriptableObjects.Characters;

namespace _Scripts.Characters.Animations.StateMachines
{
    public class CharacterMeleeAttack : CharacterAttack
    {
        #region Variables
        [Header("Attack properties")]
        [SerializeField] protected CombatCurve combatCurves;
        [SerializeField] protected int curveIndex;

        protected AnimationCurve _attackCurve;
        #endregion

        #region Inherited_Methods
        protected override void StateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            Character.PlayerSM.CurrentState = PlayerStateMachine.PlayerStates.Attack;
            base.StateEnter(animator, stateInfo, layerIndex);

            SetAttackCurve();
            Character.SetMeshOrientation(Character.Orientation.forward);
        }

        protected override void StateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            base.StateUpdate(animator, stateInfo, layerIndex);

            float speed = GetCombatMomentum(stateInfo.normalizedTime);
            Character.UpdateCharacterSpeed(speed);
            Character.MoveForwards();
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
        #endregion
    }
}
