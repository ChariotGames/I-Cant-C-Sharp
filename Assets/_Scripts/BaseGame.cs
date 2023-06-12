using _Scripts.Controllers;
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
            manager.WinCondition(id);
        }

        /// <summary>
        /// Informs the BaseGame Controller, that the game triggered a lose condition
        /// </summary>
        protected void Lose()
        {
            manager.LoseCondition(id);
        }

        /// <summary>
        /// Makes the current game easier next time it's played.
        /// </summary>
        protected void Easier()
        {
            manager.SetDifficulty(id, currentDifficulty - 1);
        }

        /// <summary>
        /// Makes the current game harder next time it's played.
        /// </summary>
        protected void Harder()
        {
            manager.SetDifficulty(id, currentDifficulty + 1);
        }

        #endregion protected

        #endregion  Methods

        #region GetSets

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

        #endregion GetSets
    }
}