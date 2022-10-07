using UnityEngine;
using System;
using Photon.Pun;

namespace Adventurer
{
	public class EventsTriggerer : MonoBehaviour
	{
        #region Variables
        [Header("Events Reference")]
		[SerializeField, Tooltip("Referencing the EventListener comp on the player")]
		private EventsListener Listener;

		//PView comp
		private PhotonView _view;
        #endregion

        #region Builts-In
        private void Awake()
        {
			_view = GetComponent<PhotonView>();
        }
        #endregion

        #region Events Calling Methods
        //Raise Fall event
        public void RaiseFall() { if (!_view.IsMine) return; Listener.onFall.RaiseEvent(); }

		//Raise Land event
		public void RaiseLand() { if (!_view.IsMine) return; Listener.onLand.RaiseEvent(); }
		
		//Raise StartDodge event
		public void RaiseStartDodge() { if (!_view.IsMine) return; Listener.onStartDodge.RaiseEvent(); }

		//Raise MiddleDodge event
		public void RaiseMiddleDodge() { if (!_view.IsMine) return; Listener.onMiddleDodge.RaiseEvent(); }

		//Raise EndDodge event
		public void RaiseEndDodge() { if (!_view.IsMine) return; Listener.onEndDodge.RaiseEvent(); }

		//Raise RaiseStartAttack event
		public void RaiseStartAttack() { if (!_view.IsMine) return; Listener.onStartAttack.RaiseEvent(); }

		//Raise MiddleAttack event
		public void RaiseMiddleAttack() { if (!_view.IsMine) return; Listener.onMiddleAttack.RaiseEvent(); }

		//Raise EndAttack event
		public void RaiseEndAttack() { if (!_view.IsMine) return; Listener.onEndAttack.RaiseEvent(); }

		//Raise StartAim event
		public void RaiseStartAim() { if (!_view.IsMine) return; Listener.onStartAim.RaiseEvent(); }

		//Raise MiddleAim event
		public void RaiseMiddleAim() { if (!_view.IsMine) return; Listener.onMiddleAim.RaiseEvent(); }

		//Raise EndAim event
		public void RaiseEndAim() { if (!_view.IsMine) return; Listener.onEndAim.RaiseEvent(); }

		//Raise StartAbility event
		public void RaiseStartAbility() { if (!_view.IsMine) return; Listener.onStartAbility.RaiseEvent(); }

		//Raise MiddleAbility event
		public void RaiseMiddleAbility() { if (!_view.IsMine) return; Listener.onMiddleAbility.RaiseEvent(); }

		//Raise EndAbility event
		public void RaiseEndAbility() { if (!_view.IsMine) return; Listener.onEndAbility.RaiseEvent(); }
        #endregion
    }
}
