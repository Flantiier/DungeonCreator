using UnityEngine;

namespace _ScriptablesObjects.Adventurers
{
    [CreateAssetMenu(fileName = "New AdventurerDatas", menuName = "Scriptables/Adventurers/BasicAdventurer")]
    public class AdventurerDatas : ScriptableObject
    {
        #region Variables
        [Header("Character properties")]
        public float health = 100f;
        public float stamina = 100f;

        [Header("Skill & Abilities")]
        public float skillCooldown = 10f;

        [Header("Combat stats")]
        public int attack = 20;
        [Range(0f, 1f)] public float critAttackRate = 0.25f;
        public float critAttackBoost = 5f;
        public float defense = 5f;
        #endregion

        #region Methods
        /// <summary>
        /// Return random attacks damages
        /// </summary>
        public int GetAttackDamages()
        {
            float rate = Random.Range(0f, 1f);
            int finalAttack = rate > critAttackRate ? attack : Mathf.RoundToInt(attack + Random.Range(0f, critAttackBoost));

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
