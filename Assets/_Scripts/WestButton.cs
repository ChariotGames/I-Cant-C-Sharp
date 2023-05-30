﻿using _Scripts._Input;
using _Scripts._Interfaces;
using UnityEngine;

namespace _Scripts
{
    public class WestButton : MonoBehaviour, IButton
    {
        private void OnEnable()
        {
            InputHandler.WestBtnAction += ButtonPressed;
        }

        public void ButtonPressed()
        {
            gameObject.SetActive(false);
        }
        
        private void OnDisable()
        {
            InputHandler.WestBtnAction -= ButtonPressed;
        }
    }
}