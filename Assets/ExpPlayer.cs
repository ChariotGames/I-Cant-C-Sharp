using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using _Scripts._Input;
using UnityEngine;

namespace _Scripts.Games
{
    public class ExpPlayer : BaseGame
    {
        [SerializeField] private float movementSpeed = 10f;
        [SerializeField] private InputActionReference[] _buttons;

        private Rigidbody2D rb;

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

        // Update is called once per frame
        void Update()
        {
            move();
        }
    }
}
