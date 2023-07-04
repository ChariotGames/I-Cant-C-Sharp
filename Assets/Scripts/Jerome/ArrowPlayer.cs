using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using Scripts._Input;
using UnityEngine;

namespace Scripts.Games
{
    public class ArrowPlayer : BaseGame
    {
        #region Serialized Fields
        [SerializeField] ArrowTerror MainGame;
        [SerializeField] float speed = 5f;
        [SerializeField] private float maxRotationSpeed = 2.5f;
        [SerializeField] private float maxRotation = 180f;

        #endregion Serialized Fields

        #region Fields
        public InputActionReference stick;


        #endregion Fields

        #region Built-Ins / MonoBehaviours

        void Start()
        {
            
        }

        void Update()
        {
            
            gameObject.transform.Translate(stick.action.ReadValue<Vector2>() * Time.deltaTime * speed);
            //SendMessage("PlayerMoved",transform.position);
            //RotatePlayer();
            //transform.rotation = Quaternion.RotateTowards(transform.position, InputHandler.LeftStickDelta, 1, 1);
            MainGame.UpdateEnemyPositions(transform.position);      
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