using System;
using System.Collections;
using System.Collections.Generic;
using _Scripts.Pascal;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _Scripts.Games
{
    public class FallingObstacles : BaseGame
    {
        #region Fields

        [SerializeField] private Cannon cannonPrefab;
        [SerializeField] private GameObject obstaclePrefab;
        [SerializeField] private float cannonMovementSpeed;
        [SerializeField] private TextMeshPro lifeCounterTextMeshPro;

        private float _spawnDelay;
        private Camera _mainCamera;
        private Bounds _cameraViewportBounds;
        private int _numObstacles;
        private Cannon _spawnedCannon;
        private Vector3 _obstacleExtents;
        private int _healthPoints = 3;

        #endregion Fields

        #region Built-Ins / MonoBehaviours

        private void Awake()
        {
            _mainCamera = Camera.main;
        }

        private void OnEnable()
        {
            BottomBounds.damageTaken += TakeDamage;
        }
        

        private void Start()
        {
            // Calculate the camera's viewport bounds
            _cameraViewportBounds = new Bounds(_mainCamera.transform.position, _mainCamera.ViewportToWorldPoint(new Vector3(1f, 1f, 0f)) - _mainCamera.ViewportToWorldPoint(Vector3.zero));
            
            lifeCounterTextMeshPro.text = _healthPoints.ToString();
            
            _obstacleExtents = obstaclePrefab.GetComponent<Renderer>().bounds.extents;
           
            Difficulty = Difficulty.LVL1;
           
            _spawnedCannon = Instantiate(cannonPrefab);

            // ReSharper disable once SwitchStatementMissingSomeEnumCasesNoDefault
            switch (Difficulty)
            {
                case Difficulty.LVL1:
                    _numObstacles = 1;
                    _spawnDelay = 1.5f;
                    break;
                case Difficulty.LVL2:
                    _numObstacles = 2;
                    _spawnDelay = 2f;
                    break;
                case Difficulty.LVL3:
                    _spawnDelay = 2f;
                    _numObstacles = 2;
                    ActivateHorizontalMovement();
                    break;
            }


            StartCoroutine(SpawnCoroutine());
        }

        private void OnDisable()
        {
            BottomBounds.damageTaken -= TakeDamage;
        }

        #endregion Built-Ins / MonoBehaviours

        #region GetSets / Properties

            

        #endregion GetSets / Properties

        #region Game Mechanics / Methods
        
        private void TakeDamage()
        {
            _healthPoints--;
            if (_healthPoints == 0)
            {
                Lose();
            }
            lifeCounterTextMeshPro.text = _healthPoints.ToString();
        }

        private IEnumerator SpawnCoroutine()
        {
            // endless loop
            while (true)
            {
                SpawnObstacle();
                yield return new WaitForSeconds(_spawnDelay);
            }
        }



        private IEnumerator HorizontalMovementCoroutine(Bounds cannonBounds)
        {
            
            var targetXPos = _spawnedCannon.transform.position.x;
            var minX = _cameraViewportBounds.min.x + cannonBounds.extents.x;
            var maxX = _cameraViewportBounds.max.x - cannonBounds.extents.x;

            while (true)
            {
                targetXPos += cannonMovementSpeed * Time.deltaTime;

                if (targetXPos > maxX)
                {
                    targetXPos = maxX;
                    cannonMovementSpeed *= -1f; 
                }
                else if (targetXPos < minX)
                {
                    targetXPos = minX;
                    cannonMovementSpeed *= -1f;
                }

                var position = _spawnedCannon.transform.position;
                position = new Vector3(targetXPos, position.y, position.z);
                _spawnedCannon.transform.position = position;

                yield return null;
            }
        }

        

        private void SpawnObstacle()
        {
            var spawnedPositions = new List<Vector3>();

            for (var i = 0; i < _numObstacles; i++)
            {
                Vector3 obstaclePosition;

                do
                {
                    var x = Random.Range(_cameraViewportBounds.min.x + _obstacleExtents.x, _cameraViewportBounds.max.x - _obstacleExtents.x);
                    obstaclePosition = new Vector3(x, _cameraViewportBounds.max.y, 0f);

                } while (!IsPositionValid(obstaclePosition, spawnedPositions));

                var obstacleGO = Instantiate(obstaclePrefab, obstaclePosition, Quaternion.identity);
                spawnedPositions.Add(obstaclePosition);
            }
        }

        private bool IsPositionValid(Vector3 position, List<Vector3> occupiedPositions)
        {
            foreach (var occupiedPosition in occupiedPositions)
            {
                var distance = Mathf.Abs(occupiedPosition.x - position.x);
                if (distance < _obstacleExtents.x * 2f)
                {
                    return false;
                }
            }

            return true;
        }


        private void ActivateHorizontalMovement()
        {
            var cannonBounds = cannonPrefab.GetComponentInChildren<Renderer>().bounds;
            StartCoroutine(HorizontalMovementCoroutine(cannonBounds));
        }

        #endregion Game Mechanics / Methods

        #region Overarching Methods / Helpers

            

        #endregion Overarching Methods / Helpers
    }
}