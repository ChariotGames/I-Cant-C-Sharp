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
        #region Serialized Fields

        [SerializeField] private InputAction test1;

        private InputAction test2;

        #endregion Serialized Fields

        #region Fields
        
        

        #endregion Fields

        #region Built-Ins / MonoBehaviours



        #endregion Built-Ins / MonoBehaviours
        

        #region Game Mechanics / Methods

        public InputAction getJumpInput()
        { 
            return Keys.Three.Input.action; // Up
            
        }
        public InputAction getBraceLeftInput()
        {
            return Keys.Two.Input.action; // left
        }
        
        public InputAction getBraceRightInput()
        {
            return Keys.Four.Input.action; // right
        }
        public InputAction getDuckInput()
        {
            return Keys.One.Input.action; // down
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
        }

        #endregion Game Mechanics / Methods

        #region Overarching Methods / Helpers
        
        

        #endregion Overarching Methods / Helpers
    }
}