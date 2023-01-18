using System;
using UnityEngine;
using _Scripts.NetworkScript;

namespace _Scripts.Hitboxs_Triggers.Triggers
{
    [RequireComponent(typeof(Rigidbody))]
    public abstract class Trigger<T> : NetworkMonoBehaviour
    {
        #region Variables
        public event Action<T> OnTriggered;
        #endregion

        #region Builts_In
        private void Awake()
        {
            if (!ViewIsMine())
                gameObject.SetActive(false);
        }

        public virtual void OnTriggerEnter(Collider other)
        {
            if (!ViewIsMine() || !other.TryGetComponent(out T target))
                return;

            OnEnterMethod(target);
        }
        #endregion

        #region Methods
        /// <summary>
        /// Executed when the object entering in the trigger is of type T
        /// </summary>
        protected virtual void OnEnterMethod(T target)
        {
            OnTriggered?.Invoke(target);
        }
        #endregion
    }
}