using _Scripts._Input;
using _Scripts._Interfaces;
using UnityEngine;

namespace _Scripts
{
    public class UpButton : MonoBehaviour, IButton
    {
        private void OnEnable()
        {
            InputHandler.ArrowUp += ButtonPressed;
        }

        public void ButtonPressed()
        {
            gameObject.SetActive(false);
        }
        
        private void OnDisable()
        {
            InputHandler.ArrowUp -= ButtonPressed;
        }
    }
}