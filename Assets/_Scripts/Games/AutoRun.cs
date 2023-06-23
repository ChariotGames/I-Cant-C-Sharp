using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _Scripts.Games
{
    /// <summary>
    /// its a simple Autorunner like Geometry Dash.
    /// player can Jump, Duck and brace himself in 2 colors.
    /// for that either the face buttons or the Dpad is used,depending on a bool from the Autorun prefab called "playerFaceButtonControls".
    /// Lose condition: Player loses all lives, as indicated by the PlayerColor. Hitpoints can be set with "playerHP".
    /// Win condition: player survives a set amount of Obstacles. can be set with variable "obstaclesUntilWinCon".
    /// 
    /// On Level 1, Spikes appear. Player has to jump with FaceSouth / Dpad Down
    /// On Level 2, Spikes & Walls appear & game is faster.
    /// Player has to Brace for the Walls with FaceNorth/DpadUp or FaceWest/DpadLeft depending on the color of the Wall.
    /// On Level 3, Spikes, Walls & "CeilingSpikes" appear & game is even faster. Player has to duck under them with FaceEast/DpadRight
    /// </summary>
    public class AutoRun : BaseGame
    {

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

        public void winCondition()
        {
            base.Win();
        }

        public void loseCondition()
        {
            base.Lose();
        }

        public void easier()
        {
            
        }

        #endregion Overarching Methods / Helpers
    }
}