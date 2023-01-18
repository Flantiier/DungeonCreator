using UnityEngine;
using _Scripts.Characters;

namespace _Scripts.Hitboxs_Triggers.Triggers
{
    public class TrapTrigger : Trigger<Character>
    {
        protected override void OnEnterMethod(Character target)
        {
            if (!target.ViewIsMine())
                return;

            base.OnEnterMethod(target);
            Debug.Log($"My player entered in a trigger");
        }
    }
}