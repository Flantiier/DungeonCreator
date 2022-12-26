//------------------------------------------------------------------------------
// <auto-generated>
//     This code was auto-generated by com.unity.inputsystem:InputActionCodeGenerator
//     version 1.4.3
//     from Assets/_Scripts/Inputs Maps/AdventurerInputs.inputactions
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

namespace InputsMaps
{
    public partial class @AdventurerInputs : IInputActionCollection2, IDisposable
    {
        public InputActionAsset asset { get; }
        public @AdventurerInputs()
        {
            asset = InputActionAsset.FromJson(@"{
    ""name"": ""AdventurerInputs"",
    ""maps"": [
        {
            ""name"": ""Gameplay"",
            ""id"": ""3782464c-a55f-41a2-97e4-0c2e4ca56061"",
            ""actions"": [
                {
                    ""name"": ""Look"",
                    ""type"": ""PassThrough"",
                    ""id"": ""d4d36912-9446-4df3-a300-108a92479d3f"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
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
                    ""name"": ""MainAttack"",
                    ""type"": ""Button"",
                    ""id"": ""027f58dc-add8-4bc7-86be-fda02543078c"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""SecondAttack"",
                    ""type"": ""Button"",
                    ""id"": ""dc19a340-ae01-49a0-b0c8-858012a3ba7a"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Skill"",
                    ""type"": ""Button"",
                    ""id"": ""11d8fabf-0a22-400e-83fc-8876713d6ad9"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""bf3ec748-4280-425c-95b8-89f6f252f7e6"",
                    ""path"": ""<Keyboard>/ctrl"",
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
                    ""groups"": ""GamePad"",
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
                    ""action"": ""MainAttack"",
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
                    ""action"": ""MainAttack"",
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
                    ""id"": ""d40fa854-d052-4438-af2f-30d4c3530425"",
                    ""path"": ""<Mouse>/rightButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""MK"",
                    ""action"": ""SecondAttack"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""c386a6f2-be5d-459c-8403-811c40c8c67c"",
                    ""path"": ""<Keyboard>/f"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""MK"",
                    ""action"": ""Skill"",
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
            // Gameplay
            m_Gameplay = asset.FindActionMap("Gameplay", throwIfNotFound: true);
            m_Gameplay_Look = m_Gameplay.FindAction("Look", throwIfNotFound: true);
            m_Gameplay_Move = m_Gameplay.FindAction("Move", throwIfNotFound: true);
            m_Gameplay_Run = m_Gameplay.FindAction("Run", throwIfNotFound: true);
            m_Gameplay_Roll = m_Gameplay.FindAction("Roll", throwIfNotFound: true);
            m_Gameplay_MainAttack = m_Gameplay.FindAction("MainAttack", throwIfNotFound: true);
            m_Gameplay_SecondAttack = m_Gameplay.FindAction("SecondAttack", throwIfNotFound: true);
            m_Gameplay_Skill = m_Gameplay.FindAction("Skill", throwIfNotFound: true);
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

        // Gameplay
        private readonly InputActionMap m_Gameplay;
        private IGameplayActions m_GameplayActionsCallbackInterface;
        private readonly InputAction m_Gameplay_Look;
        private readonly InputAction m_Gameplay_Move;
        private readonly InputAction m_Gameplay_Run;
        private readonly InputAction m_Gameplay_Roll;
        private readonly InputAction m_Gameplay_MainAttack;
        private readonly InputAction m_Gameplay_SecondAttack;
        private readonly InputAction m_Gameplay_Skill;
        public struct GameplayActions
        {
            private @AdventurerInputs m_Wrapper;
            public GameplayActions(@AdventurerInputs wrapper) { m_Wrapper = wrapper; }
            public InputAction @Look => m_Wrapper.m_Gameplay_Look;
            public InputAction @Move => m_Wrapper.m_Gameplay_Move;
            public InputAction @Run => m_Wrapper.m_Gameplay_Run;
            public InputAction @Roll => m_Wrapper.m_Gameplay_Roll;
            public InputAction @MainAttack => m_Wrapper.m_Gameplay_MainAttack;
            public InputAction @SecondAttack => m_Wrapper.m_Gameplay_SecondAttack;
            public InputAction @Skill => m_Wrapper.m_Gameplay_Skill;
            public InputActionMap Get() { return m_Wrapper.m_Gameplay; }
            public void Enable() { Get().Enable(); }
            public void Disable() { Get().Disable(); }
            public bool enabled => Get().enabled;
            public static implicit operator InputActionMap(GameplayActions set) { return set.Get(); }
            public void SetCallbacks(IGameplayActions instance)
            {
                if (m_Wrapper.m_GameplayActionsCallbackInterface != null)
                {
                    @Look.started -= m_Wrapper.m_GameplayActionsCallbackInterface.OnLook;
                    @Look.performed -= m_Wrapper.m_GameplayActionsCallbackInterface.OnLook;
                    @Look.canceled -= m_Wrapper.m_GameplayActionsCallbackInterface.OnLook;
                    @Move.started -= m_Wrapper.m_GameplayActionsCallbackInterface.OnMove;
                    @Move.performed -= m_Wrapper.m_GameplayActionsCallbackInterface.OnMove;
                    @Move.canceled -= m_Wrapper.m_GameplayActionsCallbackInterface.OnMove;
                    @Run.started -= m_Wrapper.m_GameplayActionsCallbackInterface.OnRun;
                    @Run.performed -= m_Wrapper.m_GameplayActionsCallbackInterface.OnRun;
                    @Run.canceled -= m_Wrapper.m_GameplayActionsCallbackInterface.OnRun;
                    @Roll.started -= m_Wrapper.m_GameplayActionsCallbackInterface.OnRoll;
                    @Roll.performed -= m_Wrapper.m_GameplayActionsCallbackInterface.OnRoll;
                    @Roll.canceled -= m_Wrapper.m_GameplayActionsCallbackInterface.OnRoll;
                    @MainAttack.started -= m_Wrapper.m_GameplayActionsCallbackInterface.OnMainAttack;
                    @MainAttack.performed -= m_Wrapper.m_GameplayActionsCallbackInterface.OnMainAttack;
                    @MainAttack.canceled -= m_Wrapper.m_GameplayActionsCallbackInterface.OnMainAttack;
                    @SecondAttack.started -= m_Wrapper.m_GameplayActionsCallbackInterface.OnSecondAttack;
                    @SecondAttack.performed -= m_Wrapper.m_GameplayActionsCallbackInterface.OnSecondAttack;
                    @SecondAttack.canceled -= m_Wrapper.m_GameplayActionsCallbackInterface.OnSecondAttack;
                    @Skill.started -= m_Wrapper.m_GameplayActionsCallbackInterface.OnSkill;
                    @Skill.performed -= m_Wrapper.m_GameplayActionsCallbackInterface.OnSkill;
                    @Skill.canceled -= m_Wrapper.m_GameplayActionsCallbackInterface.OnSkill;
                }
                m_Wrapper.m_GameplayActionsCallbackInterface = instance;
                if (instance != null)
                {
                    @Look.started += instance.OnLook;
                    @Look.performed += instance.OnLook;
                    @Look.canceled += instance.OnLook;
                    @Move.started += instance.OnMove;
                    @Move.performed += instance.OnMove;
                    @Move.canceled += instance.OnMove;
                    @Run.started += instance.OnRun;
                    @Run.performed += instance.OnRun;
                    @Run.canceled += instance.OnRun;
                    @Roll.started += instance.OnRoll;
                    @Roll.performed += instance.OnRoll;
                    @Roll.canceled += instance.OnRoll;
                    @MainAttack.started += instance.OnMainAttack;
                    @MainAttack.performed += instance.OnMainAttack;
                    @MainAttack.canceled += instance.OnMainAttack;
                    @SecondAttack.started += instance.OnSecondAttack;
                    @SecondAttack.performed += instance.OnSecondAttack;
                    @SecondAttack.canceled += instance.OnSecondAttack;
                    @Skill.started += instance.OnSkill;
                    @Skill.performed += instance.OnSkill;
                    @Skill.canceled += instance.OnSkill;
                }
            }
        }
        public GameplayActions @Gameplay => new GameplayActions(this);
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
        public interface IGameplayActions
        {
            void OnLook(InputAction.CallbackContext context);
            void OnMove(InputAction.CallbackContext context);
            void OnRun(InputAction.CallbackContext context);
            void OnRoll(InputAction.CallbackContext context);
            void OnMainAttack(InputAction.CallbackContext context);
            void OnSecondAttack(InputAction.CallbackContext context);
            void OnSkill(InputAction.CallbackContext context);
        }
    }
}
