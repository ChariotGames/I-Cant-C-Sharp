using UnityEditor;
using UnityEngine;

namespace _Scripts.Games
{
    public abstract class BaseGame : MonoBehaviour
    {
        #region Serialized Fields

        [SerializeField] protected MinigameManager manager;
        [SerializeField] protected Difficulty currentDifficulty = Difficulty.EASY;

        #endregion Serialized Fields

        #region Fields

        protected AssetID id;
        
        private (int min, int max) difficultyRange = ((int)Difficulty.EASY, (int)Difficulty.HARD);

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
            manager.SetDifficulty(id, Clamp(currentDifficulty - 1));
        }

        /// <summary>
        /// Makes the current game harder next time it's played.
        /// </summary>
        protected void Harder()
        {
            manager.SetDifficulty(id, Clamp(currentDifficulty + 1));
        }

        #endregion protected

        #region private

        /// <summary>
        /// Clamps the given difficulty to the set limits.
        /// </summary>
        /// <param name="difficulty">The value to clamp.</param>
        /// <returns></returns>
        private Difficulty Clamp(Difficulty difficulty)
        {
            return (Difficulty)Mathf.Clamp((int)(difficulty), difficultyRange.min, difficultyRange.max);
        }

        #endregion private

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