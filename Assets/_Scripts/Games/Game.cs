using UnityEngine;

namespace _Scripts.Games
{
    public abstract class Game : MonoBehaviour
    {
        #region Fields

        protected int origin;
        protected Difficulty currentDifficulty = Difficulty.LVL1;

        private (int min, int max) difficultyRange = ((int)Difficulty.LVL1, (int)Difficulty.LVL3);

        #endregion

        #region Methods

        #region protected

        /// <summary>
        /// Informs the Game Controller, that the game triggered a win condition
        /// </summary>
        protected void Win()
        {
            SendMessageUpwards("WinCondition", origin);
        }

        /// <summary>
        /// Informs the Game Controller, that the game triggered a lose condition
        /// </summary>
        protected void Lose()
        {
            SendMessageUpwards("WinCondition", origin);
        }

        /// <summary>
        /// Makes the current game easier next time it's played.
        /// </summary>
        protected void Easier()
        {
            SendMessageUpwards("SetDifficulty", new object[] { origin, Clamp(currentDifficulty - 1) });
        }

        /// <summary>
        /// Makes the current game harder next time it's played.
        /// </summary>
        protected void Harder()
        {
            SendMessageUpwards("SetDifficulty", new object[] { origin, Clamp(currentDifficulty + 1) });
        }

        #endregion

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

        #endregion

        #endregion

        #region GetSets

        public int Origin
        {
            get { return origin; }
            set { origin = value; }
        }

        public Difficulty Difficulty
        {
            get { return currentDifficulty; }
            set { currentDifficulty = value; }
        }

        #endregion
    }
}