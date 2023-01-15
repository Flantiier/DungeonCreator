using UnityEngine;
using Sirenix.OdinInspector;

namespace _ScriptablesObjects.Traps
{
    [CreateAssetMenu(fileName = "New Destructible Trap", menuName = "Scriptables/Traps/Destructible")]
    public class DestructibleTrapSO : TrapSO
    {
        [TitleGroup("Properties")]
        public float health = 50f;
    }
}