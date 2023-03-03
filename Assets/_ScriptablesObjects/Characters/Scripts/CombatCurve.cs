using UnityEngine;

namespace _ScriptableObjects.Curves
{
	[CreateAssetMenu(fileName = "CombatCurves", menuName = "Scriptables/Curves")]
	public class CombatCurve : ScriptableObject
	{
		public AnimationCurve[] curves;
	}
}
