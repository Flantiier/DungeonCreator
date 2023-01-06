using UnityEngine;
using _Scripts.Characters.Adventurers;

namespace _Scripts.Characters.Animations
{
    public class WarriorAnimator : CharacterAnimator
    {
        #region Variables
        [Header("Warrior requirements")]
        [SerializeField] protected GameObject sword;
        #endregion

        #region Properties
        public Warrior MyWarrior { get; private set; }
        #endregion

        #region Builts_In
        public override void Awake()
        {
            if (!ViewIsMine())
                return;

            base.Awake();
            MyWarrior = GetComponentInParent<Warrior>();
        }
        #endregion

        #region Animation Methods
        /// <summary>
        /// Enabe or Disable the sword
        /// </summary>
        public void EnableSword(bool state)
        {
            if (!sword)
                return;

            sword.SetActive(state);
        }
        #endregion
    }
}
