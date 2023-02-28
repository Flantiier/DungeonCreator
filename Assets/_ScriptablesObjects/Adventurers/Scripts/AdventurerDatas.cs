using UnityEngine;
using Sirenix.OdinInspector;

namespace _ScriptableObjects.Adventurers
{
    [CreateAssetMenu(fileName = "New CharacterDatas", menuName = "Scriptables/Adventurers/Character Datas")]
    [InlineEditor]
    public class AdventurerDatas : ScriptableObject
    {
        #region Variables
        [BoxGroup("Properties"), LabelWidth(100)]
        [Range(50f, 150f), GUIColor(0.5f, 3f, 0.5f)]
        public float health = 100f;
        [BoxGroup("Properties"), LabelWidth(100)]
        [Range(50f, 150f), GUIColor(3, 2, 0.8f)]
        public float stamina = 100f;

        [BoxGroup("Skill&Combat"), LabelWidth(110)]
        [Range(5f, 50f), GUIColor(0.5f, 2, 1)]
        public float damageAbsorber = 25f;
        [BoxGroup("Skill&Combat"), LabelWidth(110)]
        [Range(5f, 20f), GUIColor(0.8f, 2, 2)]
        public float skillCooldown = 10f;
        [BoxGroup("Skill&Combat"), LabelWidth(110)]
        public RandomAttack mainAttack;
        [BoxGroup("Skill&Combat"), LabelWidth(110)]
        public RandomAttack secondaryAttack;
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
    [SerializeField, Range(5f, 30f), GUIColor(3, 0.5f, 0.3f)]
    private int damages = 20;
    [SerializeField, Range(0, 1), GUIColor(3, 3, 0.5f)]
    private float critRate = 0.25f;
    [SerializeField, Range(2, 10), GUIColor(0.5f, 1, 2)]
    private float critBoost = 5f;

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
