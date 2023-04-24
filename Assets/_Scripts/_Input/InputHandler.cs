using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace _Scripts.Input
{
    public class InputHandler : MonoBehaviour
    {
        public static Vector2 CourserPos;
        public static event Action UpKeyAction;
        public static event Action DownKeyAction;
        public static event Action LeftKeyAction;
        public static event Action RightKeyAction;
        
        public void OnAim(InputAction.CallbackContext ctx)
        {
            CourserPos = ctx.ReadValue<Vector2>();
        }

        public void OnUpKey(InputAction.CallbackContext ctx)
        {
            if (ctx.performed) UpKeyAction?.Invoke();
        }
    
        public void OnDownKey(InputAction.CallbackContext ctx)
        {
            if (ctx.performed) DownKeyAction?.Invoke();
        }
    
        public void OnLeftKey(InputAction.CallbackContext ctx)
        {
            if (ctx.performed) LeftKeyAction?.Invoke();
        }
    
        public void OnRightKey(InputAction.CallbackContext ctx)
        {
            if (ctx.performed) RightKeyAction?.Invoke();
        }

    }
}
