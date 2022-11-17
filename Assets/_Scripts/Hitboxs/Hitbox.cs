using UnityEngine;
using _Scripts.NetworkScript;

namespace _Scripts.Hitboxs
{
    public class Hitbox : NetworkMonoBehaviour
    {
        #region Variables
        protected Collider _collider;

        public Collider Collider => _collider;
        #endregion

        #region Builts_In
        public virtual void Awake()
        {
            _collider = GetComponent<Collider>();
        }

        public void OnTriggerEnter(Collider other)
        {
            if (!ViewIsMine())
                return;

            TriggerEnter(other);
        }
        #endregion

        #region Inherited methods
        /// <summary>
        /// Method called during OnTriggerEnter
        /// </summary>
        protected virtual void TriggerEnter(Collider other) { }
        #endregion
    }
}
