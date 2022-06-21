using UnityEngine;
using System.Collections;

namespace Adventurer
{
	public class WarriorFighting : CharacterFighting, IDefending
	{
        [SerializeField] private float minDefenseAngle;
        [SerializeField] private float maxDefenseAngle;

        public enum CombatState
        {
            Attacking, Defending
        }
        public CombatState combatState { get; private set; }

        public void DefenseHit()
        {
            Debug.Log("coucou");
        }

        public void AttackState()
        {
            combatState = CombatState.Attacking;
        }

        public void DefenseState()
        {
            combatState = CombatState.Defending;
        }
    }
}
