using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace Scripts.Games
{
    public class KonamiCode : MonoBehaviour
    {
        #region Serialized Fields

        [SerializeField] private InputActionAsset playerMap;
        [SerializeField] private InputActionReference[] code;
        [SerializeField] private Animator camImator, konamiMator, endAnimator;
        [SerializeField] private Button backButton;

        #endregion Serialized Fields

        #region Fields

        private int _index;

        #endregion Fields

        #region Built-Ins / MonoBehaviours

        void OnEnable()
        {
            foreach (InputAction action in playerMap.actionMaps[0].actions)
                action.performed += ButtonPressed;
        }

        void OnDisable()
        {
            foreach (InputAction action in playerMap.actionMaps[0].actions)
                action.performed -= ButtonPressed;
        }

        #endregion Built-Ins / MonoBehaviours

        #region Game Mechanics / Methods

        private void ButtonPressed(InputAction.CallbackContext ctx)
        {
            if (!(code[_index].action == ctx.action))
            {
                _index = 0;
                return;
            }
            
            _index++;

            if (_index >= code.Length)
            {
                camImator.SetTrigger("KonamIN");
                endAnimator.SetTrigger("EndOut");
                konamiMator.SetTrigger("KonamIN");
                _index = 0;
                backButton.enabled = true;
                this.enabled = false;
            }
        }

        #endregion Game Mechanics / Methods
    }
}
