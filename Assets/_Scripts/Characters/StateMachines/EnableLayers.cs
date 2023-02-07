using UnityEngine;

namespace _Scripts.Characters.Animations.StateMachines
{
    public class EnableLayers : CharacterStateMachine
    {
        #region Inherited Methods
        protected override void StateMachineEnter(Animator animator, int stateMachinePathHash)
        {
            Character.PlayerSM.CurrentState = Characters.StateMachines.PlayerStateMachine.PlayerStates.Walk;
            Character.PlayerSM.EnableLayers = true;
        }
        #endregion
    }
}
