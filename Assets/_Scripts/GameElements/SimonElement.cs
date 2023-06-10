using _Scripts._Input;
using _Scripts._Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace _Scripts.Games
{
    /// <summary>
    /// Represents a single element of the Simon game
    /// and their respective functionalities.
    /// </summary>
    public class SimonElement : MonoBehaviour
    {
        #region Serialized Fields
        [SerializeField] private Color originalColor, targetColor;
        [SerializeField] private SpriteRenderer spriteRenderer;
        [SerializeField] [Range(0.1f, 1)] private float duration = 0.30f;

        #endregion Serialized Fields

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

