using System.Collections;
using System.Collections.Generic;
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

        [SerializeField] private List<ButtonHeroElement> buttons;
        [SerializeField] private float spawnTimeUpperBounds;
        [SerializeField] private float spawnTimeLowerBounds;
        [SerializeField] private TextMeshPro timerTextMesh;
        [SerializeField] private SpriteRenderer damageTakenSprite;

        #endregion Serialized Fields

        #region Fields

        private TextMeshPro _previousButton;
        private readonly List<TextMeshPro> _spawnedButtons = new();
        private static float _timer;

        private int _remainingLives = 3;
        private float _elapsedTime;
        private float _timeoutStemp;
        private int _currentScore;
        private float _buttonWidth;
        
        private float _timeoutDelay;

        #endregion Fields

        #region Built-Ins / MonoBehaviours

        private void Awake()
        {
            switch (Difficulty)
            {
                case Difficulty.EASY:
                    _timeoutDelay = 3;
                    break;
                case Difficulty.MEDIUM:
                    _timeoutDelay = 2;
                    break;
                case Difficulty.HARD:
                    _timeoutDelay = 1;
                    break;
            }

            for (var i = buttons.Count - 1; i >= 0; i--)
            {
                // just pool all the objects into a list
                var button = Instantiate(buttons[i].gameObject, transform.parent);
                var buttonText = button.GetComponent<TextMeshPro>();
                button.GetComponent<BasePressElement>().Button = _keys.All[i].Input;
                _spawnedButtons.Add(buttonText);
                button.SetActive(false);
            }
        }

        private void Start()
        {
            _buttonWidth = buttons[0].gameObject.GetComponent<RectTransform>().rect.width * 0.5f;
            
            StartCoroutine(SpawnCoroutine());
        }

        private void Update()
        {
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
                _timer += Time.deltaTime;
                var remainingTime = _timeoutDelay - _timer;
                var remainingSeconds = Mathf.Floor(remainingTime);
                var remainingMilliseconds = Mathf.Floor((remainingTime - remainingSeconds) * 1000f);
    
                var remainingText = $"{remainingSeconds:00}:{remainingMilliseconds:00}";
                if (remainingTime >= 0)
                {
                    timerTextMesh.text = remainingText;
                }

                if (_timer >= _timeoutDelay)
                {
                    HandleScoreChange();
                
                }
            }
        }

        private void HandleScoreChange()
        {
            _remainingLives--;
            base.ScoreDown();
            _previousButton.gameObject.SetActive(false);
            var damageIconGo = Instantiate(damageTakenSprite.gameObject, transform.parent);
            damageIconGo.SetActive(true);
            Destroy(damageIconGo, 1);
            ResetTimer();
            if (_remainingLives <= 0)
            {
                _remainingLives = 3;
                base.Lose();
            }
        }

        public void ResetTimer()
        {
            Debug.Log("It took " + _timer + " to react");
            _timer = 0;
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

            randomButton.transform.position = randomWorldPos;
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
            _currentScore++;
            base.ScoreUp();
            if (_currentScore >= 10)
            {
                _currentScore = 0;
                base.Win();
            }
        }
    }
}