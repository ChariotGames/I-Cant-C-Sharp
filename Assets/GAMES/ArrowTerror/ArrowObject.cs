using Scripts.Models;
using UnityEngine;

namespace Scripts.Games
{
    public class ArrowObject : MonoBehaviour
    {
        [SerializeField] private ArrowTerror game;
        [SerializeField] private Collider2D player;
        [SerializeField] [Range(0.25f, 1f)] private float EnemySpeed = 0.1f;

        public ElementType type;

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
            Vector3 directionToTarget = playerPosition - transform.position;
            float angle = Mathf.Atan2(directionToTarget.y, directionToTarget.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            gameObject.GetComponent<SpriteRenderer>().flipY = false;
            transform.GetChild(0).GetComponent<SpriteRenderer>().flipY = false;
            //gameObject.GetComponent<SpriteRenderer>().flipX = false;
            //transform.GetChild(0).GetComponent<SpriteRenderer>().flipX = false;
            if (Mathf.Abs(angle) <= 180 && Mathf.Abs(angle) >= 90)
            {
                gameObject.GetComponent<SpriteRenderer>().flipY = true;
                transform.GetChild(0).GetComponent<SpriteRenderer>().flipY = true;
                //gameObject.GetComponent<SpriteRenderer>().flipX = true;
                //transform.GetChild(0).GetComponent<SpriteRenderer>().flipX = true;
            }
        }
    }
}
