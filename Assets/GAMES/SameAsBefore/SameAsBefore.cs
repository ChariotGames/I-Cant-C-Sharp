using Scripts.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;
using TMPro;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

namespace Scripts.Games
{
    public class SameAsBefore : BaseGame
    {
        [SerializeField] private List<GameObject> options;
        [SerializeField] private GameObject startText, gamestateWin, gamestateLose;
        [SerializeField] private TMP_Text stepBackText,  buttonYes, buttonNo;
        [SerializeField] private int maxStepsBack;
        [SerializeField] private int timeout;
        [SerializeField] private int successesToLevelUp;

        private LinkedList<int> _lastIndices = new();
        private const string _stepsText = "steps: ";
        private int _index, _steps;
        private bool _isYes, _isNo;
        private int difficultyTracker, defaultFailsToLose;

        void Start()
        {
            difficultyTracker = successesToLevelUp;
            _fails = failsToLose;
            defaultFailsToLose = failsToLose;
            buttonYes.text = _keys.One.Icon;
            buttonNo.text = _keys.Two.Icon;
            StartCoroutine(GameStartCoroutine());
        }

        private void SpawnSymbol()
        {
            _index = Random.Range(0, options.Count);
            options[_index].SetActive(true);
        }

        private void UpdateSteps()
        {
            switch (Difficulty)
            {
                case Difficulty.EASY:
                    _steps = 0;
                    break;
                case Difficulty.MEDIUM:
                    _steps = Math.Min(1, _lastIndices.Count - 1);
                    break;
                case Difficulty.HARD:
                    _steps = Random.Range(0, Math.Min(maxStepsBack, _lastIndices.Count));
                    break;
                default:
                    _steps = 0;
                    break;
            }
        }

        private void UpdateStepBackText()
        {
            stepBackText.text = _stepsText + (_steps + 1);
        }

        private IEnumerator AnimationStartText(float distance)
        {
            float offset = 0;
            while (offset < distance)
            {
                float delta = Time.deltaTime * 2;
                startText.transform.Translate(0, delta, 0, Space.Self);
                offset += delta;
                yield return new WaitForSeconds(0.001f);
            }
            startText.SetActive(false);
        }

        private IEnumerator GameStartCoroutine()
        {
            yield return StartCoroutine(AnimationStartText(2.1f));
            SpawnSymbol();
            _lastIndices.AddFirst(_index);
            if (_lastIndices.Count > maxStepsBack) { _lastIndices.RemoveLast(); }
            yield return new WaitForSeconds(1);
            options[_index].SetActive(false);
            StartCoroutine(SpawnCoroutine());
        }

        private IEnumerator SpawnCoroutine()
        {
            yield return new WaitForSeconds(1);
            while (true)
            {
                if (options[0].activeSelf || options[1].activeSelf)
                {
                    //Debug.Log(lastIndices.Count);
                    Debug.Log(_index + " : " + _lastIndices.ElementAt(_steps) + "\tsteps: " + (_steps + 1));

                    float timer = Time.time;
                    RunTimer(timeout);
                    yield return new WaitUntil(() => _isYes || _isNo || Time.time - timer > timeout);
                    StopTimer();
                    if ((_index == _lastIndices.ElementAt(_steps) && _isYes && !_isNo) || (_index != _lastIndices.ElementAt(_steps) && _isNo && !_isYes))
                    {
                        gamestateWin.SetActive(true);
                        //GameWon();
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
                    options[_index].SetActive(false);
                    _isYes = false;
                    _isNo = false;
                    _lastIndices.AddFirst(_index);
                    if (_lastIndices.Count > maxStepsBack) { _lastIndices.RemoveLast(); }
                    gamestateWin.SetActive(false);
                    gamestateLose.SetActive(false);
                    yield return new WaitForSeconds(0.5f);
                }
                else
                {
                    SpawnSymbol();
                    UpdateSteps();
                    UpdateStepBackText();
                }
            }
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
            _keys.One.Input.action.performed += YesButtonPressed;
            _keys.Two.Input.action.performed += NoButtonPressed;
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
            _keys.One.Input.action.performed -= YesButtonPressed;
            _keys.Two.Input.action.performed -= NoButtonPressed;
        }
    }
}