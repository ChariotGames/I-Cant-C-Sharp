using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Scripts.Games
{
    public class KonamiCode : MonoBehaviour
    {
        #region Serialized Fields

        [SerializeField] private InputActionReference[] allowedButtons;
        [SerializeField] private string code = "UUDDLRLRWN";

        #endregion Serialized Fields

        #region Fields

        private string[] codes;
        private int _index;

        #endregion Fields

        #region Built-Ins / MonoBehaviours

        void OnEnable()
        {
            Restart();

            foreach (InputActionReference button in allowedButtons)
                button.action.performed += ButtonPressed;
        }

        void OnDisable()
        {
            foreach (InputActionReference button in allowedButtons)
                button.action.performed -= ButtonPressed;
        }

        #endregion Built-Ins / MonoBehaviours

        #region Game Mechanics / Methods

        private void ButtonPressed(InputAction.CallbackContext ctx)
        {
            
            if (!ctx.action.name.Contains(codes[_index])) Restart();
            
            _index++;

            if (_index >= codes.Length) gameObject.SetActive(false);
        }

        private void Restart()
        {
            _index = 0;
            codes = code.Split();
        }

        #endregion Game Mechanics / Methods
    }
}
