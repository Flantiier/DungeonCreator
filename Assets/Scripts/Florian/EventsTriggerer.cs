using UnityEngine;
using System;

namespace Adventurer
{
	public class EventsTriggerer : MonoBehaviour
	{
		[SerializeField] private EventsListener Listener;

		public void RaiseFall() { Listener.onFall.RaiseEvent(); }
		public void RaiseLand() { Listener.onLand.RaiseEvent(); }
		public void RaiseStartDodge() { Listener.onStartDodge.RaiseEvent(); }
		public void RaiseMiddleDodge() { Listener.onMiddleDodge.RaiseEvent(); }
		public void RaiseEndDodge() { Listener.onEndDodge.RaiseEvent(); }
		public void RaiseStartAttack() { Listener.onStartAttack.RaiseEvent(); }
		public void RaiseMiddleAttack() { Listener.onMiddleAttack.RaiseEvent(); }
		public void RaiseEndAttack() { Listener.onEndAttack.RaiseEvent(); }
		public void RaiseStartAim() { Listener.onStartAim.RaiseEvent(); }
		public void RaiseMiddleAim() { Listener.onMiddleAim.RaiseEvent(); }
		public void RaiseEndAim() { Listener.onEndAim.RaiseEvent(); }
		public void RaiseStartAbility() { Listener.onStartAbility.RaiseEvent(); }
		public void RaiseMiddleAbility() { Listener.onMiddleAbility.RaiseEvent(); }
		public void RaiseEndAbility() { Listener.onEndAbility.RaiseEvent(); }
	}
}
