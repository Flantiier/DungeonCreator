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
		public string afflictionName;
        [BoxGroup("Properties"), LabelWidth(100), Range(5, 60)]
        public float duration = 5f;

		public virtual void UpdateEffect(Character target) { }

		public virtual void EnableEffect(Character target, bool enabled)
		{
			target.ShowAfflictionVFX(afflictionName, enabled);
		}
        #endregion
    }
}
