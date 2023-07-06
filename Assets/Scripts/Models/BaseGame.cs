using Scripts.Controllers;
using Scripts.Models;
using UnityEngine;

namespace Scripts.Games
{
    /// <summary>
    /// Represents an abstract idea of a game.
    /// Encapsulates the basic values and functions
    /// each game should have to function.
    /// </summary>
    public abstract class BaseGame : MonoBehaviour
    {
        #region Serialized Fields

        [SerializeField] protected Difficulty difficulty = Difficulty.EASY;
        [SerializeField] protected ActionNames actionNames;

        #endregion Serialized Fields

        #region Fields

        protected KeyMap keys;
        protected Rect playarea;
        private MinigameManager manager;

        #endregion Fields

        #region Methods

        public void SetUp(MinigameManager manager, Difficulty difficulty, KeyMap keys, Rect area)
        {
            this.difficulty = difficulty;
            this.manager = manager;
            this.keys = keys;
            this.playarea = area;
        }

        protected void ScoreDown()
        {
            manager.ScoreUpdate(-(int)difficulty);
        }

        protected void ScoreUp()
        {
            manager.ScoreUpdate((int)difficulty);
        }

        /// <summary>
        /// Informs the BaseGame Controller, that the game triggered a win condition
        /// </summary>
        protected void Win()
        {
            Manager.WinCondition(gameObject);
        }

        /// <summary>
        /// Informs the BaseGame Controller, that the game triggered a lose condition
        /// </summary>
        protected void Lose()
        {
            Manager.LoseCondition(gameObject);
        }

        /// <summary>
        /// Makes the current game easier next time it's played.
        /// </summary>
        protected void Easier()
        {
            Manager.SetDifficulty(gameObject, difficulty - 1);
        }

        /// <summary>
        /// Makes the current game harder next time it's played.
        /// </summary>
        protected void Harder()
        {
            Manager.SetDifficulty(gameObject, difficulty + 1);
        }

        #endregion  Methods

        #region GetSets

        /// <summary>
        /// The game's current difficulty.
        /// </summary>
        public Difficulty Difficulty
        {
            get => difficulty;
            set => difficulty = value;
        }

        /// <summary>
        /// The game's set key map.
        /// </summary>
        public KeyMap Keys
        {
            get => keys;
            set => keys = value;
        }

        /// <summary>
        /// The Minigame Manager handling game controlls.
        /// </summary>
        public MinigameManager Manager
        {
            get => manager;
            //set => manager = value;
        }

        /// <summary>
        ///  The minigame's inner bounds, set by the manager.
        /// </summary>
        public Rect Playarea
        {
            //get => bounds;
            set => playarea = value;
        }

        #endregion GetSets
    }
}