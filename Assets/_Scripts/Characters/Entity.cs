using UnityEngine;
using Photon.Pun;
using _Scripts.NetworkScript;

namespace _Scripts.Characters
{
	public class Entity : NetworkMonoBehaviour
	{
		#region Properties
		public float CurrentHealth { get; set; }
        #endregion

        #region Health Methods
		/// <summary>
		/// Handle entity health
		/// </summary>
        protected virtual void HandleHealth(float damages) {}

		/// <summary>
		/// Method executed when the health goes down to 0
		/// </summary>
		protected virtual void HandleEntityDeath() {}

        /// <summary>
        /// Setting entity health method
        /// </summary>
		[PunRPC]
        public void HealthRPC(float healthValue)
		{
			CurrentHealth = healthValue;
        }
        #endregion
    }
}
