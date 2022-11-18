using UnityEngine;
using Photon.Pun;
using _Scripts.Weapons.Projectiles;

namespace _Scripts.Characters.Animations
{
    public class WarriorAnimator : CharacterAnimator
    {
        #region Variables
        [Header("Warrior requirements")]
        [SerializeField] protected GameObject sword;
        [SerializeField] protected Transform throwPoint;
        [SerializeField] protected Projectile projectilePrefab;
        [SerializeField] private float throwOffsetFromCamera = 5f;

        private Projectile _lastProjectile;
        #endregion

        #region Properties
        public Warrior Warrior { get; private set; }
        #endregion

        #region Builts_In
        public override void Awake()
        {
            if (!ViewIsMine())
                return;

            base.Awake();
            Warrior = GetComponentInParent<Warrior>();
        }

        private void OnDrawGizmos()
        {
            try
            {
                Gizmos.DrawSphere(Character.MainCamTransform.position + Character.MainCamTransform.forward * throwOffsetFromCamera, 0.1f);
            }
            catch { }
        }
        #endregion

        #region Animation Methods
        /// <summary>
        /// Enabe or Disable the sword
        /// </summary>
        public void EnableSword(bool state)
        {
            sword.SetActive(state);
        }

        /// <summary>
        /// Enable shield collider
        /// </summary>
        /// <param name="id"> 0 => Disable, 1 => Enable </param>
        public void EnableShield(bool state)
        {
            Warrior.Shield.EnableShield(state);
        }
        #endregion

        #region Methods
        /// <summary>
        /// Creating a projectile
        /// </summary>
        public void CreateProjectile()
        {
            if (!ViewIsMine())
                return;

            Transform instance = PhotonNetwork.Instantiate(projectilePrefab.name, throwPoint.position, Quaternion.identity).transform;
            instance.SetParent(throwPoint);

            _lastProjectile = instance.GetComponent<Projectile>();
        }

        /// <summary>
        /// Lauching the projectile
        /// </summary>
        public void LaunchProjectile()
        {
            if (!ViewIsMine() || !_lastProjectile)
                return;

            _lastProjectile.transform.SetParent(null);
            _lastProjectile.transform.position = Character.MainCamTransform.position + Character.MainCamTransform.forward * throwOffsetFromCamera;
            _lastProjectile.ThrowProjectile(Character.MainCamTransform.forward);

            _lastProjectile = null;
        }
        #endregion
    }
}
