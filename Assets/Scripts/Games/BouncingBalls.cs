<<<<<<< HEAD:Assets/Scripts/Games/BouncingBalls.cs
using Scripts._Input;
using Scripts.GameElements;
using Scripts.Models;
=======
<<<<<<< Updated upstream:Assets/_Scripts/Games/BouncingBalls.cs
using _Scripts._Input;
using _Scripts.GameElements;
using _Scripts.Models;
=======
using System;
using Scripts._Input;
using Scripts.GameElements;
using Scripts.Models;
>>>>>>> Stashed changes:Assets/Scripts/Games/BouncingBalls.cs
>>>>>>> parent of 9fee4ce (Revert "Update"):Assets/_Scripts/Games/BouncingBalls.cs
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using Input = UnityEngine.Windows.Input;
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
        
        private int _bounceCounter;
        private bool _guessingStage;
        private int _currentGuessNumber;
        private int _remainingLives = 3;
        private float _elapsedTime;
        private float _timeoutStemp;
<<<<<<< Updated upstream:Assets/_Scripts/Games/BouncingBalls.cs
        private float _ballGravityScale = 1f;
<<<<<<< HEAD:Assets/Scripts/Games/BouncingBalls.cs
        private int _currentScore;
=======
=======
        private int _currentScore;
>>>>>>> Stashed changes:Assets/Scripts/Games/BouncingBalls.cs
>>>>>>> parent of 9fee4ce (Revert "Update"):Assets/_Scripts/Games/BouncingBalls.cs
        private bool hasRandomGravity;
        
        private const float _timeoutDelay = 10f;
        private const float _maxRoundTime = 10;
        private const int _scoreToWin = 3;
        #endregion Fields

        #region Built-Ins / MonoBehaviours

        private void Awake()
        {
            base.SetUp();
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
            if (_elapsedTime >= 3 && infoText.gameObject.activeSelf)
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
                _guessingStage = false;
            }
        }

        
        private void OnEnable()
        {
            BounceGround.HitGround += IncreaseBounceCounter;
            keys.One.Input.action.performed += IncreaseGuessingNumber;
            keys.Two.Input.action.performed += DecreaseGuessingNumber;
            keys.Three.Input.action.performed += SubmitGuess;
        }

        private void OnDisable()
        {
            BounceGround.HitGround -= IncreaseBounceCounter;
            keys.One.Input.action.performed -= IncreaseGuessingNumber;
            keys.Two.Input.action.performed -= DecreaseGuessingNumber;
            keys.Three.Input.action.performed -= SubmitGuess;
        }

        #endregion Built-Ins / MonoBehaviours

        #region GetSets / Properties
        
        

        #endregion GetSets / Properties

        #region Game Mechanics / Methods
<<<<<<< HEAD:Assets/Scripts/Games/BouncingBalls.cs
        
        public void IncreaseScore()
        {
            _currentScore++;
            if (_currentScore >= 3)
            {
                _currentScore = 0;
                base.Win();
            }
        }
=======
<<<<<<< Updated upstream:Assets/_Scripts/Games/BouncingBalls.cs

>>>>>>> parent of 9fee4ce (Revert "Update"):Assets/_Scripts/Games/BouncingBalls.cs
        private void SubmitGuess()
=======
        
        private void IncreaseScore()
        {
            _currentScore++;
            if (_currentScore >= _scoreToWin)
            {
                _currentScore = 0;
                base.Win();
            }
        }
        private void SubmitGuess(InputAction.CallbackContext ctx)
>>>>>>> Stashed changes:Assets/Scripts/Games/BouncingBalls.cs
        {
            if (!_guessingStage) return;
            if (_bounceCounter == _currentGuessNumber)
            {
                IncreaseScore();
                Debug.Log("Correct Answer");
                correctAnswer.gameObject.SetActive(true);
                
            }
            else
            {
                Debug.Log("You´re " + (_currentGuessNumber - _bounceCounter) + " Bounces off");
                _remainingLives--;
                if (_remainingLives == 0)
                {
<<<<<<< HEAD:Assets/Scripts/Games/BouncingBalls.cs
                    _maxFails = 3;
=======
<<<<<<< Updated upstream:Assets/_Scripts/Games/BouncingBalls.cs
=======
                    _remainingLives = 3;
>>>>>>> Stashed changes:Assets/Scripts/Games/BouncingBalls.cs
>>>>>>> parent of 9fee4ce (Revert "Update"):Assets/_Scripts/Games/BouncingBalls.cs
                    Lose();
                    Debug.Log("You lost all your lives in this Game");
                }
                wrongAnswer.gameObject.SetActive(true);
            }

            StartCoroutine(StartNewRound());
        }
        
        private IEnumerator WaitForSecondsCoroutine(float delay)
        {
            yield return new WaitForSeconds(delay);
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
            foreach (var ball in bouncingBalls)
            {
                if (hasRandomGravity)
                {
                    var randomGravity = Random.Range(1.5f, 2.6f);
                    ball.gravityScale = randomGravity;
                }
                var randomDelay = Random.Range(0f, 1.5f);
                yield return new WaitForSeconds(randomDelay);
                ball.isKinematic = false;
                ball.gameObject.SetActive(true);
            }
        }

        private IEnumerator StartNewRound()
        {
            if (_guessingStage)
            {
                _bounceCounter = 0;
                _currentGuessNumber = 0;
                _elapsedTime = 0;
                _guessingStage = false;
                guessingOverlay.gameObject.SetActive(false);
                yield return new WaitForSeconds(1); // adjust time for how long the correct/wrong Icon will be shown before spawning new Round
                correctAnswer.gameObject.SetActive(false);
                wrongAnswer.gameObject.SetActive(false);
                infoText.gameObject.SetActive(true);
                

                foreach (var ball in bouncingBalls)
                {
                    var transformPosition = ball.transform.position;
                    transformPosition.y = 2f;
                    ball.transform.position = transformPosition;
                    ball.isKinematic = true;
                }
                balls.gameObject.SetActive(true);
                
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