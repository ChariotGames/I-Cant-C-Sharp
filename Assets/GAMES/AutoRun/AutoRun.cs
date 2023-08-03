using System;
using System.Collections;
using System.Collections.Generic;
using Scripts.Models;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
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
        [SerializeField] private GameObject spike, wall, ceilingSpike;
        [SerializeField] public List<GameObject> obstacles = new List<GameObject>();

        public float scrollSpeed = 0;


        #region Game Mechanics / Methods

        private IEnumerator Start()
        {
            SetDifficulty();
            yield return StartCoroutine(AnimateInstruction());
            gameObject.GetComponent<ScriptMachine>().enabled = true;
        }
        
        private protected override void SetDifficulty()
        {
            obstacles = new List<GameObject>();
            obstacles.Add(spike);
            
            switch (Difficulty)
            {
                case Difficulty.EASY:
                    scrollSpeed = 0.75f;
                    break;
                case Difficulty.MEDIUM:
                    scrollSpeed = 1f;
                    obstacles.Add(wall);
                    break;
                case Difficulty.HARD:
                    scrollSpeed = 1.25f;
                    obstacles.Add(wall);
                    obstacles.Add(ceilingSpike);
                    break;
            }
        }

        #region inputs
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
        
        #endregion
        
        public void baseSuccess()
        {
            base.Success();
        }

        public void baseFail()
        {
            base.Fail();
        }

        public Sprite getCharacter()
        {
            return settings.SelectedCharacter.Preview;
        }

        public int getSuccessesToWin()
        {
            return successesToWin;
        }

        public int getFailsToLose()
        {
            return failsToLose;
        }

        public int getFails()
        {
            return _fails;
        }

        #endregion Game Mechanics / Methods

        #region Overarching Methods / Helpers
        
        

        #endregion Overarching Methods / Helpers
    }
}