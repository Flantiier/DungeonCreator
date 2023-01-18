using System.Collections.Generic;
using UnityEngine;
using _Scripts.NetworkScript;

namespace _Scripts.Hitboxs_Triggers.Triggers
{
	[RequireComponent(typeof(Rigidbody))]
	public abstract class ListingTrigger<T> : NetworkMonoBehaviour
	{
		#region Variables
		protected List<T> _triggerList;
		#endregion

		#region Builts_In
		private void Awake()
		{
			_triggerList = new List<T>();
		}

		public virtual void OnTriggerEnter(Collider other)
		{
			if (!other.TryGetComponent(out T target))
				return;

			AddItem(target);
		}

        public virtual void OnTriggerExit(Collider other)
		{
            if (!other.TryGetComponent(out T target))
                return;

			RemoveItem(target);
        }
		#endregion

		#region Methods
		protected virtual void AddItem(T item)
		{
			if (_triggerList.Contains(item))
				return;

			_triggerList.Add(item);
		}

        protected virtual void RemoveItem(T item)
		{
            if (!_triggerList.Contains(item))
                return;

            _triggerList.Remove(item);
        }
		#endregion
	}
}
