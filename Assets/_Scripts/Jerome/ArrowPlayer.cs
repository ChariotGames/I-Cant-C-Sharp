using System;
using System.Collections;
using System.Collections.Generic;
using _Scripts._Input;
using UnityEngine;

namespace _Scripts.Games
{
    public class ArrowPlayer : Game
    {
        #region Serialized Fields
        [SerializeField] ArrowTerror MainGame;
        [SerializeField] float speed = 5f;
        [SerializeField] private float maxRotationSpeed = 2.5f;
        [SerializeField] private float maxRotation = 180f;

        #endregion Serialized Fields

        #region Fields



        #endregion Fields

        #region Built-Ins / MonoBehaviours

        void Start()
        {
            
        }

        void Update()
        {
            
            gameObject.transform.Translate(InputHandler.LeftStickDelta * Time.deltaTime * speed);
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
            var inputDelta = InputHandler.LeftStickDelta;
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