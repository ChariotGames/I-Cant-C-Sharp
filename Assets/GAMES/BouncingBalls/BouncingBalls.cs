using Scripts.GameElements;
using Scripts.Models;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;

namespace Scripts.Games
{
    public class BouncingBalls : BaseGame
    {
        #region Fields

        [SerializeField] private List<Rigidbody2D> bouncingBalls;
        [SerializeField] private TextMeshPro guessedNumber;
        [SerializeField] private GameObject balls;
        [SerializeField] private GameObject guessingOverlay;
        [SerializeField] private PhysicsMaterial2D groundMaterial;
        [SerializeField] private TextMeshPro infoText;
        [SerializeField] private SpriteRenderer correctAnswer;
        [SerializeField] private SpriteRenderer wrongAnswer;
        [SerializeField] private TextMeshPro resultText;

        private int _bounceCounter;
        private bool _guessingStage;
        private int _currentGuessNumber;
        private int _remainingLives = 3;
        private float _elapsedTime;
        private float _timeoutStemp;

        private int _currentScore;
        private bool hasRandomGravity;

        private const float _timeoutDelay = 10f;
        private const float _maxRoundTime = 10;
        private const int _scoreToWin = 3;

        #endregion Fields

        #region Built-Ins / MonoBehaviours

        private void Awake()
        {
            switch (Difficulty)
            {
                case Difficulty.EASY:
                    groundMaterial.bounciness = 1.05f;
                    break;
                case Difficulty.MEDIUM:
                    groundMaterial.bounciness = 1f;
                    break;
                case Difficulty.HARD:
                    groundMaterial.bounciness = 0.925f;
                    hasRandomGravity = true;
                    break;
            }
        }

        private void Start()
        {
            //groundMaterial = new PhysicsMaterial2D();
            StartCoroutine(ReleaseBallsAfterDelay());
            infoText.gameObject.SetActive(true);
        }

        private void Update()
        {
            _elapsedTime += Time.deltaTime;
            if (_elapsedTime >= 2 && infoText.gameObject.activeSelf)
            {
                infoText.gameObject.SetActive(false);
            }

            if (_elapsedTime >= _maxRoundTime && !_guessingStage)
            {
                ActivateGuessingOverlay();
            }

            if (_elapsedTime >= _timeoutStemp + _timeoutDelay && _guessingStage)
            {
                SubmitGuess(new InputAction.CallbackContext());
            }
        }


        private void OnEnable()
        {
            BounceGround.HitGround += IncreaseBounceCounter;
            _keys.One.Input.action.performed += IncreaseGuessingNumber;
            _keys.Two.Input.action.performed += DecreaseGuessingNumber;
            _keys.Three.Input.action.performed += SubmitGuess;
        }

        private void OnDisable()
        {
            BounceGround.HitGround -= IncreaseBounceCounter;
            _keys.One.Input.action.performed -= IncreaseGuessingNumber;
            _keys.Two.Input.action.performed -= DecreaseGuessingNumber;
            _keys.Three.Input.action.performed -= SubmitGuess;
        }

        #endregion Built-Ins / MonoBehaviours

        #region GetSets / Properties

        #endregion GetSets / Properties

        #region Game Mechanics / Methods

        private void IncreaseScore()
        {
            _currentScore++;
            if (_currentScore >= _scoreToWin)
            {
                _currentScore = 0;
                base.Harder();
                base.Win();
            }

            base.ScoreUp();
        }

        private void SubmitGuess(InputAction.CallbackContext ctx)
        {
            if (!_guessingStage) return;

            resultText.text = "You're " + Mathf.Abs(_currentGuessNumber - _bounceCounter) + " bounces off";

            if (_bounceCounter == _currentGuessNumber)
            {
                IncreaseScore();
                Debug.Log("Correct Answer");
                correctAnswer.gameObject.SetActive(true);
            }
            else
            {
                _remainingLives--;

                if (_remainingLives == 0)
                {
                    _remainingLives = 3;
                    base.Easier();
                    guessingOverlay.gameObject.SetActive(false);
                    Debug.Log("You lost all your lives in this game");
                    Lose();
                }
                
                guessingOverlay.gameObject.SetActive(false);
                resultText.gameObject.SetActive(true);
                wrongAnswer.gameObject.SetActive(true);
            }

            StartCoroutine(StartNewRound());
        }


        private void IncreaseGuessingNumber(InputAction.CallbackContext ctx)
        {
            if (!_guessingStage) return;

            _currentGuessNumber++;
            guessedNumber.text = _currentGuessNumber.ToString();
        }

        private void DecreaseGuessingNumber(InputAction.CallbackContext ctx)
        {
            if (!_guessingStage || _currentGuessNumber == 0) return;

            _currentGuessNumber--;
            guessedNumber.text = _currentGuessNumber.ToString();
        }

        private void ActivateGuessingOverlay()
        {
            _guessingStage = true;
            Debug.Log(_bounceCounter);
            _timeoutStemp = _elapsedTime;
            var randomOffset = Random.Range(0, 5);
            _currentGuessNumber = _bounceCounter + (Random.value < 0.5f ? randomOffset : -randomOffset);
            guessedNumber.text = _currentGuessNumber.ToString();
            balls.SetActive(false);
            guessingOverlay.SetActive(true);
        }

        private IEnumerator ReleaseBallsAfterDelay()
        {
            resultText.gameObject.SetActive(false);
            foreach (var ball in bouncingBalls)
            {
                if (hasRandomGravity)
                {
                    var randomGravity = Random.Range(1.5f, 2.6f);
                    ball.gravityScale = randomGravity;
                }

                var randomDelay = Random.Range(0f, 1.5f);
                Debug.Log(randomDelay);
                yield return new WaitForSeconds(randomDelay);
                ball.isKinematic = false;
                ball.gameObject.SetActive(true);
            }
        }

        private IEnumerator StartNewRound()
        {
            if (_guessingStage)
            {
                _guessingStage = false;
                _bounceCounter = 0;
                _currentGuessNumber = 0;
                _elapsedTime = 0f;
                guessingOverlay.gameObject.SetActive(false);
                yield return new WaitForSeconds(2f); // Adjust the duration as needed

                resultText.gameObject.SetActive(false);
                correctAnswer.gameObject.SetActive(false);
                wrongAnswer.gameObject.SetActive(false);

                foreach (var ball in bouncingBalls)
                {
                    var transformPosition = ball.transform.position;
                    transformPosition.y = 2f;
                    ball.transform.position = transformPosition;
                    ball.isKinematic = true;
                }
                _elapsedTime = 0f;
                balls.gameObject.SetActive(true);
                infoText.gameObject.SetActive(true);
                

                StartCoroutine(ReleaseBallsAfterDelay());
            }
        }

        private void IncreaseBounceCounter()
        {
            _bounceCounter++;
        }

        #endregion Game Mechanics / Methods

        #region Overarching Methods / Helpers

        #endregion Overarching Methods / Helpers
    }
}