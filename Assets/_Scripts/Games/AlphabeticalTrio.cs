using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using _Scripts._Input;
using TMPro;
using Random = UnityEngine.Random;

namespace _Scripts.Games
{
    public class AlphabeticalTrio : BaseGame
    {
        #region Serialized Fields

            [SerializeField] private TMP_Text letters;
            [SerializeField] private GameObject letterContainer;
            [SerializeField] private int timeout;

        #endregion Serialized Fields

        #region Fields

            private bool _isYes;
            private bool _isNo;

        #endregion Fields

        #region Built-Ins / MonoBehaviours

            void Start()
            {
                StartCoroutine(GameCoroutine());
            }

        #endregion Built-Ins / MonoBehaviours

        #region GetSets / Properties
        
        

        #endregion GetSets / Properties

        #region Game Mechanics / Methods
        
            private IEnumerator GameCoroutine()
            {
                while (true)
                {
                    bool isTrio = Random.Range(0, 1) > 0.5f;
                    showLetters(isTrio);
                }
            }

        #endregion Game Mechanics / Methods

        #region Overarching Methods / Helpers

            private void showLetters(bool isTrio)
            {
                char[] newLetters;
                    
                switch (isTrio)
                {
                    case true:
                        int letter = Random.Range(65, 91);
                        newLetters = new []{(char)letter, (char)(letter+1), (char)(letter+2)};
                        break;
                    case false:
                        newLetters = new[] { (char)Random.Range(65, 91), (char)Random.Range(65, 91), (char)Random.Range(65, 91) };
                        break;
                }
                letters.text = newLetters[0] + " " + newLetters[1] + " " + newLetters[2];
                letterContainer.SetActive(true);
            }
        
            private void OnEnable()
            {
                InputHandler.ShoulderRight += RightShoulderPressed;
                InputHandler.ShoulderLeft += LeftShoulderPressed;
            }

            public void RightShoulderPressed()
            {
                _isYes = true;
            }

            public void LeftShoulderPressed()
            {
                _isNo = true;
            }

            private void OnDisable()
            {
                InputHandler.ShoulderRight -= RightShoulderPressed;
                InputHandler.ShoulderLeft -= LeftShoulderPressed;
            }

        #endregion Overarching Methods / Helpers
    }
}