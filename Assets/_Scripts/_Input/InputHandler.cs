using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace _Scripts._Input
{
    public class InputHandler : MonoBehaviour
    {
        public static Vector2 RightStickDelta;
        public static Vector2 LeftStickDelta;
        
        public static event Action UpArrowBtnAction;
        public static event Action DownArrowBtnAction;
        public static event Action LeftArrowBtnAction;
        public static event Action RightArrowBtnAction;

        public static event Action NorthBtnAction;
        public static event Action EastBtnAction;
        public static event Action SouthBtnAction;
        public static event Action WestBtnAction;
        
        public static event Action RightShoulderBtnAction;
        public static event Action RightTriggerBtnAction;
        
        public static event Action LeftShoulderBtnAction;
        public static event Action LeftTriggerBtnAction;
        
        public static event Action RightStickPressAction;
        public static event Action LeftStickPressAction;
        
        
        
        public void OnRightStick(InputAction.CallbackContext ctx)
        {
            RightStickDelta = ctx.ReadValue<Vector2>();
            //Debug.Log("RightStickDelta" + RightStickDelta);
        }
        
        public void OnLeftStick(InputAction.CallbackContext ctx)
        {
            LeftStickDelta = ctx.ReadValue<Vector2>();
            //Debug.Log("LeftStickDelta" + LeftStickDelta);
        }

        public void OnUpButton(InputAction.CallbackContext ctx)
        {
            if (ctx.performed) UpArrowBtnAction?.Invoke();
            //Debug.Log("UpArrowAction");
        }
    
        public void OnDownButton(InputAction.CallbackContext ctx)
        {
            if (ctx.performed) DownArrowBtnAction?.Invoke();
            //Debug.Log("DownArrowAction");
        }
    
        public void OnLeftButton(InputAction.CallbackContext ctx)
        {
            if (ctx.performed) LeftArrowBtnAction?.Invoke();
            //Debug.Log("LeftArrowAction");
        }
    
        public void OnRightButton(InputAction.CallbackContext ctx)
        {
            if (ctx.performed) RightArrowBtnAction?.Invoke();
            //Debug.Log("RightArrowAction");
        }
        
        public void OnNorthButton(InputAction.CallbackContext ctx)
        {
            if (ctx.performed) NorthBtnAction?.Invoke();
            //Debug.Log("NorthBtnAction");
        }
        
        public void OnEastButton(InputAction.CallbackContext ctx)
        {
            if (ctx.performed) EastBtnAction?.Invoke();
            //Debug.Log("EastBtnAction");
        }
        
        public void OnSouthButton(InputAction.CallbackContext ctx)
        {
            if (ctx.performed) SouthBtnAction?.Invoke();
            //Debug.Log("SouthBtnAction");
        }
        
        public void OnWestButton(InputAction.CallbackContext ctx)
        {
            if (ctx.performed) WestBtnAction?.Invoke();
            //Debug.Log("WestBtnAction");
        }
        
        public void OnLeftShoulderButton(InputAction.CallbackContext ctx)
        {
            if (ctx.performed) LeftShoulderBtnAction?.Invoke();
            //Debug.Log("LeftShoulderAction");
        }
        
        public void OnLeftTriggerButton(InputAction.CallbackContext ctx)
        {
            if (ctx.performed) LeftTriggerBtnAction?.Invoke();
            //Debug.Log("LeftTriggerAction");
        }
        
        public void OnRightShoulderButton(InputAction.CallbackContext ctx)
        {
            if (ctx.performed) RightShoulderBtnAction?.Invoke();
            //Debug.Log("RightShoulderAction");
        }
        
        public void OnRightTriggerButton(InputAction.CallbackContext ctx)
        {
            if (ctx.performed) RightTriggerBtnAction?.Invoke();
            //Debug.Log("RightTriggerAction");
        }
        
        public void OnLeftStickPress(InputAction.CallbackContext ctx)
        {
            if (ctx.performed) LeftStickPressAction?.Invoke();
            //Debug.Log("LeftStickPressAction");
        }
        
        public void OnRightStickPress(InputAction.CallbackContext ctx)
        {
            if (ctx.performed) RightStickPressAction?.Invoke();
            //Debug.Log("RightStickPressAction");
        }

    }
}
