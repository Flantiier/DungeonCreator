using System.Collections;
using UnityEngine;
using UnityEngine.VFX;
using Sirenix.OdinInspector;
using Photon.Pun;

namespace _Scripts.GameplayFeatures.Traps
{
	public class FlameThrowerBehaviour : DamagingTrap
	{
        #region Variables
        [TitleGroup("FlameThrower properties")]
        [SerializeField] private float sprayDuration = 5f;
        [SerializeField] private float waitTime = 5f;
        [SerializeField] private VisualEffect fx;
		[SerializeField] private Transform rayStart;
		[SerializeField] private float maxDistance = 2.5f;
		[SerializeField] private float detectRadius = 0.5f;
        [SerializeField] private LayerMask rayMask;

        private float _rayLength;
        #endregion

        #region Builts_In
        private void Start()
        {
            if (!ViewIsMine())
                return;

            StartCoroutine("ThrowRoutine");
        }
        #endregion

        #region Methods
        /// <summary>
        /// Shoot a ray and detect is smth is colliding towards the flame point
        /// </summary>
        private void ShootRayTowards()
        {
            Vector3 start = rayStart.position - rayStart.forward * (detectRadius);
            float finalDistance = maxDistance + detectRadius;

            Ray ray = new Ray(start, rayStart.forward);
            Debug.DrawRay(rayStart.position, ray.direction * finalDistance, Color.cyan);

            if (Physics.SphereCast(ray, detectRadius, out RaycastHit hit, finalDistance, rayMask, QueryTriggerInteraction.Collide))
            {
                _rayLength = Vector3.Distance(rayStart.position, hit.point);
                Debug.DrawRay(rayStart.position, ray.direction * _rayLength, Color.red);
            }
        }

        /// <summary>
        /// Starts vfx and enable the hitbox
        /// </summary>
        [PunRPC]
        private void ThrowFlamesRPC()
        {
            fx.Play();
            hitbox.EnableCollider(true);
        }

        /// <summary>
        /// /// Stop vfx and disable the hitbox
        /// </summary>
        [PunRPC]
        private void StopFlamesRPC()
        {
            fx.Stop();
            hitbox.EnableCollider(false);
        }

        private IEnumerable ThrowRoutine()
        {
            RPCCall("ThrowFlamesRPC", RpcTarget.All);
            yield return new WaitForSecondsRealtime(sprayDuration);

            RPCCall("StopFlamesRPC", RpcTarget.All);
            yield return new WaitForSecondsRealtime(waitTime);

            StartCoroutine("ThrowRoutine");
        }
        #endregion
    }
}
