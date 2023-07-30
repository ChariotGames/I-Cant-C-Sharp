using System;
using System.Collections;
using System.Collections.Generic;
using Scripts.Controllers;
using Scripts.GameElements;
using TMPro;
using UnityEngine;
using Scripts.Models;
using Random = UnityEngine.Random;

namespace Scripts.Games
{
    public class ButtonHero : BaseGame
    {
        #region Serialized Fields

        [Space]
        [Header("Game Specific Stuff")]
        [SerializeField] private List<ButtonHeroElement> buttons;
        [SerializeField] private float spawnTimeUpperBounds;
        [SerializeField] private float spawnTimeLowerBounds;
        //[SerializeField] private TextMeshPro timerTextMesh;
        //[SerializeField] private SpriteRenderer damageTakenSprite;
        [SerializeField] private Transform container;
        //[SerializeField] private TextMeshPro infoText;

        #endregion Serialized Fields

        #region Fields

        private TextMeshPro _previousButton;
        private readonly List<TextMeshPro> _spawnedButtons = new();
        private float _timerIntern;
        

        //private int _remainingLives = 3;
        private float _elapsedTime;
        //private float _timeoutStemp;
        private int _currentScore;
        private float _buttonWidth;
        private const int _scoreToWin = 10;
        private bool _gameStarted;

        private float _maxRoundTime;

        #endregion Fields

        #region Built-Ins / MonoBehaviours

        private void Awake()
        {
            SetDifficulty();
            
            for (var i = buttons.Count - 1; i >= 0; i--)
            {
                // just pool all the objects into a list
                var button = Instantiate(buttons[i].gameObject, container);
                var buttonText = button.GetComponent<TextMeshPro>();
                buttonText.text = _keys.All[i].Icon;
                button.GetComponent<BasePressElement>().Button = _keys.All[i].Input;
                _spawnedButtons.Add(buttonText);
                button.SetActive(false);
            }
        }

        private void OnEnable()
        {
            MinigameManager.OnDifficultyChanged += UpdateDifficulty;
        }

        private void OnDisable()
        {
            MinigameManager.OnDifficultyChanged -= UpdateDifficulty;
        }

        private void SetDifficulty()
        {
            switch (Difficulty)
            {
                case Difficulty.EASY:
                    _maxRoundTime = 3;
                    break;
                case Difficulty.MEDIUM:
                    _maxRoundTime = 2;
                    break;
                case Difficulty.HARD:
                    _maxRoundTime = 1;
                    break;
            }
        }

        private void UpdateDifficulty(Difficulty difficulty)
        {
            base.Difficulty = difficulty;
            SetDifficulty();
        }

        private IEnumerator Start()
        {
            yield return StartCoroutine(base.AnimateInstruction());
            _gameStarted = true;
            //infoText.gameObject.SetActive(true);
            _buttonWidth = buttons[0].gameObject.GetComponent<RectTransform>().rect.width * 0.5f;
            
            StartCoroutine(SpawnCoroutine());
        }

        private void Update()
        {
            if (!_gameStarted) return;
            _elapsedTime += Time.deltaTime;
            /*if (_elapsedTime >=  3 && infoText.gameObject.activeSelf)
            {
                infoText.gameObject.SetActive(false);
            }
            */
            HandleTimer();
        }

        #endregion Built-Ins / MonoBehaviours

        #region GetSets / Properties

        #endregion GetSets / Properties

        #region Game Mechanics / Methods

        private void HandleTimer()
        {
            if (_previousButton && _previousButton.gameObject.activeSelf)
            {
                _timerIntern += Time.deltaTime;
                base.RunTimer(_maxRoundTime);
                /*var remainingTime = _timeoutDelay - _timer;
                var remainingSeconds = Mathf.Floor(remainingTime);
                var remainingMilliseconds = Mathf.Floor((remainingTime - remainingSeconds) * 1000f);
    
                var remainingText = $"{remainingSeconds:00}:{remainingMilliseconds:00}";
                if (remainingTime >= 0)
                {
                    timerTextMesh.text = remainingText;
                }
                */
                if (_timerIntern >= _maxRoundTime)
                {
                    HandleScoreChange();
                
                }
            }
        }

        private void HandleScoreChange()
        {
            //base.AnimateFail(failsToLose , 3);
            _previousButton.gameObject.SetActive(false);
            //var damageIconGo = Instantiate(damageTakenSprite.gameObject, transform.parent);
            //damageIconGo.SetActive(true);
            //Destroy(damageIconGo, 1);
            ResetTimer();
            Fail();
        }

        public void ResetTimer()
        {
            Debug.Log("It took " + _timerIntern + " to react");
            _timerIntern = 0;
        }

        #endregion Game Mechanics / Methods

        #region Overarching Methods / Helpers

        private void ActivateObjectAtRandomPos()
        {
            // Make sure only one Button is visible at a time
            if (_previousButton && _previousButton.gameObject.activeSelf) return;
            

            // Get a random TextMeshPro from the list
            var randomIndex = Random.Range(0, _spawnedButtons.Count);
            var randomButton = _spawnedButtons[randomIndex];

            
            // Calculate the maximum allowed positions within the play area bounds
            var minX = _playarea.xMin + (_buttonWidth);
            var maxX = _playarea.xMax - (_buttonWidth);
            var minY = _playarea.yMin + (_buttonWidth);
            var maxY = _playarea.yMax - (_buttonWidth);
            

            // Calculate the random world position within the play area bounds
            var randomX = Random.Range(minX, maxX);
            var randomY = Random.Range(minY, maxY);

            var randomWorldPos = new Vector3(randomX, randomY, randomButton.transform.position.z);

            randomButton.transform.localPosition = randomWorldPos;
            randomButton.gameObject.SetActive(true);

            _previousButton = randomButton;
        }


        private IEnumerator SpawnCoroutine()
        {
            while (true)
            {
                var randomDelay = Random.Range(spawnTimeLowerBounds, spawnTimeUpperBounds);
                yield return new WaitForSeconds(randomDelay);
                ActivateObjectAtRandomPos();
                
                
            }
        }

        #endregion Overarching Methods / Helpers

        public void IncreaseScore()
        {
            //base.AnimateSuccess(_currentScore, _scoreToWin);
            //base.ScoreUp();
            Success();
        }
    }
}