using UnityEngine;
using UnityEngine.InputSystem;
using _Scripts.Interfaces;
using _ScriptableObjects.Cinemachine;
using Unity.Rendering;

namespace _Scripts.Characters.Adventurers
{
    public class Bowman : Character
    {
        #region Variables
        [Header("Defuse properties")]
        [SerializeField] private float speedDuringAbility = 2f;
        [SerializeField] private TpsCameraProperties cameraProperties;
        [SerializeField] private float defuseDistance = 3f;
        [SerializeField] private LayerMask defuseLayer;

        private IDefusable _currentTrap;
        private RaycastHit _hit;
        #endregion

        #region Properties
        public bool TrapDetected { get; private set; }
        public bool IsDefusing { get; set; }
        public float TargetDefuseTime { get; set; }
        public float CurrentDefuseTime { get; set; }
        #endregion

        #region Builts_In
        protected override void Update()
        {
            base.Update();
            TrapDetection();
        }
        #endregion

        #region Inherited Methods

        #region Inputs Methods
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

            Animator.SetBool("Defusing", IsDefusing);
            UpdateAnimationLayers();
        }

        protected override void HandleCharacterStateMachine()
        {
            if (IsDefusing)
                return;

            base.HandleCharacterStateMachine();
        }

        protected override bool SkillConditions()
        {
            if (IsDefusing)
                return false;

            return base.SkillConditions();
        }
        #endregion

        #region Methods
        public override float GetMovementSpeed()
        {
            if (PlayerSM.EnableLayers)
                return speedDuringAbility;

            return base.GetMovementSpeed();
        }

        /// <summary>
        /// Shoot a raycasr and indicates if ther's a defusable trap detected
        /// </summary>
        private void TrapDetection()
        {
            Vector3 position = MainCamera.position;
            Vector3 direction = MainCamera.forward;
            float distance = cameraProperties.framingTranposer.m_CameraDistance + defuseDistance;

            if (!Physics.Raycast(position, direction, out _hit, distance, defuseLayer))
                TrapDetected = false;
            else
                TrapDetected = true;
        }

        private void SkillAction(InputAction.CallbackContext _)
        {
            if (!SkillConditions() || !TrapDetected)
                return;

            if (!_hit.collider.TryGetComponent(out IDefusable trap) || trap.IsDisabled)
                return;

            _currentTrap = trap;
            TargetDefuseTime = trap.DefuseDuration;
            RPCAnimatorTrigger(Photon.Pun.RpcTarget.All, "SkillEnabled", true);
        }

        /// <summary>
        /// Defuse the last trap
        /// </summary>
        public void DefuseTrap()
        {
            if (_currentTrap == null)
                return;

            _currentTrap.DefuseTrap();
            SkillUsed();
        }
        #endregion
    }
}
