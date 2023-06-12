using UnityEngine;
using UnityEngine.InputSystem;

namespace _Scripts.Games
{
    /// <summary>
    /// Represents an abstract idea of a button element
    /// that needs input listeners.
    /// Encapsulates the basic values and callback functions
    /// each game should have to function.
    /// </summary>
    public abstract class BasePressElement : MonoBehaviour
    {
        #region Serialized Fields

        [SerializeField] protected BaseGame parent;
        [SerializeField] protected InputActionReference button;

        #endregion Serialized Fields

        #region Built-Ins / MonoBehaviours

        void OnEnable()
        {
            button.action.performed += ButtonPressed;
        }

        void OnDisable()
        {
            button.action.performed -= ButtonPressed;
        }

        #endregion Built-Ins / MonoBehaviours

        #region Game Mechanics / Methods

        public abstract void ButtonPressed(InputAction.CallbackContext ctx);

        #endregion Game Mechanics / Methods
    }
}