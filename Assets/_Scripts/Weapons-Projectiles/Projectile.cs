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
		[SerializeField] private float speed = 10f;

		private Rigidbody _rb;
		private Collider _collider;
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
			HandleCollision();
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
		protected virtual void HandleCollision()
		{
			if (!ViewIsMine())
				return;

			PhotonNetwork.Destroy(gameObject);
		}
		#endregion
	}
}
