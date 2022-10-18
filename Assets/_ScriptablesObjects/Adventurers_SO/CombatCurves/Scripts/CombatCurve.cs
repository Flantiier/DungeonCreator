using UnityEngine;

namespace _Scriptables.Curves
{
	[CreateAssetMenu(fileName = "CombatCurves", menuName = "Scriptables/Curves")]
	public class CombatCurve : ScriptableObject
	{
		public AnimationCurve[] curves;
	}
}
