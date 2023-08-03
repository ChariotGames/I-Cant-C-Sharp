using Scripts.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Scripts.Controllers;
using UnityEngine;
using Random = UnityEngine.Random;
using TMPro;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

namespace Scripts.Games
{
    public class SameAsBefore : BaseGame
    {
        [Space]
        [Header("Game Specific Stuff")]
        [SerializeField] private List<GameObject> options;
        [SerializeField] private GameObject startText, gamestateWin, gamestateLose;
        [SerializeField] private TMP_Text stepBackText,  buttonYes, buttonNo;
        [SerializeField] private int maxStepsBack;
        [SerializeField] private int timeout;
        [SerializeField] private AudioClip[] clips;

        private LinkedList<int> _lastIndices = new();
        private AudioSource audio;
        private const string _stepsText = "think back: ";
        private int _index, _steps;
        private bool _isYes, _isNo;

        void Start()
        {
            audio = GetComponent<AudioSource>();
            audio.clip = clips[0];
            StartCoroutine(GameStartCoroutine());
        }

        private void SpawnSymbol()
        {
            _index = Random.Range(0, options.Count);
            audio.clip = clips[_index];
            audio.Play();
            if(_index != 0) StartCoroutine(PlaySoundAfterTime(0.15f));
            options[_index].SetActive(true);
        }

        IEnumerator PlaySoundAfterTime(float time)
        {
            yield return new WaitForSeconds(time);
            audio.Play();
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
                yield return new WaitForSeconds(Time.deltaTime);
            }
            startText.SetActive(false);
        }

        private IEnumerator GameStartCoroutine()
        {
            yield return StartCoroutine(AnimateInstruction());
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
                    //Debug.Log(_index + " : " + _lastIndices.ElementAt(_steps) + "\tthink back: " + (_steps + 1));

                    float timer = Time.time;
                    RunTimer(timeout);
                    yield return new WaitUntil(() => _isYes || _isNo || Time.time - timer > timeout);
                    if ((_index == _lastIndices.ElementAt(_steps) && _isYes && !_isNo) || (_index != _lastIndices.ElementAt(_steps) && _isNo && !_isYes))
                    {
                        gamestateWin.SetActive(true);
                        Success();
                    }
                    else
                    {
                        gamestateLose.SetActive(true);
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
    }
}