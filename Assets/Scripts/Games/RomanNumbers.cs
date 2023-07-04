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

            [SerializeField] private TMP_Text task;
            [SerializeField] private GameObject gamestateWin, gamestateLose;
            [SerializeField] private int timeout;

        #endregion Serialized Fields

        #region Fields

            private int _decimalNumber, _romanNumber;
            private bool _isYes, _isNo;
            private float _timeElapsed;

        #endregion Fields

        #region Built-Ins / MonoBehaviours

        private void Awake()
        {
            SetUp();
        }
        
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
                yield return new WaitUntil(() => _isYes || _isNo || Time.time - timer > timeout);
                _timeElapsed = Time.time - timer;
            }

            private IEnumerator DetermineGamestate()
            {
                if (_timeElapsed < timeout && (_decimalNumber < _romanNumber && _isYes && !_isNo) || (_decimalNumber >= _romanNumber && !_isYes && _isNo))
                {
                    gamestateWin.SetActive(true);
                    yield return new WaitForSeconds(1);
                    GameWon();
                }
                else
                {
                    gamestateLose.SetActive(true);
                    yield return new WaitForSeconds(1);
                    GameLost();
                }
                yield return new WaitForSeconds(1);
                SceneReset();
                yield return new WaitForSeconds(1);
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
                
                while (value >= 1000) { value -= 1000; res += "M"; }
                if (value >= 900) { value -= 900; res += "CM"; }

                while (value >= 500) { value -= 500; res += "D"; }
                if (value >= 400) { value -= 400; res += "CD"; }

                while (value >= 100) { value -= 100; res += "C"; }
                if (value >= 90) { value -= 90; res += "XC"; }

                while (value >= 50) { value -= 50; res += "L"; }
                if (value >= 40) { value -= 40; res += "XL"; }

                while (value >= 10) { value -= 10; res += "X"; }
                if (value >= 9) { value -= 9; res += "IX"; }

                while (value >= 5) { value -= 5; res += "V"; }
                if (value >= 4) { value -= 4; res += "IV"; }

                while (value >= 1) { value -= 1; res += "I"; }
                
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
                base.Win();
            }
        
            private void GameLost()
            {
                base.Lose();
            }
            
            private void OnEnable()
            {
                keys.One.Input.action.performed += YesButtonPressed;
                keys.Two.Input.action.performed += NoButtonPressed;
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
                keys.One.Input.action.performed -= YesButtonPressed;
                keys.Two.Input.action.performed -= NoButtonPressed;
            }

        #endregion Overarching Methods / Helpers
    }
}