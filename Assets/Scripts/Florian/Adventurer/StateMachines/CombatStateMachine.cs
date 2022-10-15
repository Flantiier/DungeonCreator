using UnityEngine;
using _Scripts.Characters.StateMachines;
using _Scriptables.Curves;
using Photon.Realtime;

namespace _Scripts.Characters.Animations.StateMachines
{
    public class CombatStateMachine : NetworkStateMachine
    {
        [SerializeField] protected float attackCooldown = 0.7f;
        [SerializeField] protected CombatCurve combatCurves;
        [SerializeField] protected int curveIndex;
        protected Character Character { get; set; }
        protected AnimationCurve _attackCurve;

        protected override void OnEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            Character player = CharacterAnimation.GetPlayer(animator);
            Character = player;

            SmoothPlayerMomentum(player);
            TurnPlayer(player);

            player.PlayerStateMachine.CurrentState = PlayerStateMachine.PlayerStates.Attack;
            Character.PlayerStateMachine.CanAttack = false;
        }

        protected override void OnUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            DisableActions(stateInfo.normalizedTime);

            float speed = GetCombatMomentum(stateInfo.normalizedTime);
            Character.HandleCombatMovement(speed);
        }

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
        /// Turn the player at the attack start
        /// </summary>
        protected void TurnPlayer(Character player)
        {
            float angle = Mathf.Atan2(player.InputsVector.x, player.InputsVector.y) * Mathf.Rad2Deg + player.orientation.eulerAngles.y;
            player.Mesh.rotation = Quaternion.Euler(0f, angle, 0f);

            player.OverrideDir = new Vector2(player.Mesh.forward.x, player.Mesh.forward.z);
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
            if (time >= attackCooldown)
                Character.PlayerStateMachine.CanAttack = true;
            else
                Character.PlayerStateMachine.CanAttack = false;
        }
    }
}
