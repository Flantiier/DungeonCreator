using UnityEngine;

namespace _ScriptableObjects.Characters
{
	[CreateAssetMenu(fileName = "New Animations Curves", menuName = "Characters/Animation Curves")]
	public class CombatCurve : ScriptableObject
	{
		public AnimationCurve[] curves;
	}
}
