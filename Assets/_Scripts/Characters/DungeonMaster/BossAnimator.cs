using UnityEngine;
using Sirenix.OdinInspector;
using _Scripts.NetworkScript;
using _Scripts.Hitboxs_Triggers;
using _Scripts.GameplayFeatures.Projectiles;
using _Scripts.Hitboxs_Triggers.Hitboxs;
using Photon.Pun;
using _Scripts.Interfaces;

namespace _Scripts.Characters.DungeonMaster
{
    public class BossAnimator : NetworkMonoBehaviour
    {
        #region Variables
        [TitleGroup("Hitboxs")]
        [SerializeField] protected EnemyHitbox[] hitboxs;

        [TitleGroup("Projectiles")]
        [SerializeField] private EnemiesProjectile projectile;
        [TitleGroup("Projectiles")]
        [SerializeField] private Transform throwPoint;

        [TitleGroup("Stun ability")]
        [SerializeField] private float stunRange = 10f;
        [TitleGroup("Stun ability")]
        [SerializeField] private float stunDuration = 4f;
        [TitleGroup("Stun ability")]
        [SerializeField] private LayerMask stunMask;
        #endregion

        #region Properties
        public BossController Boss { get; private set; }
        #endregion

        #region Built_In
        private void Awake()
        {
            Boss = GetComponentInParent<BossController>();
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.DrawWireSphere(transform.parent.position, stunRange);
        }
        #endregion

        #region Hitboxs
        /// <summary>
        /// Enable a hitbox in the colliderArray
        /// </summary>
        /// <param name="index"> hitbox's index </param>
        public void EnableCollider(int index)
        {
            if (HitboxNotFound(index))
                return;

            hitboxs[index].Collider.enabled = true;
        }

        /// <summary>
        /// Disable a hitbox in the colliderArray
        /// </summary>
        /// <param name="index"> hitbox's index </param>
        public void DisableCollider(int index)
        {
            if (HitboxNotFound(index))
                return;

            hitboxs[index].Collider.enabled = false;
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
            hitboxs = GetComponentsInChildren<EnemyHitbox>();
        }
        #endregion

        #region Animation Methods
        /// <summary>
        /// Use one of the abilities of the boss
        /// </summary>
        /// <param name="index"></param>
        public void UseAbility(int index)
        {
            Boss.AbilityUsed(index);
        }

        /// <summary>
        /// Create a zone which stuns the players inside it
        /// </summary>
        public void CreateImpactZone()
        {
            Collider[] colliders = Physics.OverlapSphere(Boss.transform.position, stunRange, stunMask);

            foreach (Collider col in colliders)
            {
                if (!col || !col.TryGetComponent(out IPlayerStunable player))
                    continue;

                Character character = col.GetComponent<Character>();
                character.StunPlayer(stunDuration);
            }
        }

        /// <summary>
        /// Create a projectile and throw it in front on the player
        /// </summary>
        public void ThrowProjectiles()
        {
            if (!ViewIsMine() || !projectile)
                return;

            Character[] characters = FindObjectsOfType<Character>();

            if (characters.Length <= 0 || characters == null)
                return;

            foreach (Character character in characters)
            {
                if (character.CurrentHealth <= 0)
                    continue;

                //Throw projectile
                Vector3 direction = character.transform.position - throwPoint.position;
                GameObject instance = PhotonNetwork.Instantiate(projectile.name, throwPoint.position, Quaternion.LookRotation(direction));
                EnemiesProjectile proj = instance.GetComponent<EnemiesProjectile>();
                proj.ThrowProjectile(direction + Vector3.up);
            }
        }
        #endregion
    }
}