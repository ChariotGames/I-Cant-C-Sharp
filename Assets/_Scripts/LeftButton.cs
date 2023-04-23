using _Scripts.Input;
using UnityEngine;

namespace _Scripts
{
    public class LeftButton : MonoBehaviour, IButton
    {
        private void OnEnable()
        {
            InputHandler.LeftKeyAction += ButtonPressed;
        }

        public void ButtonPressed()
        {
            gameObject.SetActive(false);
        }
        
        private void OnDisable()
        {
            InputHandler.LeftKeyAction -= ButtonPressed;
        }
    }
}