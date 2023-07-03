using UnityEngine.InputSystem;
using Scripts._Input;
using System.Collections;
using UnityEngine;

namespace Scripts.Games
{
    public class MPlayer : BaseGame
    {
        [SerializeField] private float movementSpeed = 5f;
        [SerializeField] private InputActionReference[] _buttons;

        public Rigidbody2D rb;

        private void Start()
        {
            rb = gameObject.GetComponent<Rigidbody2D>();
        }

        private void move()
        {
            Vector2 input = new Vector2(InputHandler.StickLeft.x, InputHandler.StickLeft.y).normalized;
            Vector2 move = input * movementSpeed;
            rb.velocity = move;
        }

        private IEnumerator AnimateMovement(Vector3 original, Vector3 target, float duration)
        {
            float elapsedTime = 0f;
            while (elapsedTime < duration)
            {
                transform.position = Vector3.Lerp(original, target, elapsedTime / duration);
                elapsedTime += Time.deltaTime;
                yield return null;
            }
        }

        // Update is called once per frame
        void Update()
        {
            move();
        }
    }
}
