using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using Scripts._Input;
using UnityEngine;

namespace Scripts.Games
{
    public class ArrowPlayer : MonoBehaviour
    {
        #region Serialized Fields
        [SerializeField] ArrowTerror parent;
        [SerializeField] float movementSpeed = 5f;
        [SerializeField] private float maxRotationSpeed = 2.5f;
        [SerializeField] private float maxRotation = 180f;

        #endregion Serialized Fields

        #region Fields
        public InputActionReference stick;
        public Rigidbody2D rb;


        #endregion Fields

        #region Built-Ins / MonoBehaviours

        void Start()
        {
            rb = gameObject.GetComponent<Rigidbody2D>();
        }

        void Update()
        {
            //transform.rotation = Quaternion.Euler(stick.action.ReadValue<Vector2>().x, stick.action.ReadValue<Vector2>().y, 0);
            //transform.Translate(stick.action.ReadValue<Vector2>() * Time.deltaTime * speed);
            var input = stick.action.ReadValue<Vector2>().normalized;
            if (input.magnitude > 0.1f) {
                float angle = Mathf.Atan2(input.y, input.x) * Mathf.Rad2Deg - 90f;
                transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            }
            move();
            
            //SendMessage("PlayerMoved",transform.position);
            //RotatePlayer();
            //transform.rotation = Quaternion.RotateTowards(transform.position, InputHandler.LeftStickDelta, 1, 1);
            parent.UpdateEnemyPositions(transform.position);      
        }

        private void move()
        {
            Vector2 input = new Vector2(stick.action.ReadValue<Vector2>().x, stick.action.ReadValue<Vector2>().y).normalized;
            Vector2 move = input * movementSpeed;
            rb.velocity = move;
        }

        #endregion Built-Ins / MonoBehaviours

        #region GetSets / Properties



        #endregion GetSets / Properties

        #region Game Mechanics / Methods



        public void RotatePlayer()
        {
            var inputDelta = stick.action.ReadValue<Vector2>();
            // calculate the angle to rotate the barrel
            var angle = Mathf.Atan2(inputDelta.y, inputDelta.x);

            var rotationSpeed = maxRotationSpeed * Mathf.Abs(angle);
            var targetRotation = Quaternion.Euler(0f, 0f, angle);
            gameObject.transform.rotation = Quaternion.RotateTowards(gameObject.transform.rotation, targetRotation, rotationSpeed);

        }

        #endregion Game Mechanics / Methods

        #region Overarching Methods / Helpers



        #endregion Overarching Methods / Helpers
    }
}