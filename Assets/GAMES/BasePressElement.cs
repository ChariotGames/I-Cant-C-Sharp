using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Scripts.Games
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

        public InputActionReference Button;

        #endregion Serialized Fields

        #region Built-Ins / MonoBehaviours

        void OnEnable()
        {
            Button.action.performed += ButtonPressed;
        }

        void OnDisable()
        {
            Button.action.performed -= ButtonPressed;
        }

        #endregion Built-Ins / MonoBehaviours

        #region Game Mechanics / Methods

        public abstract void ButtonPressed(InputAction.CallbackContext ctx);

        #endregion Game Mechanics / Methods

        #region Animations

        /// <summary>
        /// Fakes a blinking animation of the button color.
        /// </summary>
        /// <returns>An object that can be used to control the coroutine's execution.</returns>
        protected IEnumerator AnimateColor(SpriteRenderer sprite, Color original, Color target, float duration)
        {
            float elapsedTime = 0f;
            while (elapsedTime < duration)
            {
                sprite.color = Color.Lerp(original, target, elapsedTime / duration);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            elapsedTime = 0f;
            while (elapsedTime < duration)
            {
                sprite.color = Color.Lerp(target, original, elapsedTime / duration);
                elapsedTime += Time.deltaTime;
                yield return null;
            }
            sprite.color = original;
        }

        #endregion Animations
    }
}