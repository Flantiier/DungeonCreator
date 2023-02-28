using UnityEngine;
using Sirenix.OdinInspector;
using _Scripts.Characters;

namespace _ScriptableObjects.Afflictions
{
	[InlineEditor]
	public class AfflictionStatus : ScriptableObject
	{
		#region Variables
		[BoxGroup("Properties"), LabelWidth(100)]
		[Range(5f, 20f), GUIColor(2, 3, 0.5f)]
        public float duration = 5f;

		public virtual void UpdateEffect(Character target) { }
        #endregion
    }
}
