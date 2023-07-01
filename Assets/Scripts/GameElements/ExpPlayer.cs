using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using Scripts._Input;
using UnityEngine;

namespace Scripts.Games
{
    public class ExpPlayer : BaseGame
    {
        [SerializeField] private float movementSpeed = 5f;
        [SerializeField] private InputActionReference[] _buttons;

        public Rigidbody2D rb;
        public Vector2 knockback = new(0, 0);
        private float knockbackDamp = 1.2f;

        private void Start()
        {
            rb = gameObject.GetComponent<Rigidbody2D>();
        }

        private void move()
        {
            Vector2 input = new Vector2(InputHandler.StickLeft.x, InputHandler.StickLeft.y).normalized;
            Vector2 move = input * movementSpeed;
            Debug.Log(knockback);
            move += knockback;
            rb.velocity = move;
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
