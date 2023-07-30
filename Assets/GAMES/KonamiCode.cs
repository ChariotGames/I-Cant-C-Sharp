using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace Scripts.Games
{
    public class KonamiCode : MonoBehaviour
    {
        #region Serialized Fields

        [SerializeField] private InputActionReference[] allowedButtons;
        [SerializeField] private string code = "UUDDLRLRWN";
        [SerializeField] private Animator camImator, konamiMator;
        [SerializeField] private Button backButton;

        #endregion Serialized Fields

        #region Fields

        private char[] codes;
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

        private void Restart()
        {
            _index = 0;
            codes = code.ToCharArray();
        }

        private void ButtonPressed(InputAction.CallbackContext ctx)
        {
            if (!ctx.action.name.Contains(codes[_index]))
            {
                Restart();
                return;
            }
            
            _index++;

            if (_index >= codes.Length)
            {
                camImator.SetTrigger("KonamIN");
                camImator.ResetTrigger("KonamOUT");

                konamiMator.SetTrigger("KonamIN");
                _index = 0;
                backButton.enabled = true;
            }
        }

        #endregion Game Mechanics / Methods
    }
}
