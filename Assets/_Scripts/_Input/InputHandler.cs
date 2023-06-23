using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace _Scripts._Input
{
    public class InputHandler : MonoBehaviour
    {
        #region Action Fields
        public static Vector2 StickRight, StickLeft;

        public static event Action StickButtonRight, StickButtonLeft;

        public static event Action ArrowUp, ArrowDown, ArrowLeft, ArrowRight;

        public static event Action ButtonNorth,ButtonEast, ButtonSouth, ButtonWest;
        
        public static event Action ShoulderRight, ShoulderLeft;

        public static event Action TriggerRight, TriggerLeft;

        #endregion Action Fields

        #region Callback Methods

        #region Sticks

        public void OnStickRight(InputAction.CallbackContext ctx)
        {
            StickRight = ctx.ReadValue<Vector2>();
            //Debug.Log("StickRight" + StickRight);
        }
        
        public void OnStickLeft(InputAction.CallbackContext ctx)
        {
            StickLeft = ctx.ReadValue<Vector2>();
            //Debug.Log("StickLeft" + StickLeft);
        }

        public void OnStickButtonLeft(InputAction.CallbackContext ctx)
        {
            if (ctx.performed) StickButtonLeft?.Invoke();
            //Debug.Log("StickButtonLeft");
        }

        public void OnStickButtonRight(InputAction.CallbackContext ctx)
        {
            if (ctx.performed) StickButtonRight?.Invoke();
            //Debug.Log("StickButtonRight");
        }

        #endregion Sticks

        #region Arrows

        public void OnArrowUp(InputAction.CallbackContext ctx)
        {
            if (ctx.performed) ArrowUp?.Invoke();
            //Debug.Log("UpArrowAction");
        }
    
        public void OnArrowDown(InputAction.CallbackContext ctx)
        {
            if (ctx.performed) ArrowDown?.Invoke();
            //Debug.Log("DownArrowAction");
        }
    
        public void OnArrowLeft(InputAction.CallbackContext ctx)
        {
            if (ctx.performed) ArrowLeft?.Invoke();
            //Debug.Log("LeftArrowAction");
        }
    
        public void OnArrowRight(InputAction.CallbackContext ctx)
        {
            if (ctx.performed) ArrowRight?.Invoke();
            //Debug.Log("RightArrowAction");
        }

        #endregion Arrows

        #region Face Buttons

        public void OnButtonNorth(InputAction.CallbackContext ctx)
        {
            if (ctx.performed) ButtonNorth?.Invoke();
            //Debug.Log("ButtonNorth");
        }

        public void OnButtonSouth(InputAction.CallbackContext ctx)
        {
            if (ctx.performed) ButtonSouth?.Invoke();
            //Debug.Log("ButtonSouth");
        }

        public void OnButtonEast(InputAction.CallbackContext ctx)
        {
            if (ctx.performed) ButtonEast?.Invoke();
            //Debug.Log("ButtonEast");
        }
        
        public void OnButtonWest(InputAction.CallbackContext ctx)
        {
            if (ctx.performed) ButtonWest?.Invoke();
            //Debug.Log("ButtonWest");
        }

        #endregion Face Buttons

        #region Shoulder & Trigger

        public void OnShoulderLeft(InputAction.CallbackContext ctx)
        {
            if (ctx.performed) ShoulderLeft?.Invoke();
            //Debug.Log("LeftShoulderAction");
        }
        public void OnShoulderRight(InputAction.CallbackContext ctx)
        {
            if (ctx.performed) ShoulderRight?.Invoke();
            //Debug.Log("RightShoulderAction");
        }

        public void OnTriggerLeft(InputAction.CallbackContext ctx)
        {
            if (ctx.performed) TriggerLeft?.Invoke();
            //Debug.Log("LeftTriggerAction");
        }
        
        public void OnTriggerRight(InputAction.CallbackContext ctx)
        {
            if (ctx.performed) TriggerRight?.Invoke();
            //Debug.Log("RightTriggerAction");
        }

        #endregion Shoulder & Trigger

        #endregion Callback Methods
    }
}
