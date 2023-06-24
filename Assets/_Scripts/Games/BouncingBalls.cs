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
        
        private int _bounceCounter;
        private bool _guessingStage;
        private int _currentGuessNumber;
        private int _maxFails = 3;
        private int _pointsToWin = 3;
        private float _elapsedTime;
        private float _maxRoundTime;

        #endregion Fields

        #region Built-Ins / MonoBehaviours

        private void Start()
        {
            switch (Difficulty)
            {
                case Difficulty.EASY:
                    _maxRoundTime = 10f;
                    break;
                case Difficulty.MEDIUM:
                    _maxRoundTime = 15f;
                    break;
                case Difficulty.HARD:
                    _maxRoundTime = 20f;
                    break;

            }
            StartCoroutine(ReleaseBallsAfterDelay());
        }

        private void Update()
        {
            _elapsedTime += Time.deltaTime;
            if (_elapsedTime >= _maxRoundTime && !_guessingStage)
            {
                ActivateGuessingOverlay();
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
                _pointsToWin++;
                if (_pointsToWin == 3)
                {
                    Debug.Log("GAME WON");
                    Win();
                }
            }
            else
            {
                Debug.Log("You´re " + (_currentGuessNumber - _bounceCounter) + " Bounces off");
                _maxFails--;
                if (_maxFails == 0)
                {
                    Debug.Log("GAME LOST");
                    Lose();
                    
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