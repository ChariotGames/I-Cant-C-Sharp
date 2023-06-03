using _Scripts._Input;
using _Scripts._Interfaces;
using UnityEngine;

namespace _Scripts
{
    public class SouthButton : MonoBehaviour, IButton
    {
        private void OnEnable()
        {
            InputHandler.SouthBtnAction += ButtonPressed;
        }

        public void ButtonPressed()
        {
            gameObject.SetActive(false);
        }
        
        private void OnDisable()
        {
            InputHandler.SouthBtnAction -= ButtonPressed;
        }
    }
}