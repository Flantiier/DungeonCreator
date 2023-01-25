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
        [SerializeField] private MeshRenderer meshRenderer;
        [TitleGroup("References")]
        [SerializeField] private Collider meshCollider;
        [TitleGroup("References")]
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

            if (meshRenderer.enabled)
                return;

            EnableCage(true);
        }

        /// <summary>
        /// Enable the cage and disable its trigger
        /// </summary>
        private void EnableCage(bool enabled)
        {
            meshRenderer.enabled = enabled;
            meshCollider.enabled = enabled;
            triggerBox.Enabled = !enabled;
        }
        #endregion
    }
}