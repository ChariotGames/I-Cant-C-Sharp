using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

namespace _Scripts.Games
{
    /// <summary>
    /// Represents a single element of the Simon game
    /// and their respective functionalities.
    /// </summary>
    public class SimonElement : BasePressElement
    {
        #region Serialized Fields

        [SerializeField] private Color originalColor, targetColor;
        [SerializeField] private Colors colorEnum;
        [SerializeField] private SpriteRenderer spriteRenderer;
        [SerializeField] [Range(0.1f, 1)] private float duration = 0.30f;

        #endregion Serialized Fields

        #region Fields

        private Simon _game;

        #endregion Fields

        #region Built-Ins

        void Awake()
        {
            _game = (Simon)parent;
        }

        private void OnEnable()
        {
            if (button != null) ToggleInput(true);
        }

        private void OnDisable()
        {
            if (button != null) ToggleInput(false);
        }

        #endregion Built-Ins

        #region Game Mechanics / Methods

        /// <summary>
        /// Reacts on button press events and simulates such.
        /// Calls the parent check method.
        /// </summary>
        /// <param name="ctx">Input event binding information.</param>
        public override void ButtonPressed(InputAction.CallbackContext ctx)
        {
            Animate();
            _game.CheckColor(colorEnum);
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

        #endregion Game Mechanics / Methods

        #region Animations

        /// <summary>
        /// Animates blinking of a single button.
        /// </summary>
        public void Animate()
        {
            StartCoroutine(AnimateColor(spriteRenderer, originalColor, targetColor, duration));
        }

        /// <summary>
        /// Fakes a blinking animation of the button color.
        /// </summary>
        /// <returns>An object that can be used to control the coroutine's execution.</returns>
        private IEnumerator AnimateColor(SpriteRenderer sprite, Color original, Color target, float duration)
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

