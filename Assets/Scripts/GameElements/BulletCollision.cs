using UnityEngine;

namespace Scripts.Pascal
{
    public class BulletCollision : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D other)
        {
            Destroy(gameObject);
            Destroy(other.gameObject);
        }
    }
}
