using Scripts.Models;
using Scripts.Pascal;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Scripts.Games
{
    /// <summary>
    /// 
    /// </summary>
    public class FallingObstacles : BaseGame
    {
        #region Serialized Fields

        [SerializeField] private Cannon cannon;
        [SerializeField] private GameObject obstacle, obstacleContainer;
        [SerializeField] private TextMeshPro lifeCounter;
        [SerializeField] private float cannonMovementSpeed;
        

        #endregion Serialized Fields

        #region Fields

        private Camera _mainCamera;
        private Bounds _cameraViewportBounds;
        private Vector3 _obstacleExtents;
        private float _spawnDelay;
        private int _healthPoints = 3;
        private int _numObstacles;
        private int _currentScore;
        
        private const int _scoreToWin = 15;

        #endregion Fields

        #region Built-Ins / MonoBehaviours

        private void Awake()
        {
            _mainCamera = Camera.main;
        }

        private void OnEnable()
        {
            BottomBounds.DamageTaken += TakeDamage;
        }

        private void Start()
        {
            // Calculate the camera's viewport bounds
            _cameraViewportBounds = new Bounds(_mainCamera.transform.position, _mainCamera.ViewportToWorldPoint(new Vector3(1f, 1f, 0f)) - _mainCamera.ViewportToWorldPoint(Vector3.zero));
            
            lifeCounter.text = "Healthpoints : " + _healthPoints.ToString();
            
            _obstacleExtents = obstacle.GetComponent<Renderer>().bounds.extents;

            cannon.gameObject.SetActive(true);

            // ReSharper disable once SwitchStatementMissingSomeEnumCasesNoDefault
            switch (Difficulty)
            {
                case Difficulty.EASY:
                    _numObstacles = 1;
                    _spawnDelay = 1.5f;
                    break;
                case Difficulty.MEDIUM:
                    _numObstacles = 2;
                    _spawnDelay = 2f;
                    break;
                case Difficulty.HARD:
                    _numObstacles = 2;
                    _spawnDelay = 2f;
                    ActivateHorizontalMovement();
                    break;
            }

            StartCoroutine(SpawnCoroutine());
        }

        private void OnDisable()
        {
            BottomBounds.DamageTaken -= TakeDamage;
        }

        #endregion Built-Ins / MonoBehaviours

        #region Game Mechanics / Methods



        public void IncreasePoints()
        {
            _currentScore++;
            base.ScoreUp();
            
            if (_currentScore >= _scoreToWin)
            {
                _currentScore = 0;
                base.Harder();
                base.Win();
            }
        }

        /// <summary>
        /// Is called via an event when an obstacle hits the ground.
        /// </summary>
        private void TakeDamage()
        {
            _healthPoints--;
            base.ScoreDown();
            if (_healthPoints <= 0)
            {
                _healthPoints = 3;
                base.Easier();
                base.Lose();
            }
            lifeCounter.text = "Healthpoints : " + _healthPoints.ToString();
        }
        
        /// <summary>
        /// The actual mechanic that allows moving the cannon horizontally.
        /// </summary>
        /// <param name="cannonBounds">The bounding boy of the canon.</param>
        /// <returns></returns>
        private IEnumerator HorizontalMovementCoroutine(Bounds cannonBounds)
        {
            var targetXPos = cannon.transform.position.x;
            var minX = _playarea.min.x + cannonBounds.extents.x;
            var maxX = _playarea.max.x - cannonBounds.extents.x;

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

                var position = cannon.transform.position;
                position = new Vector3(targetXPos, position.y, position.z);
                cannon.transform.position = position;

                yield return null;
            }
        }

        /// <summary>
        /// Actually spawns a new obstacle object within the camera bounds.
        /// </summary>
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

                Instantiate(obstacle, obstaclePosition, Quaternion.identity, obstacleContainer.transform).SetActive(true);

                spawnedPositions.Add(obstaclePosition);
            }
        }

        #endregion Game Mechanics / Methods

        #region Overarching Methods / Helpers


        /// <summary>
        /// Activates horizontal movement for level 3.
        /// </summary>
        private void ActivateHorizontalMovement()
        {
            var cannonBounds = cannon.GetComponentInChildren<Renderer>().bounds;
            StartCoroutine(HorizontalMovementCoroutine(cannonBounds));
        }

        /// <summary>
        /// Checks if a new obstacle can be spawned.
        /// </summary>
        /// <param name="position">The new position.</param>
        /// <param name="occupiedPositions">The already taken positions.</param>
        /// <returns></returns>
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

        /// <summary>
        /// Kick-starts and executes the actual spawning routine.
        /// </summary>
        /// <returns></returns>
        private IEnumerator SpawnCoroutine()
        {
            // endless loop
            while (true)
            {
                SpawnObstacle();
                yield return new WaitForSeconds(_spawnDelay);
            }
        }

        #endregion Overarching Methods / Helpers
    }
}