using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _Scripts.Games
{
    /// <summary>
    /// G10 - Colored Traffic lights
    ///Description
    ///     traffic lights with 2 colors appear for a few seconds. player has to memorize them, then they turn black.
    ///     then after a short delay differently colored traffic lights appear and on the middle one a selector appears,
    ///     player has to select one that has the same colors as one from the start
    ///
    ///LVL0: 1 traffic light at the start, then 3. only 1 traffic light is correct.
    ///LVL1: 2 at the start, then 5. only 1 traffic light is correct.
    ///LVL2: 3 at the start, then 7. 2 traffic lights are correct.
    ///
    ///Win Condition
    ///     move the selector to one of the correct traffic lights and let the time run out
    ///
    ///Lose Condition
    ///     time runs out and a wrong traffic light has been selected
    ///
    ///Default Input
    ///     Any 2 buttons to resemble left & right:
    ///     Dpad left right, face left right, shoulderbuttons, trigger.
    ///Genre / Type
    ///     memory
    /// </summary>
    public class NewGame : Game
    {
        /**
         * TODO: General Structure Ideas:
         * 
         * Try to keep an order of fields from most complex to primitive.
         * GameObject go;
         * struct point;
         * float num;
         * bool truthy;
         * 
         * Constants before variables maybe too.
         * const int TIME_PLANNED_FOR_THIS
         * int timeSpentOnThis
         * 
         * Also from most public to private. Valid for methods too.
         * public
         * internal
         * protected
         * private
         * 
         *  Then only probably by alphabet. If at all
         */

        #region Serialized Fields

        // TODO: Put all the serialized fields here.

        #endregion Serialized Fields

        #region Fields

        // TODO: Put general non-serialized fields here.

        #endregion Fields

        #region Built-Ins / MonoBehaviours

        // TODO: Put Unity built-in event methods here.
        // Such as Awake, Start, Update.
        // But also OnEnable, OnDestroy, OnTrigger and such.

        // Start is called before the first frame update
        void Start()
        {
            
        }

        // Update is called once per frame
        void Update()
        {
            
        }

        #endregion Built-Ins / MonoBehaviours

        #region GetSets / Properties

        // TODO: Put Auto-Properties to your fields here.
        //
        // These are used for private fields as getters and setters.
        // Since they are very game specific, they are down here.
        // The structure is (amost) always the same. Copy-Paste.

        /**
        public returnType FieldNameWithCapitalStart
        {
            get { return fieldName; }
            set { fieldName = value; }
        }
        */

        #endregion GetSets / Properties

        #region Game Mechanics / Methods

        // TODO: Put your game specific mechanics here.
        // If they can be grouped by functionality, do so.

        /// <summary>
        /// TODO: Provide a summary for the method
        /// </summary>
        /// <param name="param">List the parameters.</param>
        /// <returns>Specify what it returns, if it does so.</returns>

        public void TemplateMethod(bool param)
        {
            // TODO: YOUR CODE GOES HERE
        }

        #endregion Game Mechanics / Methods

        #region Overarching Methods / Helpers

        // TODO: Put your helper methods here.

        #endregion Overarching Methods / Helpers
    }
}