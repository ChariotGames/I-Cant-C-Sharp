using UnityEngine;
using UnityEngine.InputSystem;

namespace _Scripts.Games
{
    /// <summary>
    /// Represents a single button of the Simon game
    /// and their respective functionalities.
    /// </summary>
    public class SimonButton : SimonElement
    {
        #region Serialized Fields

        [SerializeField] private Colors buttonColor;
        [SerializeField] private InputActionReference buttonAction;
        [SerializeField] private Simon parent;

        #endregion Serialized Fields

        #region Built-Ins

        private void Awake()
        {
            if(buttonAction != null) ToggleInput(true);
        }

        private void OnDisable()
        {
            if (buttonAction != null) ToggleInput(false);
        }

        #endregion

        #region Button Controls

        /// <summary>
        /// Reacts on button press events and simulates such.
        /// Sending upwards calls to the parent object.
        /// </summary>
        public void ButtonPressed(InputAction.CallbackContext ctx)
        {
            Animate();
            parent.CheckColor(buttonColor);
        }

        /// <summary>
        /// Enables user Input for this button by adding it to the InputHandler.
        /// </summary>
        /// <param name="state">The state to set to: on or off.</param>
        public void ToggleInput(bool state)
        {
            if (state)
            {
                buttonAction.action.performed += ButtonPressed;
                buttonAction.action.Enable();
            }
            else
            {
                buttonAction.action.performed -= ButtonPressed;
                buttonAction.action.Disable();
            }
        }
        #endregion Button Controls
    }
}

