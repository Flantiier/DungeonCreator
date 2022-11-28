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
        #endregion

        #region Builts_In
        public override void OnEnable()
        {
            if (!ViewIsMine())
                return;

            base.OnEnable();

            Shield.OnShieldDestroyed += InvokeSkillCooldown;
            OnSkillRecovered += Shield.InitializeShield;
        }

        public override void OnDisable()
        {
            if (!ViewIsMine())
                return;

            base.OnEnable();

            Shield.OnShieldDestroyed -= InvokeSkillCooldown;
            OnSkillRecovered -= Shield.InitializeShield;
        }
        #endregion

        #region Inherited Methods
        protected override void UpdateAnimations()
        {
            base.UpdateAnimations();

            PlayerSM.UsingSkill = SkillConditions() && _inputs.actions["Skill"].IsPressed();
            _animator.SetBool("SkillEnabled", PlayerSM.UsingSkill);

            PlayerSM.EnableLayers = PlayerSM.UsingSkill;
            UpdateAnimationLayers();
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
