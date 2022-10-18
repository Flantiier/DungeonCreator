using UnityEngine;
using _Scripts.Characters.StateMachines;
using _Scriptables.Curves;
using System;

namespace _Scripts.Characters.Animations.StateMachines
{
    public class CombatStateMachine : NetworkStateMachine
    {
        #region Variables

        [Header("Attack properties")]
        [SerializeField] protected CombatCurve combatCurves;
        [SerializeField] protected int curveIndex;
        [SerializeField] protected float attackCooldown = 0.7f;
        [SerializeField] protected float dodgeCooldown = 0.7f;

        [Header("Attack collider")]
        [SerializeField] protected int colliderIndex = 0;
        [SerializeField] protected float enableCollider = 0.3f, disableCollider = 0.8f;

        protected Character _character;
        protected AnimationCurve _attackCurve;

        #endregion

        #region Inherited_Methods

        protected override void OnEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            Character player = CharacterAnimation.GetPlayer(animator);
            _character = player;

            player.PlayerStateMachine.CurrentState = PlayerStateMachine.PlayerStates.Attack;
            _character.PlayerStateMachine.CanAttack = false;
            _character.PlayerStateMachine.CanDodge = false;

            SmoothPlayerMomentum(player);
            player.TurnPlayer();
        }

        protected override void OnUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            DisableActions(stateInfo.normalizedTime);
            UpdateAttackCollider(animator, stateInfo.normalizedTime);

            float speed = GetCombatMomentum(stateInfo.normalizedTime);
            _character.HandleCombatMovement(speed);
        }

        protected override void OnExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            animator.GetComponent<CharacterAnimator>().EnableCollider(colliderIndex, false);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Smooth momentum when starting the attack
        /// </summary>
        protected void SmoothPlayerMomentum(Character player)
        {
            float momentum = player.CurrentSpeed;
            _attackCurve = combatCurves.curves[curveIndex];
            _attackCurve.MoveKey(0, new Keyframe(_attackCurve.keys[0].time, momentum));
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
            _character.PlayerStateMachine.CanAttack = time >= attackCooldown ? true : false;
            _character.PlayerStateMachine.CanDodge = time >= dodgeCooldown ? true : false;
        }

        /// <summary>
        /// Update collider state
        /// </summary>
        private void UpdateAttackCollider(Animator animator, float time)
        {
            CharacterAnimator characterAnimator = animator.GetComponent<CharacterAnimator>();

            if (colliderIndex < 0 || characterAnimator.Hitboxs.Length > colliderIndex || !characterAnimator.Hitboxs[colliderIndex])
                return;

            if (time >= enableCollider && time <= disableCollider)
                animator.GetComponent<CharacterAnimator>().EnableCollider(colliderIndex, true);
            else
                animator.GetComponent<CharacterAnimator>().EnableCollider(colliderIndex, false);
        }

        #endregion
    }
}
