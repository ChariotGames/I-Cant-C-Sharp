using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.TextCore.Text;

namespace _Scripts.Games
{
    public class NumberVsSmileys : BaseGame
    {
        #region Serialized Fields

        [SerializeField] private GameObject smiley, number;

        #endregion Serialized Fields

        #region Fields
        
        

        #endregion Fields

        #region Built-Ins / MonoBehaviours

        void Start()
        {
            TMP_Text text = smiley.transform.GetComponent<TMP_Text>();
        }

        void Update()
        {
            
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