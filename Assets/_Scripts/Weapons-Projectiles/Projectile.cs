using UnityEngine;
using Photon.Pun;
using _Scripts.NetworkScript;

namespace _Scripts.Weapons.Projectiles
{
	[RequireComponent(typeof(Rigidbody))]
    public class Projectile : NetworkMonoBehaviour
	{
		#region Variables
		[Header("Projectile properties")]
		[SerializeField] protected float damages = 10f;
		[SerializeField] protected float speed = 10f;

		protected Rigidbody _rb;
		protected Collider _collider;
		#endregion

		#region Builts_In
		public virtual void Awake()
		{
			if (!ViewIsMine())
				return;

			_rb = GetComponent<Rigidbody>();
			_collider = GetComponent<Collider>();
			_collider.enabled = false;
		}

		private void OnTriggerEnter(Collider other)
        {
            if (!ViewIsMine())
                return;

            HandleCollision(other);
		}
		#endregion

		#region Methods
		/// <summary>
		/// Throwing the projectile
		/// </summary>
		/// <param name="direction"> Throwing direction </param>
		public void ThrowProjectile(Vector3 direction)
		{
			_collider.enabled = true;
			transform.rotation = Quaternion.LookRotation(direction, Vector3.up);
			_rb.AddForce(direction * speed, ForceMode.Impulse);
		}

		/// <summary>
		/// Executed during OnTriggerEnter, Projectile's behaviour on collision
		/// </summary>
		protected virtual void HandleCollision(Collider other)
		{
			PhotonNetwork.Destroy(gameObject);
		}
		#endregion
	}
}
