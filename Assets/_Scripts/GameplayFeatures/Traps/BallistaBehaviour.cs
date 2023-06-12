using UnityEngine;
using Sirenix.OdinInspector;
using _Scripts.GameplayFeatures.PhysicsAdds;
using _Scripts.GameplayFeatures.Projectiles;
using _ScriptableObjects.Traps;

namespace _Scripts.GameplayFeatures.Traps
{
	public class BallistaBehaviour : DestructibleTrap
	{
		#region Variables
        [FoldoutGroup("References")]
        [SerializeField] private Transform horizontalPart;
		[FoldoutGroup("References")]
        [SerializeField] private Transform verticalPart;
		[FoldoutGroup("References")]
		[SerializeField] private SphericalFOV fov;
		[FoldoutGroup("References")]
		[SerializeField] private Transform projectileParent;

		[BoxGroup("Stats")]
		[SerializeField, Range(0.2f, 1.5f)] 
		private float Yoffset = 1f;
		[BoxGroup("Stats")]
		[Required, SerializeField] private BallistaProperties datas;

		private Projectile _lastProjectile;
		private bool _isReloaded;
		#endregion

		#region Builts_In
		public override void OnDisable()
		{
			base.OnDisable();

			if (_lastProjectile)
				Destroy(_lastProjectile);
		}

		protected override void Update()
		{
			base.Update();

			HandleTurretRotation();
			HandleShootingBehaviour();
			HideTurret();
		}
        #endregion

        #region Inherited Methods
        protected override void InitializeTrap()
        {
            _isReloaded = false;

            if (!ViewIsMine())
                return;

            SetTrapHealth(datas.health);
        }
        #endregion

        #region Methods
        /// <summary>
        /// Handle the rotation of the 2 parts of the turret
        /// </summary>
        private void HandleTurretRotation()
		{
			if (!fov || !fov.IsDetecting())
				return;

			Transform target = fov.GetTargetInFOV();

			if (!target)
				return;

			//Horizontal
			Vector3 targetX = target.position - horizontalPart.position + new Vector3(0f, Yoffset, 0f);
			Vector3 targetY = target.position - verticalPart.position + new Vector3(0f, Yoffset, 0f);
            horizontalPart.rotation = Quaternion.Slerp(horizontalPart.rotation, Quaternion.LookRotation(targetX, transform.up), datas.smoothRotation);
			horizontalPart.localEulerAngles = new Vector3(0f, horizontalPart.localEulerAngles.y, 0f);
            //Vertical
            verticalPart.rotation = Quaternion.Slerp(verticalPart.rotation, Quaternion.LookRotation(targetY, transform.up), datas.smoothRotation);
            verticalPart.localEulerAngles = new Vector3(verticalPart.localEulerAngles.x, 0f, 0f);
        }

		/// <summary>
		/// Handle the behaviour to shoot projectiles
		/// </summary>
		private void HandleShootingBehaviour()
		{
			if (!_isReloaded || !fov.IsDetecting())
				return;

			Animator.SetTrigger("Shoot");
			_isReloaded = false;
		}

		/// <summary>
		/// Indicates that the ballista is reloaded
		/// </summary>
		public void BallistaReloaded()
		{
			_isReloaded = true;
		}

		/// <summary>
		/// Create a new projectile
		/// </summary>
		public void InstantiateProjectile()
        {
			if (_lastProjectile)
				return;

            _lastProjectile = Instantiate(datas.projectilePrefab, projectileParent);
        }

		/// <summary>
		/// Launch the last instantiated projectile
		/// </summary>
		public void LaunchProjectile()
		{
			if (!_lastProjectile)
				return;

			if (!_lastProjectile.gameObject.activeSelf)
				_lastProjectile.gameObject.SetActive(true);

			_lastProjectile.transform.SetParent(null);
			_lastProjectile.OverrideThrowForce(_lastProjectile.transform.forward, datas.throwForce);
			_lastProjectile = null;
		}

		/// <summary>
		/// Hide the turret and its projectile when nothing si detected
		/// </summary>
		private void HideTurret()
		{
			SetVisbility(fov.IsDetecting());

			if (_lastProjectile)
				_lastProjectile.gameObject.SetActive(false);
		}
        #endregion
    }
}
