using _Scripts._Input;
using _Scripts._Interfaces;
using UnityEngine;

namespace _Scripts
{
    public class LeftButton : MonoBehaviour, IButton
    {
        private void OnEnable()
        {
            InputHandler.ArrowLeft += ButtonPressed;
        }

        public void ButtonPressed()
        {
            gameObject.SetActive(false);
        }
        
        private void OnDisable()
        {
            InputHandler.ArrowLeft -= ButtonPressed;
        }
    }
}