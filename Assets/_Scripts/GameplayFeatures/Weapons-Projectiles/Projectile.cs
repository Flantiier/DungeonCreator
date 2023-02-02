using UnityEngine;
using Photon.Pun;
using Sirenix.OdinInspector;
using _Scripts.NetworkScript;

namespace _Scripts.GameplayFeatures.Projectiles
{
    [RequireComponent(typeof(Rigidbody))]
    public class Projectile : NetworkMonoBehaviour
    {
        #region Variables
        [TitleGroup("References")]
        [SerializeField] private Rigidbody rb;
        [SerializeField] private Collider projCollider;

        [Header("Projectile properties")]
        [SerializeField] protected float damages = 10f;
        [SerializeField] protected float speed = 10f;
        #endregion

        #region Properties
        public float Damages
        {
            get => damages;
            set
            {
                damages = value;
            }
        }
        #endregion

        #region Builts_In
        public override void OnEnable()
        {
            base.OnEnable();
            projCollider.enabled = false;
        }

        protected virtual void OnTriggerEnter(Collider other)
        {
            Destroy(gameObject);
        }
        #endregion

        #region Methods
        /// <summary>
        /// Throwing the projectile in a certain direction with a defined force
        /// </summary>
        /// <param name="direction"> Throwing direction </param>
        public virtual void ThrowProjectile(Vector3 direction)
        {
            projCollider.enabled = true;
            transform.rotation = Quaternion.LookRotation(direction, Vector3.up);
            rb.AddForce(direction * speed, ForceMode.Impulse);
        }

        /// <summary>
        /// Throwing the projectile in a certain direction with a defined force
        /// </summary>
        /// <param name="direction"> Throwing direction </param>
        public virtual void OverrideThrowForce(Vector3 direction, float force)
        {
            projCollider.enabled = true;
            transform.rotation = Quaternion.LookRotation(direction, Vector3.up);
            rb.AddForce(direction * force, ForceMode.Impulse);
        }

        /// <summary>
        /// Set a new value to projectile damages
        /// </summary>
        /// <param name="newDamages"> New damage value </param>
        public void OverrideProjectileDamages(float newDamages)
        {
            damages = newDamages;
        }
        #endregion
    }
}
