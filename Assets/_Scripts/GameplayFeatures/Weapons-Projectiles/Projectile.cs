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
        [SerializeField] protected float destructTime = 10f;
        [SerializeField] protected bool autoEnableTrail = false;

        protected Rigidbody _rb;
        protected TrailRenderer _trail;
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
            _trail = GetComponentInChildren<TrailRenderer>();

            if (!projCollider)
            {
                if (TryGetComponent(out Collider collider))
                    projCollider = collider;
            }
        }

        public override void OnEnable()
        {
            base.OnEnable();
            projCollider.enabled = enabledOnStart;
            EnableTrail(autoEnableTrail);
        }

        protected virtual void FixedUpdate()
        {
            if (!ViewIsMine())
                return;

            RPCCall("SyncTransform", RpcTarget.Others, transform.position, transform.rotation);
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

            EnableTrail(true);
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

            EnableTrail(true);
        }

        /// <summary>
        /// Set a new value to projectile damages
        /// </summary>
        /// <param name="newDamages"> New damage value </param>
        public void OverrideProjectileDamages(float newDamages)
        {
            damages = newDamages;
        }

        /// <summary>
        /// Enable projectile trail
        /// </summary>
        /// <param name="enabled"></param>
        private void EnableTrail(bool enabled)
        {
            if (!_trail)
                return;

            _trail.enabled = enabled;
        }
        #endregion

        #region RPC Methods
        [PunRPC]
        public void SyncTransform(Vector3 position, Quaternion rotation)
        {
            transform.position = position;
            transform.rotation = rotation;
        }
        #endregion
    }
}
