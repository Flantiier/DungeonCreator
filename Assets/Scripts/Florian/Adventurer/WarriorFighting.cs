using UnityEngine;
using System.Collections;
using Photon.Pun;

namespace Adventurer
{
	public class WarriorFighting : CharacterFighting, IDefending
	{
        #region Warrior Variables
        [Header("Warrior Fighting Variables")]
        [SerializeField, Tooltip("Minimum Defense angle value")]
        private float minDefenseAngle;

        [SerializeField, Tooltip("Maximum Defense angle value")]
        private float maxDefenseAngle;

        //Differents combatState
        public enum CombatState
        {
            Attacking, Defending
        }
        //Current combat state
        public CombatState combatState { get; private set; }
        #endregion

        #region Warrior Combats Methods
        /// <summary>
        /// If the player defends, chck hit angle (triggered on hit)
        /// </summary>
        public void DefenseHit()
        {
            Debug.Log("coucou");
        }

        /// <summary>
        /// Setting Attacking State
        /// </summary>
        public void AttackState()
        {
            combatState = CombatState.Attacking;
        }

        /// <summary>
        /// Setting Defense State
        /// </summary>
        public void DefenseState()
        {
            combatState = CombatState.Defending;
        }
        #endregion
    }
}