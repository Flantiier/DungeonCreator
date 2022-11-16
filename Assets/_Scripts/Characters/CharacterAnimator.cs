using UnityEngine;
using _Scripts.NetworkScript;
using _Scripts.Hitboxs;

namespace _Scripts.Characters.Animations
{
    public class CharacterAnimator : NetworkMonoBehaviour
    {
        #region Variables
        [Header("Hitboxs")]
        [SerializeField] protected CharacterHitbox[] hitboxs;
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
