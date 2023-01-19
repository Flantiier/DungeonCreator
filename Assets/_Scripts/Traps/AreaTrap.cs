using UnityEngine;
using Sirenix.OdinInspector;
using _Scripts.Characters;
using _Scripts.Hitboxs_Triggers.Triggers;

namespace _Scripts.GameplayFeatures.Traps
{
	public class AreaTrap : TrapClass1
	{
		#region Variables
		[TitleGroup("References")]
		[SerializeField] private ListingTrigger<Character> trigger;
		#endregion

		#region Builts_In
		private void Update()
		{
			if (!ViewIsMine())
				return;

			HandleAreaTrapBehaviour();
		}
		#endregion

		#region Methods
		/// <summary>
		/// Trap behaviour method
		/// </summary>
		protected virtual void HandleAreaTrapBehaviour()
		{
			if(trigger.List.Count <= 0)
			{
				Debug.Log("No one in the trigger");
				return;
			}

			Debug.Log("Atleast one in the trigger");
		}
		#endregion
	}
}
