using UnityEngine;

namespace _SciptablesObjects.Adventurer
{
    [CreateAssetMenu(fileName = "New AdventurerDatas", menuName = "Scriptables/Adventurers/BasicAdventurer")]
    public class AdventurerDatas : ScriptableObject
    {
        #region Variables
        [Header("Character properties")]
        public float health = 100f;
        public float stamina = 100f;

        [Header("Combat stats")]
        public float attack = 20f;
        [Range(0f, 1f)] public float critAttackRate = 0.25f;
        public float critAttackBoost = 5f;
        public float defense = 5f;
        #endregion

        #region Methods
        /// <summary>
        /// Return random attacks damages
        /// </summary>
        public float GetAttackDamages()
        {
            float rate = Random.Range(0f, 1f);
            float finalAttack = critAttackRate > rate ? attack : Random.Range(attack - critAttackBoost, attack + critAttackBoost);

            Debug.Log(finalAttack);
            return finalAttack;
        }

        /// <summary>
        /// Return the incoming damages affected by the defense system
        /// </summary>
        public float GetDefenseDamages(float damages)
        {
            float finalDefense = damages - (defense / 100 * damages);

            Debug.Log(finalDefense);
            return finalDefense;
        }
        #endregion
    }
}
