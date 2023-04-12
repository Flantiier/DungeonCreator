using UnityEngine;
using Sirenix.OdinInspector;

namespace _ScriptableObjects.Characters
{
    [CreateAssetMenu(fileName = "New Adventurer Properties", menuName = "Characters/Adventurer Properties")]
    [InlineEditor]
    public class AdventurerProperties : ScriptableObject
    {
        #region Variables
        [BoxGroup("Properties"), LabelWidth(100), Range(50, 200), GUIColor(0, 2, 0.5f)]
        public float health = 100;
        [BoxGroup("Properties"), LabelWidth(100), Range(50, 200), GUIColor(3, 2, 0.5f)]
        public float stamina = 100;

        [BoxGroup("Skill&Combat", LabelText = "Skill & Combat"), LabelWidth(110), Range(5, 50), GUIColor(2.5f, 1, 0.25f)]
        public float damageAbsorber = 25;
        [BoxGroup("Skill&Combat"), LabelWidth(110), Range(5, 60), GUIColor(2.5f, 1, 0.25f)]
        public float skillCooldown = 10;
        [BoxGroup("Skill&Combat"), LabelWidth(110), GUIColor(0, 1, 1.5f)]
        public RandomAttack mainAttack;
        [BoxGroup("Skill&Combat"), LabelWidth(110), GUIColor(1, 1, 2)]
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
    [SerializeField, Range(5, 100)]
    private int damages = 20;
    [SerializeField, Range(0, 1)]
    private float critRate = 0.25f;
    [SerializeField, Range(1, 40)]
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
