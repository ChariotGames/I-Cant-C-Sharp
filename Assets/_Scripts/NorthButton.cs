using _Scripts._Input;
using _Scripts._Interfaces;
using UnityEngine;

namespace _Scripts
{
    public class NorthButton : MonoBehaviour, IButton
    {
        private void OnEnable()
        {
            InputHandler.NorthBtnAction += ButtonPressed;
        }

        public void ButtonPressed()
        {
            gameObject.SetActive(false);
        }
        
        private void OnDisable()
        {
            InputHandler.NorthBtnAction -= ButtonPressed;
        }
    }
}