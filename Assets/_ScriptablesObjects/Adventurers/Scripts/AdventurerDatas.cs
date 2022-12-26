using UnityEngine;

namespace _ScriptableObjects.Adventurers
{
    [CreateAssetMenu(fileName = "New CharacterDatas", menuName = "Scriptables/Adventurers/Character Datas")]
    public class AdventurerDatas : ScriptableObject
    {
        #region Variables
        [Header("Character properties")]
        public float health = 100f;
        public float stamina = 100f;

        [Header("Skill & Abilities")]
        public float skillCooldown = 10f;

        [Header("Combat stats")]
        public RandomAttack mainAttack;
        public RandomAttack secondaryAttack;
        [Range(0f, 100f)] public float damageAbsorber = 25f;
        #endregion

        #region Methods
        public float GetAttackDamages(bool main)
        {
            if (!main)
                return secondaryAttack.GetAttackDamages();

            return mainAttack.GetAttackDamages();
        }

        /// <summary>
        /// Return the incoming damages affected by the defense system
        /// </summary>
        public float GetDefenseDamages(float damages)
        {
            float finalDamages = damages - (damageAbsorber * damages);

            return finalDamages;
        }
        #endregion
    }
}

#region RandomAttack_Class
[System.Serializable]
public class RandomAttack
{
    [SerializeField] private int damages = 20;
    [SerializeField, Range(0f, 1f)] private float critRate = 0.25f;
    [SerializeField] private float critBoost = 5f;

    /// <summary>
    /// Return random attacks damages
    /// </summary>
    public int GetAttackDamages()
    {
        float rate = Random.Range(0f, 1f);
        int finalAttack = rate > critRate ? damages : Mathf.RoundToInt(damages + Random.Range(0f, critBoost));

        return finalAttack;
    }
}
#endregion
