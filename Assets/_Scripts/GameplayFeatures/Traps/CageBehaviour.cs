﻿using UnityEngine;
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
            EnableCage(false);

            if (ViewIsMine())
            {
                meshRenderer.sharedMaterial = inactiveMaterial;
                meshRenderer.enabled = true;
            }
        }

        private void Update()
        {
            HandleCageBehaviour();
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

            if (meshRenderer.enabled)
                return;

            EnableCage(true);

            if (ViewIsMine())
                meshRenderer.sharedMaterial = sharedMaterial;
        }

        /// <summary>
        /// Handle mesh, collider and trigger enabled state
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