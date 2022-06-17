using UnityEngine;
using UnityEngine.InputSystem;

namespace Adventurer
{
    [RequireComponent(typeof(PlayerInput))]
    public class AdventurerInputManager : MonoBehaviour
    {
        //Reference
        [SerializeField] private PlayerInput inputs;

        public PlayerInput Input => inputs;

        public Vector2 DirectionInputs { get; private set; }

        public bool isDodging { get; private set; }

        public bool isRunning { get; private set; }

        public bool isAttacking { get; private set; }

        public bool IsHoldingWeapon { get; private set; }

        public bool usingAbility01 { get; private set; }

        public bool usingAbility02 { get; private set; }

        private void OnEnable()
        {
            inputs.ActivateInput();
            
            //Direction
            Input.actions["Motion"].performed += HandleMotion;
            Input.actions["Motion"].canceled += HandleMotion;

            //Dodge
            Input.actions["Dodge"].started += HandleDodge;
            Input.actions["Dodge"].canceled += HandleDodge;

            //Run
            Input.actions["Run"].started += HandleRun;
            Input.actions["Run"].canceled += HandleRun;

            //Attack
            Input.actions["Attack"].started += HandleAttack;
            Input.actions["Attack"].canceled += HandleAttack;

            //Aim 
            Input.actions["Aim"].started += HandleAim;
            Input.actions["Aim"].canceled += HandleAim;

            //Ability 01 
            Input.actions["Ability01"].started += HandleAbility01;
            Input.actions["Ability01"].canceled += HandleAbility01;

            //Aim 
            Input.actions["Ability02"].started += HandleAbility02;
            Input.actions["Ability02"].canceled += HandleAbility02;
        }

        private void OnDisable()
        {
            inputs.DeactivateInput();

            //Direction
            Input.actions["Motion"].performed += HandleMotion;
            Input.actions["Motion"].canceled -= HandleMotion;

            //Dodge
            Input.actions["Dodge"].started -= HandleDodge;
            Input.actions["Dodge"].canceled -= HandleDodge;

            //Run
            Input.actions["Run"].started -= HandleRun;
            Input.actions["Run"].canceled -= HandleRun;

            //Attack
            Input.actions["Attack"].started -= HandleAttack;
            Input.actions["Attack"].canceled -= HandleAttack;

            //Aim
            Input.actions["Aim"].started -= HandleAim;
            Input.actions["Aim"].canceled -= HandleAim;

            //Ability 01 
            Input.actions["Ability01"].started -= HandleAbility01;
            Input.actions["Ability01"].canceled -= HandleAbility01;

            //Aim 
            Input.actions["Ability02"].started -= HandleAbility02;
            Input.actions["Ability02"].canceled -= HandleAbility02;
        }

        private void HandleMotion(InputAction.CallbackContext ctx)
        { 
            DirectionInputs = ctx.ReadValue<Vector2>().normalized;
        }

        private void HandleDodge(InputAction.CallbackContext ctx)
        {
            isDodging = ctx.ReadValueAsButton();
        }

        private void HandleRun(InputAction.CallbackContext ctx)
        {
            isRunning = ctx.ReadValueAsButton();
        }

        private void HandleAttack(InputAction.CallbackContext ctx)
        {
            isAttacking = ctx.ReadValueAsButton();
        }

        private void HandleAim(InputAction.CallbackContext ctx)
        {
            IsHoldingWeapon = ctx.ReadValueAsButton();
        }

        private void HandleAbility01(InputAction.CallbackContext ctx)
        {
            usingAbility01 = ctx.ReadValueAsButton();
        }

        private void HandleAbility02(InputAction.CallbackContext ctx)
        {
            usingAbility02 = ctx.ReadValueAsButton();
        }
    }
}
