using _Scripts._Interfaces;
using _Scripts.Input;
using UnityEngine;

namespace _Scripts
{
    public class UpButton : MonoBehaviour, IButton
    {
        private void OnEnable()
        {
            InputHandler.UpKeyAction += ButtonPressed;
        }

        public void ButtonPressed()
        {
            gameObject.SetActive(false);
        }
        
        private void OnDisable()
        {
            InputHandler.UpKeyAction -= ButtonPressed;
        }
    }
}