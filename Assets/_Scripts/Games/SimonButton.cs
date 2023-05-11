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
        private static readonly Dictionary<GameObject, (Action listener, Action silencer)> simonActions = new();
        private Color originalColor, targetColor;
        private SpriteRenderer spriteRenderer;
        private readonly float transitionDuration = 0.15f;

        /// <summary>
        /// Reacts on button press events and simulates such.
        /// Sending upwards calls to the parent object.
        /// </summary>
        public void ButtonPressed()
        {
            Animate();
            SendMessageUpwards("CheckColor", gameObject);
        }

        public void Animate()
        {
            StartCoroutine(AnimateColor(spriteRenderer, originalColor, targetColor, transitionDuration));
        }

        public void Animate(Color target)
        {
            StartCoroutine(AnimateColor(spriteRenderer, originalColor, target, transitionDuration));
        }

        /// <summary>
        /// Enables user Input for this button by adding it to the InputHandler.
        /// </summary>
        /// <param name="button">The object to allow Input for.</param>
        /// <param name="state">The state to set to: on or off.</param>
        public void ToggleInput(GameObject button, bool state)
        {
            if (button == null || !simonActions.ContainsKey(button)) return;
            if (state)
            {
                simonActions[button].listener();
            }
            else
            {
                simonActions[button].silencer();
            }
        }

        /// <summary>
        /// Fakes a blinking animation of the button color.
        /// </summary>
        /// <returns></returns>
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

            // Adds lambda expressions as anonymous functions to the Dictionary
            if (gameObject.name.Equals("BLUE"))
            {
                simonActions.Add(gameObject, (
                    () => InputHandler.UpArrowBtnAction += ButtonPressed,
                    () => InputHandler.UpArrowBtnAction -= ButtonPressed
                ));
            }
            if (gameObject.name.Equals("RED"))
            {
                simonActions.Add(gameObject, (
                    () => InputHandler.RightArrowBtnAction += ButtonPressed,
                    () => InputHandler.RightArrowBtnAction -= ButtonPressed
                ));
            }
            if (gameObject.name.Equals("YELLOW"))
            {
                simonActions.Add(gameObject, (
                    () => InputHandler.DownArrowBtnAction += ButtonPressed,
                    () => InputHandler.DownArrowBtnAction -= ButtonPressed
                ));
            }
            if (gameObject.name.Equals("GREEN"))
            {
                simonActions.Add(gameObject, (
                    () => InputHandler.LeftArrowBtnAction += ButtonPressed,
                    () => InputHandler.LeftArrowBtnAction -= ButtonPressed
                ));
            }
        }

        private void OnEnable()
        {
            Animate();
        }

        private void OnDisable()
        {
            ToggleInput(gameObject, false);
        }
    }
}

