using System.Collections;
using System.Collections.Generic;
using Scripts.Models;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Scripts.Games
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
        [Space]
        [Header("Game Specific Stuff")]
        [SerializeField] private Settings settings;
        int successCounter = 0;

        
        #region Game Mechanics / Methods

        private IEnumerator Start()
        {
            yield return StartCoroutine(AnimateInstruction());
        }
        public InputAction getJumpInput()
        { 
            return Keys.One.Input.action; // Up
        }
        public InputAction getDuckInput()
        {
            return Keys.Two.Input.action; // down
        }
        public InputAction getBraceLeftInput()
        {
            return Keys.Three.Input.action; // left
        }
        public InputAction getBraceRightInput()
        {
            return Keys.Four.Input.action; // right
        }

        public void easier()
        {
            base.Easier();
            
        }

        public void harder()
        {
            base.Harder();
        }
        public void winCondition()
        {
            base.Win();
        }

        public void loseCondition()
        {
            base.Lose();
        }

        public void Score()
        {
            base.ScoreUp(1);
            ++successCounter;
            base.AnimateSuccess(successCounter, successesToWin );
        }

        public Sprite getCharacter()
        {
            return settings.SelectedCharacter.Preview;
        }

        public int getSuccessToWin()
        {
            return successesToWin;
        }

        #endregion Game Mechanics / Methods

        #region Overarching Methods / Helpers
        
        

        #endregion Overarching Methods / Helpers
    }
}