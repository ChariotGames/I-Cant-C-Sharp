using _Scripts._Input;
using _Scripts._Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _Scripts.Games
{
    public class SimonButton : MonoBehaviour, IButton
    {
        private static readonly Dictionary<Simon, (Action listener, Action silencer)> simonActions = new();
        private Color originalColor, targetColor;
        private SpriteRenderer spriteRenderer;
        private const float DURATION = 0.30f;

        /// <summary>
        /// Reacts on button press events and simulates such.
        /// Sending upwards calls to the parent object.
        /// </summary>
        public void ButtonPressed()
        {
            Animate();
            if(Enum.TryParse<Simon>(gameObject.name, true, out Simon key)) SendMessageUpwards("CheckColor", key);
        }

        public void Animate()
        {
            StartCoroutine(AnimateColor(spriteRenderer, originalColor, targetColor, DURATION));
        }

        /// <summary>
        /// Enables user Input for this button by adding it to the InputHandler.
        /// </summary>
        /// <param name="button">The object to allow Input for.</param>
        /// <param name="state">The state to set to: on or off.</param>
        public void ToggleInput(GameObject button, bool state)
        {
            Enum.TryParse<Simon>(button.name, true, out Simon key);
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

        private void Awake()
        {
            spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
            originalColor = spriteRenderer.color;
            targetColor = originalColor + originalColor;
            if (targetColor == new Color(2, 2, 2, 0)) targetColor += new Color(1, 1, 1, 1);

            // Adds lambda expressions as anonymous functions to the Dictionary
            if (gameObject.name.Equals(Simon.BLUE.ToString()))
            {
                simonActions.Add(Simon.BLUE, (
                    () => InputHandler.UpArrowBtnAction += ButtonPressed,
                    () => InputHandler.UpArrowBtnAction -= ButtonPressed
                ));
            }
            if (gameObject.name.Equals(Simon.RED.ToString()))
            {
                simonActions.Add(Simon.RED, (
                    () => InputHandler.RightArrowBtnAction += ButtonPressed,
                    () => InputHandler.RightArrowBtnAction -= ButtonPressed
                ));
            }
            if (gameObject.name.Equals(Simon.YELLOW.ToString()))
            {
                simonActions.Add(Simon.YELLOW, (
                    () => InputHandler.DownArrowBtnAction += ButtonPressed,
                    () => InputHandler.DownArrowBtnAction -= ButtonPressed
                ));
            }
            if (gameObject.name.Equals(Simon.GREEN.ToString()))
            {
                simonActions.Add(Simon.GREEN, (
                    () => InputHandler.LeftArrowBtnAction += ButtonPressed,
                    () => InputHandler.LeftArrowBtnAction -= ButtonPressed
                ));
            }
        }

        private void OnDisable()
        {
            ToggleInput(gameObject, false);
        }
    }
}

