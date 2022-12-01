using UnityEngine;

namespace _Scripts.Characters.Animations.StateMachines
{
    public class EnableLayers : NetworkStateMachine
    {
        #region Inherited Methods
        protected override void StateMachineEnter(Animator animator, int stateMachinePathHash)
        {
            MyCharacter.PlayerSM.CurrentState = Characters.StateMachines.PlayerStateMachine.PlayerStates.Walk;
            MyCharacter.PlayerSM.EnableLayers = true;
        }
        #endregion
    }
}
