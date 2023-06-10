using _Scripts._Input;
using _Scripts._Interfaces;
using UnityEngine;

namespace _Scripts
{
    public class DownButton : MonoBehaviour, IButton
    {
        private void OnEnable()
        {
            InputHandler.ArrowDown += ButtonPressed;
        }

        public void ButtonPressed()
        {
            gameObject.SetActive(false);
        }
        
        private void OnDisable()
        {
            InputHandler.ArrowDown -= ButtonPressed;
        }
    }
}