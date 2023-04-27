//------------------------------------------------------------------------------
// <auto-generated>
//     This code was auto-generated by com.unity.inputsystem:InputActionCodeGenerator
//     version 1.5.1
//     from Assets/_Scripts/_Input/BasicControls.inputactions
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

public partial class @BasicControls: IInputActionCollection2, IDisposable
{
    public InputActionAsset asset { get; }
    public @BasicControls()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""BasicControls"",
    ""maps"": [
        {
            ""name"": ""Player"",
            ""id"": ""d43f1f2f-86a2-44c4-aeff-564bff731131"",
            ""actions"": [
                {
                    ""name"": ""LeftKey"",
                    ""type"": ""Button"",
                    ""id"": ""d2882894-50db-415d-ab9d-2c1145133d77"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""RightKey"",
                    ""type"": ""Button"",
                    ""id"": ""ec05d8e3-c080-4820-b195-11dfcefe27b1"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""DownKey"",
                    ""type"": ""Button"",
                    ""id"": ""f765fc6d-3e9f-40c1-89bb-db9af6a3a021"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""UpKey"",
                    ""type"": ""Button"",
                    ""id"": ""80c84240-95f9-43b8-83da-43e3771b6729"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""CourserPos"",
                    ""type"": ""Value"",
                    ""id"": ""0fa21c14-e7f5-41c7-b325-8a2ec38ebeb1"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""db9d7a41-941c-4dcc-b10d-9dbcaafc4336"",
                    ""path"": ""<Keyboard>/leftArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""LeftKey"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""e451b109-77b8-4216-8365-8a844a3b23b0"",
                    ""path"": ""<Gamepad>/dpad/left"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""LeftKey"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""89d1e898-0480-40b3-b256-87374d875a61"",
                    ""path"": ""<Keyboard>/rightArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""RightKey"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""c561a7c9-aaa8-42ca-b16e-b0f29d7e25f1"",
                    ""path"": ""<Gamepad>/dpad/right"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""RightKey"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""dfcf399e-897e-40fe-bc46-158b49d0341f"",
                    ""path"": ""<Keyboard>/downArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""DownKey"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""8becb381-2c28-4468-ac3d-ff5f68ac2cce"",
                    ""path"": ""<Gamepad>/dpad/down"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""DownKey"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""66fa2b99-f732-4489-a5e9-1cf0ec65f4a2"",
                    ""path"": ""<Keyboard>/upArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""UpKey"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""43f59fff-e19f-4c2b-866d-4b3b3649ef27"",
                    ""path"": ""<Gamepad>/dpad/up"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""UpKey"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""ee1e9a82-c8b8-4bc5-ab2b-0fd1c862a879"",
                    ""path"": ""<Mouse>/delta"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""CourserPos"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""f142da81-8875-4103-982d-f1bede149e95"",
                    ""path"": ""<Gamepad>/rightStick"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""CourserPos"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // Player
        m_Player = asset.FindActionMap("Player", throwIfNotFound: true);
        m_Player_LeftKey = m_Player.FindAction("LeftKey", throwIfNotFound: true);
        m_Player_RightKey = m_Player.FindAction("RightKey", throwIfNotFound: true);
        m_Player_DownKey = m_Player.FindAction("DownKey", throwIfNotFound: true);
        m_Player_UpKey = m_Player.FindAction("UpKey", throwIfNotFound: true);
        m_Player_CourserPos = m_Player.FindAction("CourserPos", throwIfNotFound: true);
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

    // Player
    private readonly InputActionMap m_Player;
    private List<IPlayerActions> m_PlayerActionsCallbackInterfaces = new List<IPlayerActions>();
    private readonly InputAction m_Player_LeftKey;
    private readonly InputAction m_Player_RightKey;
    private readonly InputAction m_Player_DownKey;
    private readonly InputAction m_Player_UpKey;
    private readonly InputAction m_Player_CourserPos;
    public struct PlayerActions
    {
        private @BasicControls m_Wrapper;
        public PlayerActions(@BasicControls wrapper) { m_Wrapper = wrapper; }
        public InputAction @LeftKey => m_Wrapper.m_Player_LeftKey;
        public InputAction @RightKey => m_Wrapper.m_Player_RightKey;
        public InputAction @DownKey => m_Wrapper.m_Player_DownKey;
        public InputAction @UpKey => m_Wrapper.m_Player_UpKey;
        public InputAction @CourserPos => m_Wrapper.m_Player_CourserPos;
        public InputActionMap Get() { return m_Wrapper.m_Player; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(PlayerActions set) { return set.Get(); }
        public void AddCallbacks(IPlayerActions instance)
        {
            if (instance == null || m_Wrapper.m_PlayerActionsCallbackInterfaces.Contains(instance)) return;
            m_Wrapper.m_PlayerActionsCallbackInterfaces.Add(instance);
            @LeftKey.started += instance.OnLeftKey;
            @LeftKey.performed += instance.OnLeftKey;
            @LeftKey.canceled += instance.OnLeftKey;
            @RightKey.started += instance.OnRightKey;
            @RightKey.performed += instance.OnRightKey;
            @RightKey.canceled += instance.OnRightKey;
            @DownKey.started += instance.OnDownKey;
            @DownKey.performed += instance.OnDownKey;
            @DownKey.canceled += instance.OnDownKey;
            @UpKey.started += instance.OnUpKey;
            @UpKey.performed += instance.OnUpKey;
            @UpKey.canceled += instance.OnUpKey;
            @CourserPos.started += instance.OnCourserPos;
            @CourserPos.performed += instance.OnCourserPos;
            @CourserPos.canceled += instance.OnCourserPos;
        }

        private void UnregisterCallbacks(IPlayerActions instance)
        {
            @LeftKey.started -= instance.OnLeftKey;
            @LeftKey.performed -= instance.OnLeftKey;
            @LeftKey.canceled -= instance.OnLeftKey;
            @RightKey.started -= instance.OnRightKey;
            @RightKey.performed -= instance.OnRightKey;
            @RightKey.canceled -= instance.OnRightKey;
            @DownKey.started -= instance.OnDownKey;
            @DownKey.performed -= instance.OnDownKey;
            @DownKey.canceled -= instance.OnDownKey;
            @UpKey.started -= instance.OnUpKey;
            @UpKey.performed -= instance.OnUpKey;
            @UpKey.canceled -= instance.OnUpKey;
            @CourserPos.started -= instance.OnCourserPos;
            @CourserPos.performed -= instance.OnCourserPos;
            @CourserPos.canceled -= instance.OnCourserPos;
        }

        public void RemoveCallbacks(IPlayerActions instance)
        {
            if (m_Wrapper.m_PlayerActionsCallbackInterfaces.Remove(instance))
                UnregisterCallbacks(instance);
        }

        public void SetCallbacks(IPlayerActions instance)
        {
            foreach (var item in m_Wrapper.m_PlayerActionsCallbackInterfaces)
                UnregisterCallbacks(item);
            m_Wrapper.m_PlayerActionsCallbackInterfaces.Clear();
            AddCallbacks(instance);
        }
    }
    public PlayerActions @Player => new PlayerActions(this);
    public interface IPlayerActions
    {
        void OnLeftKey(InputAction.CallbackContext context);
        void OnRightKey(InputAction.CallbackContext context);
        void OnDownKey(InputAction.CallbackContext context);
        void OnUpKey(InputAction.CallbackContext context);
        void OnCourserPos(InputAction.CallbackContext context);
    }
}
