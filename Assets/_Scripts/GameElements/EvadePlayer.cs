using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using _Scripts._Input;
using UnityEngine;

namespace _Scripts.Games
{
    public class EvadePlayer : BaseGame
    {

        [SerializeField] private float movementSpeed = 10f;
        [SerializeField] private InputActionReference[] _buttons;

        private void move()
        {
            float playerPos = transform.position.x;
            float moveAmount = InputHandler.StickLeft.x * movementSpeed * Time.deltaTime;
            float newPos = Mathf.Clamp(playerPos + moveAmount, -3f, 3f);

            transform.position = new Vector3(newPos, transform.position.y, transform.position.z);
        }

        // Update is called once per frame
        void Update()
        {
            move();
        }
    }
}
