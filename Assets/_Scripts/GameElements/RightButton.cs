using _Scripts._Input;
using _Scripts._Interfaces;
using UnityEngine;

namespace _Scripts
{
    public class RightButton : MonoBehaviour, IButton
    {
        private void OnEnable()
        {
            InputHandler.ArrowRight += ButtonPressed;
        }

        public void ButtonPressed()
        {
            gameObject.SetActive(false);
        }
        
        private void OnDisable()
        {
            InputHandler.ArrowRight -= ButtonPressed;
        }
    }
}