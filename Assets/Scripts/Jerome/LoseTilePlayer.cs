using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Scripts._Input;

namespace Scripts.Games
{
    public class LoseTilePlayer : MonoBehaviour
    {
        #region Serialized Fields
        [SerializeField] LoseTile MainGame;
        [SerializeField] float speed = 5f;


        #endregion Serialized Fields

        #region Fields



        #endregion Fields

        #region Built-Ins / MonoBehaviours

        void Start()
        {
            
        }

        void Update()
        {
            gameObject.transform.Translate(InputHandler.StickLeft * Time.deltaTime * speed);
           
        }

        #endregion Built-Ins / MonoBehaviours

        #region GetSets / Properties
        
        

        #endregion GetSets / Properties

        #region Game Mechanics / Methods
        
        

        public void TemplateMethod(bool param)
        {
            
        }

        #endregion Game Mechanics / Methods

        #region Overarching Methods / Helpers
        
        

        #endregion Overarching Methods / Helpers
    }
}