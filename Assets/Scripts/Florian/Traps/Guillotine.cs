using UnityEngine;

public class Guillotine : TrapClass
{
    public override void ActivateTrap()
    {
        Debug.Log($"Trap Activ�, {trapDamage}, {trapCooldown}");
    }

    public override void DeactivateTrap()
    {
        Debug.Log("Trap d�sactiv�");
    }
}
