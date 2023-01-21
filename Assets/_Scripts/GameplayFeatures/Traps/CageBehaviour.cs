using UnityEngine;
using Sirenix.OdinInspector;
using Photon.Pun;
using _Scripts.GameplayFeatures.PhysicsAdds;

namespace _Scripts.GameplayFeatures.Traps
{
    public class CageBehaviour : DestructibleTrap
    {
        #region Variables
        [TitleGroup("References")]
        [SerializeField] private GameObject cage;
        [SerializeField] private DetectionBox triggerBox;
        #endregion

        #region Builts_In
        public override void OnEnable()
        {
            base.OnEnable();
            EnableCage(false);
        }

        private void Update()
        {
            HandleCageBehaviour();
        }
        #endregion

        #region Methods
        /// <summary>
        /// Enable the cage if smth has beeen detected
        /// </summary>
        private void HandleCageBehaviour()
        {
            if (!triggerBox || !triggerBox.IsDetecting())
                return;

            if (cage.activeSelf)
                return;

            EnableCage(true);
        }

        /// <summary>
        /// Enable the cage and disable its trigger
        /// </summary>
        private void EnableCage(bool enabled)
        {
            cage.SetActive(enabled);
            triggerBox.Enabled = !enabled;
        }

        protected override void HandleTrapDestruction()
        {
            PhotonNetwork.Destroy(gameObject);
        }
        #endregion
    }
}