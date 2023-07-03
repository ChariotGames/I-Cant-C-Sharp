using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Scripts.Games
{
    /// <summary>
    /// Represents a single magic ring of the MagicCircle minigame.
    /// </summary>
    public class MagicRing : BaseGame
    {
        #region Serialized Fields

        [SerializeField] private MagicCircle parent;
        [SerializeField] private LineRenderer line;
        [SerializeField] [Range(0, 1)] private float timer = 0.5f;
        [SerializeField] [Range(64, 128)] private int lineSegments = 64;

        #endregion Serialized Fields

        #region Fields

        private InputActionReference[] _buttons;
        private Color _ringColor;
        private Vector3 _offset;
        private float _radius = 4f;

        #endregion Fields

        #region Built-Ins / MonoBehaviours

        void Update()
        {
            if (_radius < 0.25)
            {
                parent.EvaluateResult(gameObject, false);
            }

            DrawRing(lineSegments, _radius -= Time.deltaTime * timer);
        }

        private void OnDisable()
        {
            for (int i = 0; i < _buttons.Length; i++)
            {
                _buttons[i].action.performed -= ButtonPressed;
            }
        }

        #endregion Built-Ins / MonoBehaviours

        #region GetSets / Properties



        #endregion GetSets / Properties

        #region Game Mechanics / Methods

        /// <summary>
        /// Switches the event listeners for the buttons.
        /// <param name="state">The state to set button listeners to.</param>
        /// </summary>
        public void ToggleListeners(bool state)
        {
            for (int i = 0; i < _buttons.Length; i++)
            {
                if(state)
                {
                    _buttons[i].action.performed += ButtonPressed;
                }
                else
                {
                    _buttons[i].action.performed -= ButtonPressed;
                }
            }
        }

        /// <summary>
        /// Sets up the properties of a ring.
        /// </summary>
        /// <param name="parentOffset">The position offset of the game container.</param>
        /// <param name="color">The color of the ring to display.</param>
        /// <param name="inputs">The button references to listen for.</param>
        public void Setup(Vector3 parentOffset, Color color, InputActionReference[] inputs)
        {
            _offset = parentOffset;
            _ringColor = color;
            _buttons = inputs;
        }

        #endregion Game Mechanics / Methods

        #region Overarching Methods / Helpers

        /// <summary>
        /// Callback function on button press. The 0th button is always correct!
        /// </summary>
        /// <param name="ctx">Input event binding information.</param>
        private void ButtonPressed(InputAction.CallbackContext ctx)
        {
            parent.EvaluateResult(gameObject, ctx.action == _buttons[0].action && _radius <= 0.75 && _radius >= 0.25);
        }

        /// <summary>
        /// Actually draw a shrinking ring over time.
        /// </summary>
        /// <param name="steps">Number of line segments.</param>
        /// <param name="radius">The ring radius.</param>
        private void DrawRing(int steps, float radius)
        {
            line.startColor = line.endColor = _ringColor;
            line.positionCount = steps;

            for (int i = 0; i < steps; i++)
            {
                float circleProgress = (float)i / steps;
                float currentRadians = circleProgress * 2 * Mathf.PI;
                float xScaled = Mathf.Cos(currentRadians);
                float yScaled = Mathf.Sin(currentRadians);

                float x = xScaled * radius;
                float y = yScaled * radius;

                Vector3 currentPosition = new Vector3(x, y, 0) + _offset;
                line.SetPosition(i, currentPosition);
            }
        }

        #endregion Overarching Methods / Helpers
    }
}