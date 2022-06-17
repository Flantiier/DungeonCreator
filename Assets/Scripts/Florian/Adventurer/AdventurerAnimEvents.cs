using UnityEngine;
using System;

namespace Adventurer
{
	public class AdventurerAnimEvents : MonoBehaviour
	{
		//Dodge
		public event Action onStartDodge;
		public event Action onMiddleDodge;
		public event Action onEndDodge;

		//Attack
		public event Action onStartAttack;
		public event Action onMiddleAttack;
		public event Action onEndAttack;

		//Falling
		public event Action OnFall;
		public event Action OnLand;

		public void RaiseStartAttack() { onStartAttack?.Invoke(); }

		public void RaiseMiddleAttack()	{ onMiddleAttack?.Invoke(); }

		public void RaiseEndAttack() { onEndAttack?.Invoke(); }

		public void RaiseStartDodge() { onStartDodge?.Invoke(); }

		public void RaiseMiddleDodge() { onMiddleDodge?.Invoke(); }

		public void RaiseEndDodge() { onEndDodge?.Invoke(); }

		public void RaiseOnFall() { OnFall?.Invoke(); }
		public void RaiseOnLand() { OnLand?.Invoke(); }
	}
}
