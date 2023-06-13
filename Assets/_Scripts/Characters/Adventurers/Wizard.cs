using Photon.Pun;
using UnityEngine;
using UnityEngine.InputSystem;
using _Scripts.GameplayFeatures;
using System.Runtime.CompilerServices;

namespace _Scripts.Characters.Adventurers
{
	public class Wizard : Character
	{
        #region Variables
        [Header("Wizard properties")]
        [SerializeField] private float speedDuringAbility = 2f;
        [SerializeField] private ScanArea scanAreaPrefab;
        #endregion

        #region Inherited Methods

        #region Inputs
        protected override void SubscribeInputActions()
        {
            base.SubscribeInputActions();

            _inputs.Gameplay.Skill.started += SkillAction;
        }

        protected override void UnsubscribeInputActions()
        {
            base.UnsubscribeInputActions();

            _inputs.Gameplay.Skill.started -= SkillAction;
        }
        #endregion

        protected override void UpdateAnimations()
        {
            base.UpdateAnimations();
            //UpdateAnimationLayers();
        }
        #endregion

        #region Methods
        public override float GetMovementSpeed()
        {
            if (PlayerSM.EnableLayers)
                return speedDuringAbility;

            return base.GetMovementSpeed();
        }

        private void SkillAction(InputAction.CallbackContext _)
        {
            if (!SkillConditions())
                return;

            RPCAnimatorTrigger(RpcTarget.All, "SkillEnabled", true);
        }

        public void InstantiateScanArea()
        {
            if (!ViewIsMine() || !scanAreaPrefab)
                return;

            PhotonNetwork.Instantiate(scanAreaPrefab.name, transform.position, Quaternion.identity);
            SkillUsed();
        }
        #endregion
    }
}
