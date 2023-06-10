using System;
using System.Collections;
using System.Collections.Generic;
using _Scripts._Interfaces;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _Scripts.Games
{
    public class ButtonHero : BaseGame
    {
        #region Fields

        [SerializeField] private List<GameObject> buttonPrefabs;
        [SerializeField] private float spawnTimeUpperBounds;
        [SerializeField] private float spawnTimeLowerBounds;
        [SerializeField] private TextMeshPro timerTextMesh;
        
        
        private Camera _mainCamera;
        private TextMeshPro _previousButton;
        private Bounds _cameraViewportBounds;
        private readonly List<TextMeshPro> spawnedButtons = new ();
        private static float _timer;
  

        #endregion Fields

        #region Built-Ins / MonoBehaviours

       
        private void Awake()
        {
            _mainCamera = Camera.main;
            for (var i = buttonPrefabs.Count - 1; i >= 0; i--)
            {
                var buttonPrefab = buttonPrefabs[i];
                // just pool all the objects into a list
                var button = Instantiate(buttonPrefab);
                var buttonRenderer = button.GetComponent<TextMeshPro>();
                spawnedButtons.Add(buttonRenderer);
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
            }


        }

        #endregion Built-Ins / MonoBehaviours

        #region GetSets / Properties

            

        #endregion GetSets / Properties

        #region Game Mechanics / Methods
        
        public static void ResetTimer()
        {
            Debug.Log("It took " + _timer + " to react");
            _timer = 0;
            
        }
            
        private void ActivateObjectAtRandomPos()
        {
            // Make sure only one Button is visible at a time
            if (_previousButton && _previousButton.gameObject.activeSelf)
            {
                return;
            }

            
            // Get a random TextMeshPro from the list
            var randomIndex = Random.Range(0, spawnedButtons.Count);
            var randomButton = spawnedButtons[randomIndex];

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
        
        #endregion Game Mechanics / Methods

        #region Overarching Methods / Helpers

            

        #endregion Overarching Methods / Helpers
    }
}