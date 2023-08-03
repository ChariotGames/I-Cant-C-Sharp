using Scripts.Models;
using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Scripts.Games
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
        [SerializeField] private SpriteRenderer spriteRenderer, outline;
        [SerializeField] [Range(0.1f, 1)] private float duration = 0.30f;
        [SerializeField] private AudioSource note;

        #endregion Serialized Fields

        #region Fields

        private SimonSays _game;

        #endregion Fields

        #region Built-Ins

        void Awake()
        {
            _game = (SimonSays)parent;
        }

        private void OnEnable()
        {
            if (Button != null) ToggleInput(true);
        }

        private void OnDisable()
        {
            if (Button != null) ToggleInput(false);
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
                Button.action.performed += ButtonPressed;
            }
            else
            {
                Button.action.performed -= ButtonPressed;
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
            if(note != null)
                note.Play();
            if(outline != null)
                StartCoroutine(AnimateColor(outline, originalColor, targetColor, duration));
        }

        #endregion Animations
    }
}

