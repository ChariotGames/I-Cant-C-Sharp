using UnityEngine;

namespace _Scripts.Games
{
    public class ArrowObject : MonoBehaviour
    {
        [SerializeField] private ArrowTerror game;
        [SerializeField] private PolygonCollider2D player;
        [SerializeField] [Range(0.25f, 1f)] private float EnemySpeed = 0.1f;

        public Type type;

        private void Start()
        {
            EnemySpeed = Random.Range(0.25f, 1f);
         //   transform.position = new Vector3(Random.Range(-9f, 9f), Random.Range(-5f, 5f), 0);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.Equals(player))
            {
                game.PlayerTouched(gameObject);
            }
        }
        public void PlayerMoved(Vector3 playerPosition)
        {
            transform.position = Vector3.MoveTowards(transform.position, playerPosition, EnemySpeed * Time.deltaTime);

        }
    }
}
