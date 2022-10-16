using UnityEngine;
using _Scripts.Characters.StateMachines;
using UnityEditorInternal;

namespace _Scripts.Characters.Animations.StateMachines
{
    public class MotionSMBehaviour : NetworkStateMachine
    {
        protected override void OnEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            Character player = CharacterAnimation.GetPlayer(animator);

            player.PlayerStateMachine.CurrentState = PlayerStateMachine.PlayerStates.Walk;
            player.PlayerStateMachine.CanAttack = true;
        }

        protected override void OnUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            Character player = CharacterAnimation.GetPlayer(animator);

            if (player.Inputs.actions["Run"].IsPressed())
                player.UseStamina( player.StaminaToRun * Time.deltaTime);
        }
    }
}