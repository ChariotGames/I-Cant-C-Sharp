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

        private void Restart()
        {
            _index = 0;
            codes = code.Split();
        }

        private void ButtonPressed(InputAction.CallbackContext ctx)
        {
            
            if (!ctx.action.name.Contains(codes[_index])) Restart();
            
            _index++;

            if (_index >= codes.Length)
            {
                camImator.SetTrigger("KonamIN");
                camImator.ResetTrigger("KonamOUT");

                konamiMator.SetTrigger("Alex");
                konamiMator.SetTrigger("Enno");
                konamiMator.SetTrigger("Jannis");
                konamiMator.SetTrigger("Jerome");
                konamiMator.SetTrigger("Milana");
                konamiMator.SetTrigger("Nermin");
                konamiMator.SetTrigger("Pascal");

                backButton.enabled = true;
            }
        }

        #endregion Game Mechanics / Methods
    }
}
