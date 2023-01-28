using UnityEngine;
using Sirenix.OdinInspector;
using _Scripts.Characters;

namespace _ScriptableObjects.Afflictions
{
	[CreateAssetMenu(fileName = "New Poison Status", menuName = "Scriptables/Afflictions/Poison")]
	public class PoisonStatus : AfflictionStatus
	{
        #region Variables
        [BoxGroup("Properties"), LabelWidth(100)]
        [Range(1f, 8f), GUIColor(2, 0.6f, 0.6f)]
        [SerializeField] private float strength = 5f;
        #endregion

        #region Methods
        public override void UpdateEffect(Character target)
		{
            target.DealDamage(strength * Time.deltaTime);
		}
        #endregion
    }
}
