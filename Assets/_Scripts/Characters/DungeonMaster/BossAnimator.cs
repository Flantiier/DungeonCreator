using UnityEngine;
using Sirenix.OdinInspector;
using _Scripts.NetworkScript;
using _Scripts.Hitboxs_Triggers;
using _Scripts.GameplayFeatures.Projectiles;
using _Scripts.Hitboxs_Triggers.Hitboxs;
using Photon.Pun;

namespace _Scripts.Characters.DungeonMaster
{
    public class BossAnimator : NetworkMonoBehaviour
    {
        #region Variables
        [FoldoutGroup("References")]
        [SerializeField] private StunHitbox stunZone;
        [TitleGroup("References/Projectile")]
        [SerializeField] private EnemiesProjectile projectile;
        [TitleGroup("References/Projectile")]
        [SerializeField] private Transform throwPoint;

        [TitleGroup("Hitboxs")]
        [SerializeField] protected EnemyHitbox[] hitboxs;
        #endregion

        #region Properties
        public BossController Boss { get; private set; }
        #endregion

        #region Built_In
        private void Awake()
        {
            Boss = GetComponentInParent<BossController>();
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
            if (!stunZone)
                return;

            Instantiate(stunZone, transform.position, transform.rotation);
        }

        /// <summary>
        /// Create a projectile and throw it in front on the player
        /// </summary>
        public void ThrowProjectile()
        {
            if (!ViewIsMine() || !projectile)
                return;

            GameObject instance = PhotonNetwork.Instantiate(projectile.name, throwPoint.position, transform.rotation);
            EnemiesProjectile proj = instance.GetComponent<EnemiesProjectile>();
            proj.ThrowProjectile(Boss.MainCamera.forward);
        }
        #endregion
    }
}