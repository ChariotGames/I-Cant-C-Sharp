using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _Scripts.Pascal
{
    public class P_Level01 : MonoBehaviour
    {
        [SerializeField] private List<GameObject> buttonPrefabs;
        [SerializeField] private float spawnTimeUpperBounds;
        [SerializeField] private float spawnTimeLowerBounds;
        
        private Camera _mainCamera;
        private SpriteRenderer _previousButton;
        private Bounds _cameraViewportBounds;
        private readonly List<SpriteRenderer> spawnedButtons = new ();



        private void Awake()
        {
            _mainCamera = Camera.main;
            for (var i = buttonPrefabs.Count - 1; i >= 0; i--)
            {
                var buttonPrefab = buttonPrefabs[i];
                // just pool all the objects into a list
                var button = Instantiate(buttonPrefab);
                var buttonSprite = button.GetComponent<SpriteRenderer>();
                spawnedButtons.Add(buttonSprite);
                button.SetActive(false);
            }
        }

        private void Start()
        {            
            // Calculate the camera's viewport bounds
            _cameraViewportBounds = new Bounds(_mainCamera.transform.position, _mainCamera.ViewportToWorldPoint(new Vector3(1f, 1f, 0f)) - _mainCamera.ViewportToWorldPoint(Vector3.zero));

            StartCoroutine(SpawnCoroutine());
            
        }

        private void ActivateObjectAtRandomPos()
        {
            // Make sure only one Button is visible at a time
            if (_previousButton)
            {
                if (_previousButton.gameObject.activeSelf) return;
            }
            
            // Get a random sprite from the list
            var randomIndex = Random.Range(0, spawnedButtons.Count);
            var randomButton = spawnedButtons[randomIndex];
            // Get the bounds of the sprite
            var spriteBounds = randomButton.bounds;

            // Calculate the random world position
            var randomWorldPos = new Vector3(
                Random.Range(_cameraViewportBounds.min.x + spriteBounds.extents.x, _cameraViewportBounds.max.x - spriteBounds.extents.x),
                Random.Range(_cameraViewportBounds.min.y + spriteBounds.extents.y, _cameraViewportBounds.max.y - spriteBounds.extents.y),
                0f
            );
            
            randomButton.transform.position = randomWorldPos;
            // Set random button active from list
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
    }
}