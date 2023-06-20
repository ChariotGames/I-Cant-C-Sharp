using _Scripts.Controllers;
using _Scripts.Models;
using UnityEngine;

namespace _Scripts.Games
{
    /// <summary>
    /// Represents an abstract idea of a game.
    /// Encapsulates the basic values and functions
    /// each game should have to function.
    /// </summary>
    public abstract class BaseGame : MonoBehaviour
    {
        #region Serialized Fields

        [SerializeField] protected MinigameManager manager;
        [SerializeField] protected KeyMap keys;
        [SerializeField] protected Difficulty currentDifficulty = Difficulty.EASY;

        #endregion Serialized Fields

        #region Fields

        protected AssetID id;

        #endregion Fields

        #region Methods

        #region protected

        /// <summary>
        /// Informs the BaseGame Controller, that the game triggered a win condition
        /// </summary>
        protected void Win()
        {
            Manager.WinCondition(id, gameObject);
        }

        /// <summary>
        /// Informs the BaseGame Controller, that the game triggered a lose condition
        /// </summary>
        protected void Lose()
        {
            Manager.LoseCondition(id, gameObject);
        }

        /// <summary>
        /// Makes the current game easier next time it's played.
        /// </summary>
        protected void Easier()
        {
            Manager.SetDifficulty(id, currentDifficulty - 1);
        }

        /// <summary>
        /// Makes the current game harder next time it's played.
        /// </summary>
        protected void Harder()
        {
            Manager.SetDifficulty(id, currentDifficulty + 1);
        }

        #endregion protected

        #endregion  Methods

        #region GetSets

        /// <summary>
        /// The game's unique ID.
        /// </summary>
        public AssetID ID
        {
            get => id;
            set => id = value;
        }

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
            set => manager = value;
        }

        #endregion GetSets
    }
}