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

        [SerializeField] protected Difficulty currentDifficulty = Difficulty.EASY;
        [SerializeField] protected ActionName actionNames;
        

        #endregion Serialized Fields

        #region Fields

        protected Bounds bounds;
        protected KeyMap keys;
        protected MinigameManager manager;

        #endregion Fields

        #region Built-Ins

        
        protected void SetUp()
        {
            manager = GameObject.Find(nameof(MinigameManager)).GetComponent<MinigameManager>();
        }

        #endregion

        #region Methods

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
            Manager.SetDifficulty(gameObject, currentDifficulty - 1);
        }

        /// <summary>
        /// Makes the current game harder next time it's played.
        /// </summary>
        protected void Harder()
        {
            Manager.SetDifficulty(gameObject, currentDifficulty + 1);
        }

        #endregion  Methods

        #region GetSets

        /// <summary>
        /// The game's current difficulty.
        /// </summary>
        public Difficulty Difficulty
        {
            get => currentDifficulty;
            set => currentDifficulty = value;
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
        public Bounds Bounds
        {
            //get => bounds;
            set => bounds = value;
        }

        #endregion GetSets
    }
}