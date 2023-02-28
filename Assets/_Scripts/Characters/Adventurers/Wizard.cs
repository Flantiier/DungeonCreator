using Photon.Pun;
using UnityEngine;
using UnityEngine.InputSystem;
using _Scripts.GameplayFeatures;

namespace _Scripts.Characters.Adventurers
{
	public class Wizard : Character
	{
        #region Variables
        [Header("Wizard properties")]
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

            UpdateAnimationLayers();
        }
        #endregion

        #region Methods
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
        }
        #endregion
    }
}
