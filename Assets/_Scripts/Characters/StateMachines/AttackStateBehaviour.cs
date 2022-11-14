using UnityEngine;
using _Scripts.Characters.StateMachines;
using _Scriptables.Curves;

namespace _Scripts.Characters.Animations.StateMachines
{
    public class AttackStateBehaviour : NetworkStateMachine
    {
        #region Variables
        [Header("Attack properties")]
        [SerializeField] protected CombatCurve combatCurves;
        [SerializeField] protected int curveIndex;
        [SerializeField] protected float attackCooldown = 0.7f;
        [SerializeField] protected float dodgeCooldown = 0.7f;

        protected Character _character;
        protected AnimationCurve _attackCurve;
        #endregion

        #region Inherited_Methods
        protected override void StateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            Character player = CharacterAnimation.GetPlayer(animator);
            _character = player;

            player.PlayerSM.CurrentState = PlayerStateMachine.PlayerStates.Attack;
            _character.PlayerSM.CanAttack = false;
            _character.PlayerSM.CanDodge = false;

            SetAttackCurve(player);
            player.SetPlayerMeshOrientation(player.Orientation.forward);
        }

        protected override void StateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            DisableActions(stateInfo.normalizedTime);

            float speed = GetCombatMomentum(stateInfo.normalizedTime);
            _character.UpdateSpeed(speed);
        }
        #endregion

        #region Methods
        /// <summary>
        /// Smooth momentum when starting the attack
        /// </summary>
        protected void SetAttackCurve(Character player)
        {
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
        protected void DisableActions(float time)
        {
            _character.PlayerSM.CanAttack = time >= attackCooldown ? true : false;
            _character.PlayerSM.CanDodge = time >= dodgeCooldown ? true : false;
        }
        #endregion
    }
}
