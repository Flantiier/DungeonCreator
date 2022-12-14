using UnityEngine;
using _Scripts.Characters;

namespace _Scripts.GameplayFeatures.Afflictions
{
	public class AfflictionStatus : ScriptableObject
	{
		[SerializeField] private float duration = 5f;

		public float Duration => duration;

		public virtual void UpdateEffect(Character target) { }
	}
}
