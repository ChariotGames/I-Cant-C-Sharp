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
        private static readonly Dictionary<Colors, (Action listener, Action silencer)> objectActions = new();
        private Color originalColor, targetColor;
        private SpriteRenderer spriteRenderer;
        private readonly float transitionDuration = 0.15f;

        /// <summary>
        /// Reacts on button press events and simulates such.
        /// Sending upwards calls to the parent object.
        /// </summary>
        public void ButtonPressed()
        {
            StartCoroutine(AnimateColor(spriteRenderer, originalColor, targetColor, transitionDuration));
            SendMessageUpwards("CheckColor", Enum.Parse(typeof(Colors), gameObject.name.ToUpper()));
        }

        /// <summary>
        /// Enables user Input for this button.
        /// </summary>
        /// <param name="name">The name of the object to allow Input for.</param>
        public void AllowInput(string name)
        {
            objectActions[(Colors)Enum.Parse(typeof(Colors), name.ToUpper())].listener();
        }

        /// <summary>
        /// Disables user Input for this button by removing it from the InputHandler.
        /// </summary>
        /// <param name="name">The name of the object to disable Input for.</param>
        public void DisableInput(string name)
        {
            objectActions[(Colors)Enum.Parse(typeof(Colors), name.ToUpper())].silencer();
        }

        /// <summary>
        /// Fakes a blinking animation of the button color
        /// </summary>
        /// <returns></returns>
        private IEnumerator AnimateColor(SpriteRenderer property, Color original, Color target, float duration)
        {
            float elapsedTime = 0f;
            while (elapsedTime < duration)
            {
                property.color = Color.Lerp(original, target, elapsedTime / duration);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            elapsedTime = 0f;
            while (elapsedTime < duration)
            {
                property.color = Color.Lerp(target, original, elapsedTime / duration);
                elapsedTime += Time.deltaTime;
                yield return null;
            }
            property.color = original;
        }

        private void Awake()
        {
            spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
            originalColor = spriteRenderer.color;
            targetColor = originalColor + originalColor;

            // Adds lambda expressions as anonymous functions to the Dictionary
            if (gameObject.name.Equals(Colors.BLUE.ToString()))
            {
                objectActions.Add(Colors.BLUE, (
                    () => InputHandler.UpArrowBtnAction += ButtonPressed,
                    () => InputHandler.UpArrowBtnAction -= ButtonPressed
                ));
            }
            if (gameObject.name.Equals(Colors.RED.ToString()))
            {
                objectActions.Add(Colors.RED, (
                    () => InputHandler.RightArrowBtnAction += ButtonPressed,
                    () => InputHandler.RightArrowBtnAction -= ButtonPressed
                ));
            }
            if (gameObject.name.Equals(Colors.YELLOW.ToString()))
            {
                objectActions.Add(Colors.YELLOW, (
                    () => InputHandler.DownArrowBtnAction += ButtonPressed,
                    () => InputHandler.DownArrowBtnAction -= ButtonPressed
                ));
            }
            if (gameObject.name.Equals(Colors.GREEN.ToString()))
            {
                objectActions.Add(Colors.GREEN, (
                    () => InputHandler.LeftArrowBtnAction += ButtonPressed,
                    () => InputHandler.LeftArrowBtnAction -= ButtonPressed
                ));
            }
        }

        private void OnEnable()
        {
            StartCoroutine(AnimateColor(spriteRenderer, originalColor, targetColor, transitionDuration));
        }

        private void OnDisable()
        {
            DisableInput(gameObject.name);
        }
    }
}

