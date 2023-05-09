using UnityEngine;

namespace _ScriptableObjects.Characters
{
	[CreateAssetMenu(fileName = "New Animations Curves", menuName = "SO/Characters/Animation Curves")]
	public class CombatCurve : ScriptableObject
	{
		public AnimationCurve[] curves;
	}
}
