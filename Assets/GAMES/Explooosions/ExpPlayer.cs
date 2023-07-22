using UnityEngine.InputSystem;
using UnityEngine;
using System.Collections;

namespace Scripts.Games
{
    public class ExpPlayer : MonoBehaviour
    {
        [SerializeField] private float movementSpeed = 5f;
        [SerializeField] public InputActionReference stick;
        [SerializeField] Exploooosions parent;

        public Rigidbody2D rb;
        public Vector2 knockback = new(0, 0);
        private float knockbackDamp = 1.2f;

        private void Start()
        {
            rb = gameObject.GetComponent<Rigidbody2D>();
        }

        private void move()
        {
            Vector2 input = new Vector2(stick.action.ReadValue<Vector2>().x, stick.action.ReadValue<Vector2>().y).normalized;
            Vector2 move = input * movementSpeed;
            move += knockback;
            rb.velocity = move;

            if (transform.position.x >= parent.Playarea.xMax) transform.position = new Vector2(parent.Playarea.xMax, transform.position.y);
            if (transform.position.x <= parent.Playarea.xMin) transform.position = new Vector2(parent.Playarea.xMin, transform.position.y);
            if (transform.position.y >= parent.Playarea.yMax) transform.position = new Vector2(transform.position.x, parent.Playarea.yMax);
            if (transform.position.y <= parent.Playarea.yMin) transform.position = new Vector2(transform.position.x, parent.Playarea.yMin);
        }

        public IEnumerator AnimateColor(SpriteRenderer sprite, Color original, Color target, float duration)
        {
            float elapsedTime = 0f;
            while (elapsedTime < duration)
            {
                sprite.color = Color.Lerp(original, target, elapsedTime / duration);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            elapsedTime = 0f;
            while (elapsedTime < duration)
            {
                sprite.color = Color.Lerp(target, original, elapsedTime / duration);
                elapsedTime += Time.deltaTime;
                yield return null;
            }
            sprite.color = original;
        }

        // Update is called once per frame
        void Update()
        {
            move();
            knockback /= knockbackDamp;
            if (knockback.magnitude <= 0.01f)
            {
                knockback = Vector2.zero;
            }
        }
    }
}
