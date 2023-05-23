using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _Scripts.Games
{
    public class FallingKeys : Game
    {
        #region Fields

        [SerializeField] private GameObject cannonPrefab;
        [SerializeField] private GameObject obstaclePrefab;
        [SerializeField] private float spawnTimeUpperBounds;
        [SerializeField] private float spawnTimeLowerBounds;
        
        private Camera _mainCamera;
        private Bounds _cameraViewportBounds;
        private int _numObstacles;
        private GameObject _spawnedCannon;

        #endregion Fields

        #region Built-Ins / MonoBehaviours

        private void Awake()
        {
            _mainCamera = Camera.main;
        }

        private void Start()
        {
            // Calculate the camera's viewport bounds
            _cameraViewportBounds = new Bounds(_mainCamera.transform.position, _mainCamera.ViewportToWorldPoint(new Vector3(1f, 1f, 0f)) - _mainCamera.ViewportToWorldPoint(Vector3.zero));
            Difficulty = Difficulty.LVL3;
            _spawnedCannon = Instantiate(cannonPrefab);
            switch (Difficulty)
            {
                case Difficulty.LVL1:
                    _numObstacles = 1;
                    break;
                case Difficulty.LVL2:
                    _numObstacles = 2;
                    break;
                case Difficulty.LVL3:
                    _numObstacles = 4;
                    ActivateHorizontalMovement();
                    break;
            }
            
            
            StartCoroutine(SpawnCoroutine());
        }

        #endregion Built-Ins / MonoBehaviours

        #region GetSets / Properties

            

        #endregion GetSets / Properties

        #region Game Mechanics / Methods

        private IEnumerator SpawnCoroutine()
        {
            // endless loop
            while (true)
            {
                var randomDelay = Random.Range(spawnTimeLowerBounds, spawnTimeUpperBounds);
                SpawnObstacle();
                yield return new WaitForSeconds(randomDelay);
            }
        }



        private IEnumerator HorizontalMovementCoroutine(Bounds cannonBounds)
        {
            var movementSpeed = 2f;
            var targetXPos = _spawnedCannon.transform.position.x;
            var minX = _cameraViewportBounds.min.x + cannonBounds.extents.x;
            var maxX = _cameraViewportBounds.max.x - cannonBounds.extents.x;

            while (true)
            {
                targetXPos += movementSpeed * Time.deltaTime;

                if (targetXPos > maxX)
                {
                    targetXPos = maxX;
                    movementSpeed *= -1f; // Reverse movement direction
                }
                else if (targetXPos < minX)
                {
                    targetXPos = minX;
                    movementSpeed *= -1f; // Reverse movement direction
                }

                var position = _spawnedCannon.transform.position;
                position = new Vector3(targetXPos, position.y, position.z);
                _spawnedCannon.transform.position = position;

                yield return null;
            }
        }




        




        private void SpawnObstacle()
        {

            for (var i = 0; i < _numObstacles; i++)
            {
                // Generate a random horizontal position within the camera viewport bounds
                var x = Random.Range(_cameraViewportBounds.min.x, _cameraViewportBounds.max.x);
                var obstacleGO = Instantiate(obstaclePrefab, new Vector3(x, _cameraViewportBounds.max.y, 0f), Quaternion.identity);
            }
        }

        private void ActivateHorizontalMovement()
        {
            Bounds cannonBounds = cannonPrefab.GetComponentInChildren<Renderer>().bounds;
            StartCoroutine(HorizontalMovementCoroutine(cannonBounds));
        }

        #endregion Game Mechanics / Methods

        #region Overarching Methods / Helpers

            

        #endregion Overarching Methods / Helpers
    }
}