using UnityEngine;

public class Guillotine : TrapClass
{
    public override void ActivateTrap()
    {
        Debug.Log($"Trap Activé, {trapDamage}, {trapCooldown}");
    }

    public override void DeactivateTrap()
    {
        Debug.Log("Trap désactivé");
    }
}
