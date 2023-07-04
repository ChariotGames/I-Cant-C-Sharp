using Scripts.Games;
using UnityEngine;

namespace Scripts.Pascal
{
    public class BulletCollision : MonoBehaviour
    {
        [SerializeField] private FallingObstacles fallingObstacles;
        private void OnTriggerEnter2D(Collider2D other)
        {
            fallingObstacles.IncreasePoints();
            Destroy(gameObject);
            Destroy(other.gameObject);
        }
    }
}
