using System.Collections;
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
		[SerializeField] private Transform projectilePoint;

		[BoxGroup("Stats")]
		[Required, SerializeField] private BallistaDatas datas;

        private bool _isReloaded = true;
		#endregion

		#region Builts_In
		public override void OnEnable()
		{
			base.OnEnable();
			_isReloaded = true;
		}

		private void Update()
		{
			HandleTurretRotation();
			//HandleShootBehaviour();
		}
        #endregion

        #region Inherited Methods
        protected override void InitializeTrap()
        {
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
			horizontalPart.rotation = Quaternion.Slerp(horizontalPart.rotation, Quaternion.LookRotation(target.position - horizontalPart.position, transform.up), datas.smoothRotation);
			horizontalPart.localEulerAngles = new Vector3(0f, horizontalPart.localEulerAngles.y, 0f);
            //Vertical
            verticalPart.rotation = Quaternion.Slerp(verticalPart.rotation, Quaternion.LookRotation(target.position - verticalPart.position, transform.up), datas.smoothRotation);
            verticalPart.localEulerAngles = new Vector3(verticalPart.localEulerAngles.x, 0f, 0f);
        }

		/// <summary>
		/// Handle turret shooting
		/// </summary>
		[ContextMenu("Shoot")]
		private void HandleShootBehaviour()
		{
			/*if (!_isReloaded)
				return;*/

			Projectile projectile = Instantiate(datas.projectilePrefab, projectilePoint.position, projectilePoint.rotation);
			projectile.ThrowProjectile(projectile.transform.forward);
			//StartCoroutine("ReloadCoroutine");
		}

		private IEnumerator ReloadCoroutine()
		{
			yield return new WaitForSecondsRealtime(datas.damage);
			_isReloaded = false;
			StartCoroutine("ReloadCoroutine");
		}

        #region Animation Methods
		/// <summary>
		/// Indicates if the reload animation is finished
		/// </summary>
		public void IsReloaded()
		{
			_isReloaded = true;
		}
        #endregion

        #endregion
    }
}
