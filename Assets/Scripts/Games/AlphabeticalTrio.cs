using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using _Scripts._Input;
using _Scripts.Models;
using TMPro;
using Debug = UnityEngine.Debug;
using Random = UnityEngine.Random;

namespace _Scripts.Games
{
    public class AlphabeticalTrio : BaseGame
    {
        #region Serialized Fields

            [SerializeField] private TMP_Text letters;
            [SerializeField] private GameObject letterContainer, gamestateWin, gamestateLose;
            [SerializeField] private int timeout;

        #endregion Serialized Fields

        #region Fields

            private bool _isYes;
            private bool _isNo;
            private float _timeElapsed;

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
                yield return new WaitForSeconds(1);
                while (true)
                {
                    bool isTrio = Random.Range(0f, 1f) > 0.48f;
                    ShowLetters(isTrio);

                    yield return StartCoroutine(MeasureTime());
                    yield return StartCoroutine(DetermineGamestate(isTrio));
                }
            }

        #endregion Game Mechanics / Methods

        #region Overarching Methods / Helpers

            private IEnumerator MeasureTime()
            {
                _timeElapsed = -1;
                Stopwatch stopwatch = new();
                stopwatch.Start();
                yield return new WaitUntil(() => _isYes || _isNo || stopwatch.ElapsedMilliseconds > timeout * 1000);
                stopwatch.Stop();
                _timeElapsed = stopwatch.ElapsedMilliseconds;
            }
            
            private IEnumerator DetermineGamestate(bool isTrio)
            {
                if (_timeElapsed < timeout * 1000 && _timeElapsed >= 0 && _isYes == isTrio && _isNo != isTrio)
                {
                    GameWon();
                }
                else
                {
                    GameLost();
                }
                yield return new WaitForSeconds(1);
                SceneReset();
                yield return new WaitForSeconds(1);
            }
            
            private void GameWon()
            {
                gamestateWin.SetActive(true);
                //Win();
            }
        
            private void GameLost()
            {
                gamestateLose.SetActive(true);
                //Lose();
            }
        
            private void ShowLetters(bool isTrio)
            {
                Debug.Log(isTrio);

                int letter;
                char[] newLetters;
                    
                switch (isTrio)
                {
                    case true:
                        letter = Random.Range(66, 90);
                        newLetters = new []{(char)(letter - 1), (char)letter, (char)(letter + 1)};
                        break;
                    case false:
                        letter = Random.Range(66, 90);
                        int random = letter == 66 ? 0 : letter == 89 ? 1 : Random.Range(0, 2);
                        int shift1 = random == 0 ? 1 : Random.Range(2, letter - 65);
                        int shift2 = random == 1 ? 1 : Random.Range(2, 90 - letter + 1);
                        //Debug.Log(letter + " : " + shift1 + " , " + shift2);
                        newLetters = new[]
                        {
                            (char)(letter - shift1), 
                            (char)letter, 
                            (char)(letter + shift2)
                        };
                        break;
                }
                SetLettersByDifficulty(newLetters);
                letterContainer.SetActive(true);
            }

            private void SetLettersByDifficulty(char[] newLetters)
            {
                switch (Difficulty)
                {
                    case Difficulty.EASY:
                        SetLettersInAlphabeticalOrder(newLetters);
                        break;
                    case Difficulty.MEDIUM:
                        SetLettersInReverseAlphabeticalOrder(newLetters);
                        break;
                    case Difficulty.HARD:
                        if (Random.Range(0, 2) == 1)
                        {
                            SetLettersInAlphabeticalOrder(newLetters);
                        }
                        else
                        {
                            SetLettersInReverseAlphabeticalOrder(newLetters);
                        }
                        break;
                }
            }

            private void SetLettersInAlphabeticalOrder(char[] newLetters)
            {
                letters.text = newLetters[0] + " " + newLetters[1] + " " + newLetters[2];
            }
            
            private void SetLettersInReverseAlphabeticalOrder(char[] newLetters)
            {
                letters.text = newLetters[2] + " " + newLetters[1] + " " + newLetters[0];
            }

            private void SceneReset()
            {
                //Debug.Log("Reset!");
                _isYes = false;
                _isNo = false;
                letterContainer.SetActive(false);
                gamestateWin.SetActive(false);
                gamestateLose.SetActive(false);
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