using _Scripts._Input;
using _Scripts.GameElements;
using _Scripts.Models;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _Scripts.Games
{
    public class BouncingBalls : BaseGame
    {

        #region Fields

        [SerializeField] private List<Rigidbody2D> bouncingBalls;
        [SerializeField] private TextMeshPro guessedNumber;
        [SerializeField] private GameObject balls;
        [SerializeField] private GameObject guessingOverlay;
        [SerializeField] private PhysicsMaterial2D groundMaterial;
        
        private int _bounceCounter;
        private bool _guessingStage;
        private int _currentGuessNumber;
        private int _maxFails = 3;
        private float _elapsedTime;
        private float _maxRoundTime = 10;
        private float _timeoutStemp;
        private float _timeoutDelay = 15f;

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
                    break;

            }
        }

        private void Start()
        {
            //groundMaterial = new PhysicsMaterial2D();
           
            StartCoroutine(ReleaseBallsAfterDelay());
        }

        private void Update()
        {
            _elapsedTime += Time.deltaTime;
            if (_elapsedTime >= _maxRoundTime && !_guessingStage)
            {
                ActivateGuessingOverlay();
            }

            if (_elapsedTime >= _timeoutStemp + _timeoutDelay && _guessingStage)
            {
                SubmitGuess();
                _guessingStage = false;
                
            }
        }

        
        private void OnEnable()
        {
            BounceGround.HitGround += IncreaseBounceCounter;
            InputHandler.ButtonEast += IncreaseGuessingNumber;
            InputHandler.ButtonWest += DecreaseGuessingNumber;
            InputHandler.ButtonSouth += SubmitGuess;
        }

        private void OnDisable()
        {
            BounceGround.HitGround -= IncreaseBounceCounter;
            InputHandler.ButtonEast -= IncreaseGuessingNumber;
            InputHandler.ButtonWest -= DecreaseGuessingNumber;
            InputHandler.ButtonSouth -= SubmitGuess;
        }

        #endregion Built-Ins / MonoBehaviours

        #region GetSets / Properties
        
        

        #endregion GetSets / Properties

        #region Game Mechanics / Methods

        private void SubmitGuess()
        {
            if (!_guessingStage) return;
            if (_bounceCounter == _currentGuessNumber)
            {
                Debug.Log("Correct Answer");
            }
            else
            {
                Debug.Log("You´re " + (_currentGuessNumber - _bounceCounter) + " Bounces off");
                _maxFails--;
                if (_maxFails == 0)
                {
                    Lose();
                    Debug.Log("You lost all your lives in this Game");
                }
            }

            StartNewRound();
        }

        private void IncreaseGuessingNumber()
        {
            if (!_guessingStage) return;

            _currentGuessNumber++;
            guessedNumber.text = _currentGuessNumber.ToString();
        }

        private void DecreaseGuessingNumber()
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
                var randomDelay = Random.Range(0f, 1.5f);
                yield return new WaitForSeconds(randomDelay);
                ball.isKinematic = false;
                ball.gameObject.SetActive(true);
            }
        }

        private void StartNewRound()
        {
            if (_guessingStage)
            {
                _bounceCounter = 0;
                _currentGuessNumber = 0;
                _elapsedTime = 0;
                _guessingStage = false;
                guessingOverlay.gameObject.SetActive(false);

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