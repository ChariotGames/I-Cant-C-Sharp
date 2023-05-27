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
            
            RotatePlayer();
        }

        #endregion Built-Ins / MonoBehaviours

        #region GetSets / Properties

            

        #endregion GetSets / Properties

        #region Game Mechanics / Methods

            

        public void RotatePlayer()
        {
            var inputDelta = InputHandler.LeftStickDelta.x;
            //  Mathf.Sign will return 1,-1 or 0
            var direction = Vector2.right * Mathf.Sign(inputDelta);

            // calculate the angle to rotate the barrel
            var angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90;

            angle = Mathf.Repeat(angle + 180f, 360f) - 180f;

            // clamp the angle to the range of -maxRotation to maxRotation
            //angle = Mathf.Clamp(angle, -maxRotation, maxRotation);
       
            // multiply the maxRotationSpeed by the inputDelta value
            var rotationSpeed = maxRotationSpeed * Mathf.Abs(inputDelta);
            var targetRotation = Quaternion.Euler(0f, 0f, angle);
            gameObject.transform.rotation = Quaternion.RotateTowards(gameObject.transform.rotation, targetRotation, rotationSpeed); ;

        }

        #endregion Game Mechanics / Methods

        #region Overarching Methods / Helpers



        #endregion Overarching Methods / Helpers
    }
}