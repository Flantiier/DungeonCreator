using UnityEngine;

namespace _Scripts.Characters
{
    public class Warrior : Character
    {
        #region Variables
        [Header("Warrior references")]
        [SerializeField] private Shield shield;
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

        public override bool RunConditions()
        {
            if (UsingShield)
                return false;

            return base.RunConditions();
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
