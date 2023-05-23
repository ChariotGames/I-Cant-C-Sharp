using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _Scripts
{
    public class P_Level02 : MonoBehaviour
    {
        [SerializeField] private GameObject cannonPrefab;
        [SerializeField] private GameObject obstaclePrefab;
        [SerializeField] private float spawnTimeUpperBounds;
        [SerializeField] private float spawnTimeLowerBounds;
        
        private Camera _mainCamera;
        private Bounds _cameraViewportBounds;
       

        private void Awake()
        {
            _mainCamera = Camera.main;
        }

        private void Start()
        {
            // Calculate the camera's viewport bounds
            _cameraViewportBounds = new Bounds(_mainCamera.transform.position, _mainCamera.ViewportToWorldPoint(new Vector3(1f, 1f, 0f)) - _mainCamera.ViewportToWorldPoint(Vector3.zero));
            
            Instantiate(cannonPrefab);
            StartCoroutine(SpawnCoroutine());
        }
        
        private IEnumerator SpawnCoroutine()
        {
            while (true)
            {
                var randomDelay = Random.Range(spawnTimeLowerBounds, spawnTimeUpperBounds);
                SpawnObstacle();
                yield return new WaitForSeconds(randomDelay);
            }
        }

        private void SpawnObstacle()
        {
            
            var numObstacles = Random.Range(1, 4);

            for (var i = 0; i < numObstacles; i++)
            {
                // Generate a random horizontal position within the camera viewport bounds
                var x = Random.Range(_cameraViewportBounds.min.x, _cameraViewportBounds.max.x);
                var obstacleGO = Instantiate(obstaclePrefab, new Vector3(x, _cameraViewportBounds.max.y, 0f), Quaternion.identity);
            }
        }

    }
}
