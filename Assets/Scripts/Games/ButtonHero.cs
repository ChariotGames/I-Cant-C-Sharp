using System.Collections;
using System.Collections.Generic;
using Scripts.GameElements;
using TMPro;
using UnityEngine;
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

        #endregion Serialized Fields

        #region Fields

        private Camera _mainCamera;
        private TextMeshPro _previousButton;
        private Bounds _cameraViewportBounds;
        private readonly List<TextMeshPro> _spawnedButtons = new ();
        private static float _timer;
        private float _timeoutDelay = 5f;
        private int _maxFails = 3;
        private float _elapsedTime;
        private float _timeoutStemp;
        private int _currentScore;

        #endregion Fields

        #region Built-Ins / MonoBehaviours
        
        private void Awake()
        {
            base.SetUp();
            _mainCamera = Camera.main;
            for (var i = buttons.Count - 1; i >= 0; i--)
            {
                // just pool all the objects into a list
                GameObject button = Instantiate(buttons[i].gameObject);
                TextMeshPro buttonText = button.GetComponent<TextMeshPro>();
                button.GetComponent<BasePressElement>().Button = keys.All[i].Input;
                _spawnedButtons.Add(buttonText);
                button.SetActive(false);
            }
        }

        private void Start()
        {            
            // Calculate the camera's viewport bounds
            _cameraViewportBounds = new Bounds(_mainCamera.transform.position, _mainCamera.ViewportToWorldPoint(new Vector3(1f, 1f, 0f)) - _mainCamera.ViewportToWorldPoint(Vector3.zero));

            StartCoroutine(SpawnCoroutine());
        }

        private void Update()
        {

            if (_previousButton && _previousButton.gameObject.activeSelf)
            {
                _timer += Time.deltaTime;
                var seconds = Mathf.Floor(_timer);
                var milliseconds = Mathf.Floor((_timer - seconds) * 1000f); 
                var timeText = $"{seconds:00}:{milliseconds:00}";
                timerTextMesh.text = timeText;
                if (_timer >= _timeoutDelay)
                {
                    _maxFails--;
                    _previousButton.gameObject.SetActive(false);
                    ResetTimer();
                    if (_maxFails <= 0)
                    {
                        _maxFails = 3;
                        base.Lose();
                    }
                }
               
            }
        }

        #endregion Built-Ins / MonoBehaviours

        #region GetSets / Properties

            

        #endregion GetSets / Properties

        #region Game Mechanics / Methods
        

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

            // Calculate the preferred values of the text
            var preferredValues = randomButton.GetPreferredValues();
            var preferredWidth = preferredValues.x;
            var preferredHeight = preferredValues.y;

            // Calculate the maximum allowed positions within the camera viewport bounds
            var minX = _cameraViewportBounds.min.x + (preferredWidth * 0.5f);
            var maxX = _cameraViewportBounds.max.x - (preferredWidth * 0.5f);
            var minY = _cameraViewportBounds.min.y + (preferredHeight * 0.5f);
            var maxY = _cameraViewportBounds.max.y - (preferredHeight * 0.5f);

            // Calculate the random world position within the camera viewport bounds
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
                ActivateObjectAtRandomPos();
                yield return new WaitForSeconds(randomDelay);
            }
        }

        #endregion Overarching Methods / Helpers

        public void IncreaseScore()
        {
            _currentScore++;
            if (_currentScore >= 10)
            {
                _currentScore = 0;
                base.Win();
                
            }
        }
    }
}