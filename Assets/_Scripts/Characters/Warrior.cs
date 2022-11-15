using UnityEngine;

namespace _Scripts.Characters
{
	public class Warrior : Character
	{
		#region Variables
		[Header("Warrior properties")]
		[SerializeField] private Collider shieldCollider;
        #endregion

        #region Properties
        public Collider ShieldCollider => shieldCollider;
        #endregion

        #region Builts_In
        public override void Update()
        {
            if (!ViewIsMine())
                return;

            base.Update();

            WarriorUpdate();
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
        private void WarriorUpdate()
        {
            if (PlayerSM.EnableLayers)
                RotateMeshToOrientation();
        }
        #endregion
    }
}
