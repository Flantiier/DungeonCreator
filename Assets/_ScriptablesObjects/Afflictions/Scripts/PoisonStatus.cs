using UnityEngine;
using _Scripts.Characters;

namespace _ScriptableObjects.Afflictions
{
	[CreateAssetMenu(fileName = "New Poison Status", menuName = "Scriptables/Afflictions/Poison")]
	public class PoisonStatus : AfflictionStatus
	{
        [SerializeField] private float strength = 5f;

		public override void UpdateEffect(Character target)
		{
            target.DealDamage(strength * Time.deltaTime);
		}
	}
}
