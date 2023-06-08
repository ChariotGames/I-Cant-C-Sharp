using _Scripts._Input;
using _Scripts._Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _Scripts.Games
{
    /// <summary>
    /// Represents a single button of the Simon game
    /// and their respective functionalities.
    /// </summary>
    public class SimonButton : MonoBehaviour, IButton
    {
        #region Fields

        private static readonly Dictionary<Colors, (Action listener, Action silencer)> simonActions = new();
        private Color originalColor, targetColor;
        private SpriteRenderer spriteRenderer;
        private const float DURATION = 0.30f;

        #endregion Fields

        #region Built-Ins

        private void Awake()
        {
            spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
            originalColor = spriteRenderer.color;
            targetColor = originalColor + originalColor;
            if (targetColor == new Color(2, 2, 2, 0)) targetColor += new Color(1, 1, 1, 1);

            // Adds lambda expressions as anonymous functions to the Dictionary
            if (gameObject.name.Equals(Colors.BLUE.ToString()) &&
                !simonActions.ContainsKey(Colors.BLUE))
            {
                simonActions.Add(Colors.BLUE, (
                    (Action)(() => InputHandler.NorthBtnAction += ButtonPressed),
                    (Action)(() => InputHandler.NorthBtnAction -= ButtonPressed)
                ));
            }
            if (gameObject.name.Equals(Colors.RED.ToString()) &&
                !simonActions.ContainsKey(Colors.RED))
            {
                simonActions.Add(Colors.RED, (
                    (Action)(() => InputHandler.EastBtnAction += ButtonPressed),
                    (Action)(() => InputHandler.EastBtnAction -= ButtonPressed)
                ));
            }
            if (gameObject.name.Equals(Colors.YELLOW.ToString()) &&
                !simonActions.ContainsKey(Colors.YELLOW))
            {
                simonActions.Add(Colors.YELLOW, (
                    (Action)(() => InputHandler.SouthBtnAction += ButtonPressed),
                    (Action)(() => InputHandler.SouthBtnAction -= ButtonPressed)
                ));
            }
            if (gameObject.name.Equals(Colors.GREEN.ToString()) &&
                !simonActions.ContainsKey(Colors.GREEN))
            {
                simonActions.Add(Colors.GREEN, (
                    (Action)(() => InputHandler.WestBtnAction += ButtonPressed),
                    (Action)(() => InputHandler.WestBtnAction -= ButtonPressed)
                ));
            }
        }

        private void OnDisable()
        {
            ToggleInput(gameObject, false);
        }

        #endregion Built-Ins

        #region Button Controls

        /// <summary>
        /// Reacts on button press events and simulates such.
        /// Sending upwards calls to the parent object.
        /// </summary>
        public void ButtonPressed()
        {
            Animate();
            if (Enum.TryParse(gameObject.name, true, out Colors key)) SendMessageUpwards("CheckColor", key);
        }

        /// <summary>
        /// Enables user Input for this button by adding it to the InputHandler.
        /// </summary>
        /// <param name="button">The object to allow Input for.</param>
        /// <param name="state">The state to set to: on or off.</param>
        public void ToggleInput(GameObject button, bool state)
        {
            Enum.TryParse(button.name, true, out Colors key);
            if (!simonActions.ContainsKey(key)) return;
            if (state)
            {
                simonActions[key].listener();
            }
            else
            {
                simonActions[key].silencer();
            }
        }

        #endregion Button Controls

        #region Animations

        /// <summary>
        /// Animates blinking of a single button.
        /// </summary>
        public void Animate()
        {
            StartCoroutine(AnimateColor(spriteRenderer, originalColor, targetColor, DURATION));
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

