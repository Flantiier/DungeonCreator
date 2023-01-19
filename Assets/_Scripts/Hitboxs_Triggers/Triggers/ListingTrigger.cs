using System.Collections.Generic;
using UnityEngine;
using _Scripts.NetworkScript;

namespace _Scripts.Hitboxs_Triggers.Triggers
{
	[RequireComponent(typeof(Rigidbody))]
	public abstract class ListingTrigger<T> : NetworkMonoBehaviour
	{
		#region Variables
		public List<T> List { get; private set; }
		#endregion

		#region Builts_In
		private void Awake()
		{
			List = new List<T>();
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
			if (List.Contains(item))
				return;

			List.Add(item);
		}

        protected virtual void RemoveItem(T item)
		{
            if (!List.Contains(item))
                return;

            List.Remove(item);
        }
		#endregion
	}
}
