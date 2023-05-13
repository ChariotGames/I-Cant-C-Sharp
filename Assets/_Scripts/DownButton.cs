using _Scripts._Input;
using _Scripts._Interfaces;
using UnityEngine;

namespace _Scripts
{
    public class DownButton : MonoBehaviour, IButton
    {
        private void OnEnable()
        {
            InputHandler.DownArrowBtnAction += ButtonPressed;
        }

        public void ButtonPressed()
        {
            gameObject.SetActive(false);
        }
        
        private void OnDisable()
        {
            InputHandler.DownArrowBtnAction -= ButtonPressed;
        }
    }
}