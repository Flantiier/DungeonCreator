using UnityEngine;
using Photon.Pun;
using _Scripts.NetworkScript;

namespace _Scripts.Characters
{
	public class Entity : NetworkAnimatedObject
	{
		#region Properties
		public float CurrentHealth { get; protected set; }
        #endregion

        #region Health Methods
		/// <summary>
		/// Handle entity health
		/// </summary>
        protected virtual void HandleEntityHealth(float damages) { }

		/// <summary>
		/// Method executed when the health goes down to 0
		/// </summary>
		protected virtual void HandleEntityDeath() { }

		/// <summary>
		/// Clamping current health between a minimum and a maximum
		/// </summary>
		/// <param name="minValue"> Minimum value </param>
		/// <param name="maxValue"> Maximum value </param>
		protected virtual float ClampedHealth(float damages, float minValue, float maxValue)
		{
			float health = CurrentHealth - damages;
            health = Mathf.Clamp(health, minValue, maxValue);

			return health;
		}

        /// <summary>
        /// Setting entity health method
        /// </summary>
        [PunRPC]
        public virtual void HealthRPC(float healthValue)
		{
			CurrentHealth = healthValue;
        }
        #endregion
    }
}
