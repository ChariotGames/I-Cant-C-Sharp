using _Scripts._Interfaces;
using _Scripts.Input;
using UnityEngine;

namespace _Scripts
{
    public class DownButton : MonoBehaviour, IButton
    {
        private void OnEnable()
        {
            InputHandler.DownKeyAction += ButtonPressed;
        }

        public void ButtonPressed()
        {
            gameObject.SetActive(false);
        }
        
        private void OnDisable()
        {
            InputHandler.DownKeyAction -= ButtonPressed;
        }
    }
}