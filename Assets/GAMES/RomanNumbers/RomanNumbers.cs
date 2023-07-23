using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Scripts._Input;
using Scripts.Models;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;

namespace Scripts.Games
{
    public class RomanNumbers : BaseGame
    {
        #region Serialized Fields

            [SerializeField] private TMP_Text task, buttonYes, buttonNo;
            [SerializeField] private GameObject gamestateWin, gamestateLose;
            [SerializeField] private int timeout, successesToLevelUp;

        #endregion Serialized Fields

        #region Fields

            private int _decimalNumber, _romanNumber;
            private bool _isYes, _isNo;
            private float _timeElapsed;
            private int difficultyTracker, defaultFailsToLose;

        #endregion Fields

        #region Built-Ins / MonoBehaviours

        void Start()
        {
            difficultyTracker = successesToLevelUp;
            _fails = failsToLose;
            defaultFailsToLose = failsToLose;
            buttonYes.text = _keys.One.Icon;
            buttonNo.text = _keys.Two.Icon;
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
                    GenerateRandomNumber();
                    RenderTask();

                    yield return StartCoroutine(MeasureTime());
                    yield return StartCoroutine(DetermineGamestate());
                }
            }

        #endregion Game Mechanics / Methods

        #region Overarching Methods / Helpers

            private IEnumerator MeasureTime()
            {
                _timeElapsed = -1;
                float timer = Time.time;
                RunTimer(timeout);
                yield return new WaitUntil(() => _isYes || _isNo || Time.time - timer > timeout);
                StopTimer();
                _timeElapsed = Time.time - timer;
            }

            private IEnumerator DetermineGamestate()
            {
                if (_timeElapsed < timeout && (_decimalNumber < _romanNumber && _isYes && !_isNo) || (_decimalNumber >= _romanNumber && !_isYes && _isNo))
                {
                    gamestateWin.SetActive(true);
                    //GameWon();
                    difficultyTracker--;
                    if (difficultyTracker <= 0)
                    {
                        difficultyTracker = successesToLevelUp;
                        Harder();
                    }
                    Success();
                }
                else
                {
                    gamestateLose.SetActive(true);
                    //GameLost();
                    difficultyTracker++;
                    Fail();
                }
                yield return new WaitForSeconds(1);
                SceneReset();
                yield return new WaitForSeconds(0.5f);
            }

            private void GenerateRandomNumber()
            {
                int rangeTop = 0;
                switch (Difficulty)
                {
                    case Difficulty.EASY:
                        rangeTop = 25;
                        break;
                    case Difficulty.MEDIUM:
                        rangeTop = 100;
                        break;
                    case Difficulty.HARD:
                        rangeTop = 250;
                        break;
                }
                _decimalNumber = Random.Range(1, rangeTop + 1);
                _romanNumber = Random.Range(1, rangeTop + 1);
            }
            
            private void RenderTask()
            {
                task.text = _decimalNumber + " < " + DecimalToRoman(_romanNumber);
            }

            private string DecimalToRoman(int value)
            {
                if ((value < 1) || (value >= 4999)) { return ""; }
                string res = "";
                
                /*
                    res += new StringBuilder().Insert(0, "M", value / 1000).ToString();
                    value %= 1000;
                    ...
                 */
                
                while (value >= 1000) { value -= 1000; res += "m"; }
                if (value >= 900) { value -= 900; res += "cm"; }

                while (value >= 500) { value -= 500; res += "d"; }
                if (value >= 400) { value -= 400; res += "cd"; }

                while (value >= 100) { value -= 100; res += "c"; }
                if (value >= 90) { value -= 90; res += "xc"; }

                while (value >= 50) { value -= 50; res += "l"; }
                if (value >= 40) { value -= 40; res += "xl"; }

                while (value >= 10) { value -= 10; res += "x"; }
                if (value >= 9) { value -= 9; res += "ix"; }

                while (value >= 5) { value -= 5; res += "v"; }
                if (value >= 4) { value -= 4; res += "iv"; }

                while (value >= 1) { value -= 1; res += "i"; }
                
                return res;
            }
            
            private void SceneReset()
            {
                //UnityEngine.Debug.Log("Reset!");
                _isYes = false;
                _isNo = false;
                gamestateWin.SetActive(false);
                gamestateLose.SetActive(false);
            }

            private void GameWon()
            {
                ScoreUp();
                _successes++;
                difficultyTracker--;
                if (difficultyTracker <= 0)
                {
                    difficultyTracker = successesToLevelUp;
                    Harder();
                }
                if (_successes >= successesToWin)
                {
                    Win(); 
                }
            }
        
            private void GameLost()
            {
                _fails--;
                difficultyTracker++;
                if (_fails <= 0)
                {
                    _fails = failsToLose;
                    Easier();
                    Lose();
                }
            }
            
            private void OnEnable()
            {
                _keys.Two.Input.action.performed += YesButtonPressed;
                _keys.One.Input.action.performed += NoButtonPressed;
            }

            public void YesButtonPressed(InputAction.CallbackContext ctx)
            {
                _isYes = true;
            }

            public void NoButtonPressed(InputAction.CallbackContext ctx)
            {
                _isNo = true;
            }

            private void OnDisable()
            {
                _keys.Two.Input.action.performed -= YesButtonPressed;
                _keys.One.Input.action.performed -= NoButtonPressed;
            }

        #endregion Overarching Methods / Helpers
    }
}