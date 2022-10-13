//------------------------------------------------------------------------------
// <auto-generated>
//     This code was auto-generated by com.unity.inputsystem:InputActionCodeGenerator
//     version 1.4.2
//     from Assets/Inputs_Maps/Adventurer.inputactions
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

namespace PlayerInputs
{
    public partial class @Adventurer : IInputActionCollection2, IDisposable
    {
        public InputActionAsset asset { get; }
        public @Adventurer()
        {
            asset = InputActionAsset.FromJson(@"{
    ""name"": ""Adventurer"",
    ""maps"": [
        {
            ""name"": ""Controls"",
            ""id"": ""3782464c-a55f-41a2-97e4-0c2e4ca56061"",
            ""actions"": [
                {
                    ""name"": ""Move"",
                    ""type"": ""Value"",
                    ""id"": ""8deae71b-928b-426f-b673-66e2264873d3"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""Run"",
                    ""type"": ""Button"",
                    ""id"": ""ab886c4d-e460-4bb8-9cfc-374bad69a1ea"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Roll"",
                    ""type"": ""Button"",
                    ""id"": ""41d8770c-6a73-457f-ae21-b23bc3296d2c"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Attack"",
                    ""type"": ""Button"",
                    ""id"": ""027f58dc-add8-4bc7-86be-fda02543078c"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Aim"",
                    ""type"": ""Value"",
                    ""id"": ""de1feb52-ba08-4334-b3a6-533494d3a77f"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""Look"",
                    ""type"": ""Value"",
                    ""id"": ""d4d36912-9446-4df3-a300-108a92479d3f"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""FirstAbility"",
                    ""type"": ""Button"",
                    ""id"": ""cbbda7f5-2e17-4100-aa72-6e0a22d08962"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""SecondAbility"",
                    ""type"": ""Button"",
                    ""id"": ""6b08696e-b4a2-46ac-88b6-8588d484dd4f"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": ""WASD/ZQSD"",
                    ""id"": ""173da773-4384-4a5b-9614-a36c51b758cf"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""4695c2da-3384-4b5d-b8fa-dc25f9ff033b"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""MK"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""9054f5a9-040f-413e-b1ef-3cafbea93ad2"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""MK"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""b064948a-efb6-403d-b91d-8ebb4b815b94"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""MK"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""d342c31f-e55e-4fec-9b43-75149b74dac2"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""MK"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""e877bb4e-ae95-4cad-a8d2-aab527e2fdf5"",
                    ""path"": ""<Gamepad>/leftStick"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""GamePad"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""bf3ec748-4280-425c-95b8-89f6f252f7e6"",
                    ""path"": ""<Gamepad>/buttonEast"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""MK"",
                    ""action"": ""Roll"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""f26f9cfd-810b-4d06-afbf-b9f4a9e10436"",
                    ""path"": ""<Gamepad>/buttonEast"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Roll"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""8f69fbe1-126c-4317-adee-06b784b82b21"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""MK"",
                    ""action"": ""Attack"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""37eb4d58-ee54-4270-9b01-a465955757df"",
                    ""path"": ""<Gamepad>/buttonNorth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""GamePad"",
                    ""action"": ""Attack"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""770bbcf9-ed46-4d53-917f-ef04dbf1a9a1"",
                    ""path"": ""<Mouse>/rightButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""MK"",
                    ""action"": ""Aim"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""c50d635d-37da-437f-8a43-c2abc3badf3d"",
                    ""path"": ""<Gamepad>/leftTrigger"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""GamePad"",
                    ""action"": ""Aim"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""8c21649b-6e89-400c-bf06-7efd07022973"",
                    ""path"": ""<Mouse>/delta"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""MK"",
                    ""action"": ""Look"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""f4bf3675-b80b-441e-89f2-0ed05fbcc47e"",
                    ""path"": ""<Gamepad>/rightStick"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""GamePad"",
                    ""action"": ""Look"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""8bdca0f9-1155-4cfa-b0a6-0688fab320dd"",
                    ""path"": ""<Keyboard>/q"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""FirstAbility"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""3fea16f8-8f16-40b1-9d2d-cec250a544ed"",
                    ""path"": ""<Keyboard>/e"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""SecondAbility"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""2bc036f5-05f5-473a-81d2-f14da2f8dfe6"",
                    ""path"": ""<Keyboard>/leftShift"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""MK"",
                    ""action"": ""Run"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""b146bd69-9627-4dcc-a84f-403220533f73"",
                    ""path"": ""<Gamepad>/buttonSouth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""GamePad"",
                    ""action"": ""Run"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": [
        {
            ""name"": ""MK"",
            ""bindingGroup"": ""MK"",
            ""devices"": [
                {
                    ""devicePath"": ""<Keyboard>"",
                    ""isOptional"": false,
                    ""isOR"": false
                },
                {
                    ""devicePath"": ""<Mouse>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        },
        {
            ""name"": ""GamePad"",
            ""bindingGroup"": ""GamePad"",
            ""devices"": [
                {
                    ""devicePath"": ""<Gamepad>"",
                    ""isOptional"": true,
                    ""isOR"": false
                }
            ]
        }
    ]
}");
            // Controls
            m_Controls = asset.FindActionMap("Controls", throwIfNotFound: true);
            m_Controls_Move = m_Controls.FindAction("Move", throwIfNotFound: true);
            m_Controls_Run = m_Controls.FindAction("Run", throwIfNotFound: true);
            m_Controls_Roll = m_Controls.FindAction("Roll", throwIfNotFound: true);
            m_Controls_Attack = m_Controls.FindAction("Attack", throwIfNotFound: true);
            m_Controls_Aim = m_Controls.FindAction("Aim", throwIfNotFound: true);
            m_Controls_Look = m_Controls.FindAction("Look", throwIfNotFound: true);
            m_Controls_FirstAbility = m_Controls.FindAction("FirstAbility", throwIfNotFound: true);
            m_Controls_SecondAbility = m_Controls.FindAction("SecondAbility", throwIfNotFound: true);
        }

        public void Dispose()
        {
            UnityEngine.Object.Destroy(asset);
        }

        public InputBinding? bindingMask
        {
            get => asset.bindingMask;
            set => asset.bindingMask = value;
        }

        public ReadOnlyArray<InputDevice>? devices
        {
            get => asset.devices;
            set => asset.devices = value;
        }

        public ReadOnlyArray<InputControlScheme> controlSchemes => asset.controlSchemes;

        public bool Contains(InputAction action)
        {
            return asset.Contains(action);
        }

        public IEnumerator<InputAction> GetEnumerator()
        {
            return asset.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Enable()
        {
            asset.Enable();
        }

        public void Disable()
        {
            asset.Disable();
        }
        public IEnumerable<InputBinding> bindings => asset.bindings;

        public InputAction FindAction(string actionNameOrId, bool throwIfNotFound = false)
        {
            return asset.FindAction(actionNameOrId, throwIfNotFound);
        }
        public int FindBinding(InputBinding bindingMask, out InputAction action)
        {
            return asset.FindBinding(bindingMask, out action);
        }

        // Controls
        private readonly InputActionMap m_Controls;
        private IControlsActions m_ControlsActionsCallbackInterface;
        private readonly InputAction m_Controls_Move;
        private readonly InputAction m_Controls_Run;
        private readonly InputAction m_Controls_Roll;
        private readonly InputAction m_Controls_Attack;
        private readonly InputAction m_Controls_Aim;
        private readonly InputAction m_Controls_Look;
        private readonly InputAction m_Controls_FirstAbility;
        private readonly InputAction m_Controls_SecondAbility;
        public struct ControlsActions
        {
            private @Adventurer m_Wrapper;
            public ControlsActions(@Adventurer wrapper) { m_Wrapper = wrapper; }
            public InputAction @Move => m_Wrapper.m_Controls_Move;
            public InputAction @Run => m_Wrapper.m_Controls_Run;
            public InputAction @Roll => m_Wrapper.m_Controls_Roll;
            public InputAction @Attack => m_Wrapper.m_Controls_Attack;
            public InputAction @Aim => m_Wrapper.m_Controls_Aim;
            public InputAction @Look => m_Wrapper.m_Controls_Look;
            public InputAction @FirstAbility => m_Wrapper.m_Controls_FirstAbility;
            public InputAction @SecondAbility => m_Wrapper.m_Controls_SecondAbility;
            public InputActionMap Get() { return m_Wrapper.m_Controls; }
            public void Enable() { Get().Enable(); }
            public void Disable() { Get().Disable(); }
            public bool enabled => Get().enabled;
            public static implicit operator InputActionMap(ControlsActions set) { return set.Get(); }
            public void SetCallbacks(IControlsActions instance)
            {
                if (m_Wrapper.m_ControlsActionsCallbackInterface != null)
                {
                    @Move.started -= m_Wrapper.m_ControlsActionsCallbackInterface.OnMove;
                    @Move.performed -= m_Wrapper.m_ControlsActionsCallbackInterface.OnMove;
                    @Move.canceled -= m_Wrapper.m_ControlsActionsCallbackInterface.OnMove;
                    @Run.started -= m_Wrapper.m_ControlsActionsCallbackInterface.OnRun;
                    @Run.performed -= m_Wrapper.m_ControlsActionsCallbackInterface.OnRun;
                    @Run.canceled -= m_Wrapper.m_ControlsActionsCallbackInterface.OnRun;
                    @Roll.started -= m_Wrapper.m_ControlsActionsCallbackInterface.OnRoll;
                    @Roll.performed -= m_Wrapper.m_ControlsActionsCallbackInterface.OnRoll;
                    @Roll.canceled -= m_Wrapper.m_ControlsActionsCallbackInterface.OnRoll;
                    @Attack.started -= m_Wrapper.m_ControlsActionsCallbackInterface.OnAttack;
                    @Attack.performed -= m_Wrapper.m_ControlsActionsCallbackInterface.OnAttack;
                    @Attack.canceled -= m_Wrapper.m_ControlsActionsCallbackInterface.OnAttack;
                    @Aim.started -= m_Wrapper.m_ControlsActionsCallbackInterface.OnAim;
                    @Aim.performed -= m_Wrapper.m_ControlsActionsCallbackInterface.OnAim;
                    @Aim.canceled -= m_Wrapper.m_ControlsActionsCallbackInterface.OnAim;
                    @Look.started -= m_Wrapper.m_ControlsActionsCallbackInterface.OnLook;
                    @Look.performed -= m_Wrapper.m_ControlsActionsCallbackInterface.OnLook;
                    @Look.canceled -= m_Wrapper.m_ControlsActionsCallbackInterface.OnLook;
                    @FirstAbility.started -= m_Wrapper.m_ControlsActionsCallbackInterface.OnFirstAbility;
                    @FirstAbility.performed -= m_Wrapper.m_ControlsActionsCallbackInterface.OnFirstAbility;
                    @FirstAbility.canceled -= m_Wrapper.m_ControlsActionsCallbackInterface.OnFirstAbility;
                    @SecondAbility.started -= m_Wrapper.m_ControlsActionsCallbackInterface.OnSecondAbility;
                    @SecondAbility.performed -= m_Wrapper.m_ControlsActionsCallbackInterface.OnSecondAbility;
                    @SecondAbility.canceled -= m_Wrapper.m_ControlsActionsCallbackInterface.OnSecondAbility;
                }
                m_Wrapper.m_ControlsActionsCallbackInterface = instance;
                if (instance != null)
                {
                    @Move.started += instance.OnMove;
                    @Move.performed += instance.OnMove;
                    @Move.canceled += instance.OnMove;
                    @Run.started += instance.OnRun;
                    @Run.performed += instance.OnRun;
                    @Run.canceled += instance.OnRun;
                    @Roll.started += instance.OnRoll;
                    @Roll.performed += instance.OnRoll;
                    @Roll.canceled += instance.OnRoll;
                    @Attack.started += instance.OnAttack;
                    @Attack.performed += instance.OnAttack;
                    @Attack.canceled += instance.OnAttack;
                    @Aim.started += instance.OnAim;
                    @Aim.performed += instance.OnAim;
                    @Aim.canceled += instance.OnAim;
                    @Look.started += instance.OnLook;
                    @Look.performed += instance.OnLook;
                    @Look.canceled += instance.OnLook;
                    @FirstAbility.started += instance.OnFirstAbility;
                    @FirstAbility.performed += instance.OnFirstAbility;
                    @FirstAbility.canceled += instance.OnFirstAbility;
                    @SecondAbility.started += instance.OnSecondAbility;
                    @SecondAbility.performed += instance.OnSecondAbility;
                    @SecondAbility.canceled += instance.OnSecondAbility;
                }
            }
        }
        public ControlsActions @Controls => new ControlsActions(this);
        private int m_MKSchemeIndex = -1;
        public InputControlScheme MKScheme
        {
            get
            {
                if (m_MKSchemeIndex == -1) m_MKSchemeIndex = asset.FindControlSchemeIndex("MK");
                return asset.controlSchemes[m_MKSchemeIndex];
            }
        }
        private int m_GamePadSchemeIndex = -1;
        public InputControlScheme GamePadScheme
        {
            get
            {
                if (m_GamePadSchemeIndex == -1) m_GamePadSchemeIndex = asset.FindControlSchemeIndex("GamePad");
                return asset.controlSchemes[m_GamePadSchemeIndex];
            }
        }
        public interface IControlsActions
        {
            void OnMove(InputAction.CallbackContext context);
            void OnRun(InputAction.CallbackContext context);
            void OnRoll(InputAction.CallbackContext context);
            void OnAttack(InputAction.CallbackContext context);
            void OnAim(InputAction.CallbackContext context);
            void OnLook(InputAction.CallbackContext context);
            void OnFirstAbility(InputAction.CallbackContext context);
            void OnSecondAbility(InputAction.CallbackContext context);
        }
    }
}
