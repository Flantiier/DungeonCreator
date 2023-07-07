using UnityEngine;
using Sirenix.OdinInspector;
using _ScriptableObjects.Traps;
using _Scripts.GameplayFeatures.PhysicsAdds;

namespace _Scripts.GameplayFeatures.Traps
{
    public class CageBehaviour : DestructibleTrap
    {
        #region Variables
        [FoldoutGroup("References")]
        [SerializeField] private DetectionBox triggerBox;
        [FoldoutGroup("References")]
        [SerializeField] private MeshRenderer meshRenderer;
        [FoldoutGroup("References")]
        [SerializeField] private Collider meshCollider;
        [FoldoutGroup("References")]
        [SerializeField] private Material inactiveMaterial;

        [BoxGroup("Stats")]
        [Required, SerializeField] private CageProperties datas;
        #endregion

        #region Builts_In
        public override void OnEnable()
        {
            base.OnEnable();
            meshRenderer.enabled = true;
            meshRenderer.sharedMaterial = ViewIsMine() ? inactiveMaterial : _sharedMaterial;
            SetVisbility(false);

            if (!ViewIsMine())
            {
                EnableCageCollider(false);
                _sharedMaterial.SetFloat(DISSOLVE_PARAM, 0f);
            }
            else
                meshRenderer.sharedMaterial = inactiveMaterial;
        }

        protected override void Update()
        {
            base.Update();
            HandleCageBehaviour();
            HideCage();
        }
        #endregion

        #region Inherited Methods
        protected override void InitializeTrap()
        {
            if (!ViewIsMine())
                return;

            SetTrapHealth(datas.health);
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

            if (ViewIsMine() && meshRenderer.sharedMaterial != _sharedMaterial)
                meshRenderer.sharedMaterial = _sharedMaterial;

            if (!meshCollider.enabled)
                EnableCageCollider(true);
        }

        /// <summary>
        /// Handle mesh, collider and trigger enabled state
        /// </summary>
        private void EnableCageCollider(bool enabled)
        {
            meshCollider.enabled = enabled;
            triggerBox.Enabled = !enabled;
        }

        /// <summary>
        /// Enable the dissolve transition on the cage
        /// </summary>
        private void HideCage()
        {
            if (ViewIsMine())
                SetVisbility(meshRenderer.sharedMaterial == trapMaterial);
            else
                SetVisbility(meshCollider.enabled);
        }
        #endregion
    }
}