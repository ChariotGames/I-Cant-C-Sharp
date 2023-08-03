using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
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
        private bool done = false;

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

        private IEnumerator ThankYou()
        {
            yield return new WaitForSeconds(0.5f);
            konamiMator.SetTrigger("ThankYOU");
        }

        public void BackToEnd()
        {
            if (!done)  StartCoroutine(ThankYou());
            else StartCoroutine(AnimateKonamiOut());
            done = !done;
        }

        private IEnumerator AnimateKonamiOut()
        {
            yield return new WaitForSeconds(0.5f);

            endAnimator.SetTrigger("EndIn");
            camImator.SetTrigger("KonamOUT");
            enabled = true;
            yield return new WaitForSeconds(0.25f);
            konamiMator.SetTrigger("KonamOUT");
        }

        #endregion Game Mechanics / Methods
    }
}
