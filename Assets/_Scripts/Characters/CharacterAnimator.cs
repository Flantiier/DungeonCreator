using UnityEngine;
using _Scripts.Characters;
using _Scripts.Characters.Animations;
using _Scripts.NetworkScript;
using _Scripts.Weapons.Projectiles;
using Photon.Pun;

namespace _Scripts.Characters.Animations
{
    public class CharacterAnimator : NetworkMonoBehaviour
    {
        #region Variables
        [Header("Animations requirements")]
        [SerializeField] protected Transform throwPoint;
        [SerializeField] protected Projectile projectilePrefab;
        [SerializeField] private float throwOffsetFromCamera = 5f;

        private Projectile _lastProjectile;

        [Header("Hitboxs")]
        [SerializeField] protected CharacterHitbox[] hitboxs;
        #endregion

        #region Properties
        public Character Character { get; private set; }
        #endregion

        #region Builts_In
        public override void Awake()
        {
            SetView(transform.root.gameObject);

            if (!ViewIsMine())
                return;

            Character = GetComponentInParent<Character>();
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

        #region AnimationsEvents Methods
        /// <summary>
        /// Enable a hitbox in the colliderArray
        /// </summary>
        /// <param name="index"> hitbox's index </param>
        public void EnableCollider(int index)
        {
            if (!ViewIsMine() || HitboxNotFound(index))
                return;

            hitboxs[index].gameObject.SetActive(true);
        }

        /// <summary>
        /// Disable a hitbox in the colliderArray
        /// </summary>
        /// <param name="index"> hitbox's index </param>
        public void DisableCollider(int index)
        {
            if (!ViewIsMine() || HitboxNotFound(index))
                return;

            hitboxs[index].gameObject.SetActive(false);
        }

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

        #region Methods
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
    }
}

#region AdventurerStatic
public static class CharacterAnimation
{
    /// <summary>
    /// Get character script from an animator's parent object
    /// </summary>
    /// <param name="animator"> Animator to get from  </param>
    public static Character GetPlayer(Animator animator)
    {
        return animator.GetComponent<CharacterAnimator>().Character;
    }

    /// <summary>
    /// Return if the character photon view is the local one
    /// </summary>
    /// <param name="character"> Character to check </param>
    public static bool IsMyPlayer(Character character)
    {
        return character.ViewIsMine();
    }
}
#endregion
