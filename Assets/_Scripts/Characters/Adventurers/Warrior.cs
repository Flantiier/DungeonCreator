using UnityEngine;
using _Scripts.GameplayFeatures.Weapons;

namespace _Scripts.Characters.Adventurers
{
    public class Warrior : Character
    {
        #region Variables
        [Header("Warrior references")]
        [SerializeField] private Shield shield;
        [SerializeField] private float speedDuringAbility = 1.5f;
        #endregion

        #region Properties
        public Shield Shield => shield;
        public bool UsingShield { get; set; }
        #endregion

        #region Builts_In
        public override void OnEnable()
        {
            if (!ViewIsMine())
                return;

            base.OnEnable();

            Shield.OnShieldDestroyed += SkillUsed;
            OnSkillRecovered += Shield.InitializeShield;
        }

        public override void OnDisable()
        {
            if (!ViewIsMine())
                return;

            base.OnEnable();

            Shield.OnShieldDestroyed -= SkillUsed;
            OnSkillRecovered -= Shield.InitializeShield;
        }
        #endregion

        #region Inherited Methods
        protected override void UpdateAnimations()
        {
            base.UpdateAnimations();

            UsingShield = SkillConditions() && _inputs.Gameplay.Skill.IsPressed();
            Animator.SetBool("SkillEnabled", UsingShield);

            PlayerSM.EnableLayers = UsingShield;
            UpdateAnimationLayers();
        }

        public override float GetMovementSpeed()
        {
            if (SkillConditions() && PlayerSM.EnableLayers)
                return speedDuringAbility;

            return base.GetMovementSpeed();
        }

        public override bool RunConditions()
        {
            if (UsingShield)
                return false;

            return base.RunConditions();
        }

        protected override bool SkillConditions()
        {
            if (shield.CurrentHealth <= 0)
                return false;

            return base.SkillConditions();
        }
        #endregion

        #region Methods
        protected override void InitializeCharacter()
        {
            base.InitializeCharacter();

            shield.InitializeShield();
        }
        #endregion
    }
}
