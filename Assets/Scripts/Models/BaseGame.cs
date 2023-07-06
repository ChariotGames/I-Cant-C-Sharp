using Scripts.Controllers;
using Scripts.Models;
using System;
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

        public static event Action<GameObject> OnWin, OnLose;
        public static event Action<GameObject, Difficulty> OnUpdateDifficulty;
        public static event Action<int> OnScoreUpdate;

        protected KeyMap keys;
        protected Rect playarea;

        #endregion Fields

        #region Methods

        public void SetUp(Difficulty difficulty, KeyMap keys, Rect area)
        {
            this.difficulty = difficulty;
            this.keys = keys;
            this.playarea = area;
        }

        protected void ScoreDown()
        {
            OnScoreUpdate?.Invoke(-(int)difficulty);
        }

        protected void ScoreUp()
        {
            OnScoreUpdate?.Invoke((int)difficulty);
        }

        /// <summary>
        /// Informs the BaseGame Controller, that the game triggered a win condition
        /// </summary>
        protected void Win()
        {
            OnWin?.Invoke(gameObject);
        }

        /// <summary>
        /// Informs the BaseGame Controller, that the game triggered a lose condition
        /// </summary>
        protected void Lose()
        {
            OnLose?.Invoke(gameObject);
        }

        /// <summary>
        /// Makes the current game easier next time it's played.
        /// </summary>
        protected void Easier()
        {
            OnUpdateDifficulty?.Invoke(gameObject, difficulty - 1);
        }

        /// <summary>
        /// Makes the current game harder next time it's played.
        /// </summary>
        protected void Harder()
        {
            OnUpdateDifficulty?.Invoke(gameObject, difficulty + 1);
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
        ///  The minigame's inner bounds, set by the manager.
        /// </summary>
        public Rect Playarea
        {
            get => playarea;
            set => playarea = value;
        }

        #endregion GetSets
    }
}