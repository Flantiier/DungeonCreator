using UnityEngine;
using Sirenix.OdinInspector;
using _Scripts.Characters;
using _Scripts.Hitboxs_Triggers.Triggers;

namespace _Scripts.GameplayFeatures.Traps
{
    public class TriggeredTrap : TrapClass1
    {
        #region Variables
        [TitleGroup("References")]
        [SerializeField] protected Trigger<Character> trigger;
        #endregion
    }
}