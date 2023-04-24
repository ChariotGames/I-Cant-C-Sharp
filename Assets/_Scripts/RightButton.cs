using _Scripts.Input;
using UnityEngine;

namespace _Scripts
{
    public class RightButton : MonoBehaviour, IButton
    {
        private void OnEnable()
        {
            InputHandler.RightKeyAction += ButtonPressed;
        }

        public void ButtonPressed()
        {
            gameObject.SetActive(false);
        }
        
        private void OnDisable()
        {
            InputHandler.RightKeyAction -= ButtonPressed;
        }
    }
}