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
        [SerializeField][Range (3f, 3.6f)] float x = 3.3f;
        [SerializeField][Range (3f, 3.6f)] float y = 3.3f;




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

            if (transform.localPosition.x >= x) transform.localPosition = new Vector2(x, transform.localPosition.y);
            if (transform.localPosition.x <= -x) transform.localPosition = new Vector2(-x, transform.localPosition.y);
            if (transform.localPosition.y >= y) transform.localPosition = new Vector2(transform.localPosition.x, y);
            if (transform.localPosition.y <= -y) transform.localPosition = new Vector2(transform.localPosition.x, -y);
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