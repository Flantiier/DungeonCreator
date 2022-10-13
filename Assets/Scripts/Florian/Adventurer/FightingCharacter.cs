using UnityEngine;
using UnityEngine.InputSystem;

public class FightingCharacter : PlayerController
{
    protected override void SubscribeToInputs()
    {
        base.SubscribeToInputs();

        //Attack
        _inputs.actions["Attack"].started += TriggerAttack;
    }

    protected override void UnsubscribeToInputs()
    {
        base.UnsubscribeToInputs();

        //Attack
        _inputs.actions["Attack"].started -= TriggerAttack;
    }

    //Condition to the player attack
    protected bool AttackCondition()
    {
        if (_currentGroundState == GroundStates.Grounded)
            return true;

        return false;
    }

    private void TriggerAttack(InputAction.CallbackContext ctx)
    {
        if (!AttackCondition())
            return;

        //Set anim
        _animator.SetTrigger("Attack");
    }
}
