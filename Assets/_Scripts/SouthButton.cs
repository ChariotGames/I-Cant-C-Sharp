using System;
using _Scripts._Input;
using _Scripts._Interfaces;
using _Scripts.Games;
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
            ButtonHero.ResetTimer();
        }
        
        private void OnDisable()
        {
            InputHandler.SouthBtnAction -= ButtonPressed;
        }
    }
}