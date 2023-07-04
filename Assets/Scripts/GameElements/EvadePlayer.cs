using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using Scripts._Input;
using UnityEngine;

namespace Scripts.Games
{
    public class EvadePlayer : MonoBehaviour
    {

        [SerializeField] private float movementSpeed = 10f;
        [SerializeField] public InputActionReference stick;

        private void move()
        {
            float playerPos = transform.position.x;
            float moveAmount = stick.action.ReadValue<Vector2>().x * movementSpeed * Time.deltaTime;
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
