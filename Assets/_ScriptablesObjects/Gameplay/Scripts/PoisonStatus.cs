using UnityEngine;
using Sirenix.OdinInspector;
using _Scripts.Characters;

namespace _ScriptableObjects.Afflictions
{
	[CreateAssetMenu(fileName = "New Poison Status", menuName = "Gameplay/Afflictions/Poison")]
	public class PoisonStatus : AfflictionStatus
	{
        #region Variables
        [BoxGroup("Properties"), LabelWidth(100), Range(1f, 15)]
        [SerializeField] private float strength = 5;
        #endregion

        #region Methods
        public override void UpdateEffect(Character target)
		{
            target.DealDamage(strength * Time.deltaTime);
		}
        #endregion
    }
}
