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
        [SerializeField] private Collider projCollider;

        [Header("Projectile properties")]
        [SerializeField] protected float damages = 10f;
        [SerializeField] protected float speed = 10f;
        [SerializeField] protected bool enabledOnStart = false;

        private Rigidbody _rb;
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
        protected virtual void Awake()
        {
            _rb = GetComponent<Rigidbody>();

            if (!projCollider)
            {
                if (TryGetComponent(out Collider collider))
                    projCollider = collider;
                else
                    Debug.LogError($"Missing collider reference on {gameObject}");
            }
        }

        public override void OnEnable()
        {
            base.OnEnable();
            projCollider.enabled = enabledOnStart;
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
            _rb.AddForce(direction * speed, ForceMode.Impulse);
        }

        /// <summary>
        /// Throwing the projectile in a certain direction with a defined force
        /// </summary>
        /// <param name="direction"> Throwing direction </param>
        public virtual void OverrideThrowForce(Vector3 direction, float force)
        {
            projCollider.enabled = true;
            transform.rotation = Quaternion.LookRotation(direction, Vector3.up);
            _rb.AddForce(direction * force, ForceMode.Impulse);
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
