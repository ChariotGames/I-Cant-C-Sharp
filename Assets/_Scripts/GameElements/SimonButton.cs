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

        [SerializeField] private Colors color;
        [SerializeField] private InputActionReference button;
        [SerializeField] private Simon parent;

        #endregion Serialized Fields

        #region Built-Ins

        private void OnEnable()
        {
            if(button != null) ToggleInput(true);
        }

        private void OnDisable()
        {
            if (button != null) ToggleInput(false);
        }

        #endregion

        #region Button Controls

        /// <summary>
        /// Reacts on button press events and simulates such.
        /// Calls the parent check method.
        /// </summary>
        public void ButtonPressed(InputAction.CallbackContext ctx)
        {
            Animate();
            parent.CheckColor(color);
        }

        /// <summary>
        /// Enables user Input for this button by adding it to the InputHandler.
        /// </summary>
        /// <param name="state">The state to set to: on or off.</param>
        public void ToggleInput(bool state)
        {
            if (state)
            {
                button.action.performed += ButtonPressed;
            }
            else
            {
                button.action.performed -= ButtonPressed;
            }
        }
        #endregion Button Controls
    }
}

