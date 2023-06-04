using UnityEngine;
using Photon.Pun;
using _Scripts.NetworkScript;
using _Scripts.Hitboxs_Triggers.Hitboxs;
using _Scripts.GameplayFeatures.Projectiles;
using Cinemachine.Utility;

namespace _Scripts.Characters.Animations
{
    [RequireComponent(typeof(PhotonView))]
    [RequireComponent(typeof(PhotonTransformView))]
    [RequireComponent(typeof(PhotonAnimatorView))]
    public class CharacterAnimator : NetworkMonoBehaviour
    {
        #region Variables
        [Header("Hitboxs")]
        [SerializeField] protected CharacterHitbox[] hitboxs;

        [Header("Projectile properties")]
        [SerializeField] protected Transform throwPoint;
        [SerializeField] protected Projectile projectilePrefab;
        [SerializeField] private float addOffset = 0.25f;
        [SerializeField] protected bool projectileMainAttack = false;
        
        protected Projectile _lastProjectile;
        #endregion

        #region Properties
        public Character Character { get; private set; }
        #endregion

        #region Builts_In
        public virtual void Awake()
        {
            if (!ViewIsMine())
                return;

            Character = GetComponentInParent<Character>();
            SetHitboxsReferences(Character);
        }
        #endregion

        #region AnimationsEvents Methods
        /// <summary>
        /// Enable a hitbox in the colliderArray
        /// </summary>
        /// <param name="index"> hitbox's index </param>
        public void EnableCollider(int index)
        {
            if (!ViewIsMine() || HitboxNotFound(index))
                return;

            hitboxs[index].Collider.enabled = true;
        }

        /// <summary>
        /// Disable a hitbox in the colliderArray
        /// </summary>
        /// <param name="index"> hitbox's index </param>
        public void DisableCollider(int index)
        {
            if (!ViewIsMine() || HitboxNotFound(index))
                return;

            hitboxs[index].Collider.enabled = false;
        }
        #endregion

        #region Methods

        #region Hitboxs
        /// <summary>
        /// Set the character reference in all the hitboxs in the hitbox array
        /// </summary>
        /// <param name="character"> Character reference </param>
        private void SetHitboxsReferences(Character character)
        {
            if (!character && hitboxs.Length <= 0)
                return;

            foreach(CharacterHitbox hitbox in hitboxs)
            {
                if (!hitbox)
                    continue;

                hitbox.character = character;
            }
        }

        /// <summary>
        /// Indicates if the selected index isn't out of hitbox's array bounds
        /// </summary>
        /// <param name="index"> Hitbox index </param>
        protected bool HitboxNotFound(int index)
        {
            return hitboxs.Length <= 0 || index >= hitboxs.Length || !hitboxs[index];
        }

        /// <summary>
        /// Show all hitboxs on object's children
        /// </summary>
        [ContextMenu("ShowHitboxs")]
        private void ShowHitboxs()
        {
            hitboxs = GetComponentsInChildren<CharacterHitbox>();
        }
        #endregion

        #region Projectiles
        /// <summary>
        /// Creating a projectile
        /// </summary>
        public void CreateProjectile()
        {
            if (!ViewIsMine())
                return;

            if (!projectilePrefab || !throwPoint)
            {
                Debug.LogWarning("Missing reference to launch projectile");
                return;
            }

            if (_lastProjectile)
                return;

            Transform instance = PhotonNetwork.Instantiate(projectilePrefab.name, throwPoint.position, throwPoint.rotation).transform;
            instance.SetParent(throwPoint);
            _lastProjectile = instance.GetComponent<Projectile>();
        }

        /// <summary>
        /// Lauching the projectile
        /// </summary>
        public virtual void LaunchProjectile()
        {
            if (!ViewIsMine())
                return;

            if (!_lastProjectile)
                CreateProjectile();

            _lastProjectile.transform.SetParent(null);
            _lastProjectile.OverrideProjectileDamages(Character.CharacterDatas.GetAttackDamages(projectileMainAttack));
            _lastProjectile.ThrowProjectile(Character.MainCamera.forward);
            _lastProjectile = null;
        }
        #endregion

        #endregion
    }
}
