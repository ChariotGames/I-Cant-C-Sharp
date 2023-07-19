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
        [SerializeField] protected int successesToWin = 5;
        [SerializeField] protected int failsToLose = 3;

        #endregion Serialized Fields

        #region Fields

        public static event Action<GameObject> OnWin, OnLose;
        public static event Action<GameObject, Difficulty> OnUpdateDifficulty;
        public static event Action<int> OnScoreUpdate;
        //public static event Action<(string side, int score, float timer, int toWin, int toLose)> OnSetVariables;
        public static event Action<string, AnimType> OnPlayAnimations;

        protected KeyMap _keys;
        protected Rect _playarea;
        protected int _successes, _fails;
        protected Genre _genre;

        #endregion Fields

        #region Methods

        /// <summary>
        /// Called by the game manager to set relevant data.
        /// </summary>
        /// <param name="difficulty">The difficulty loaded with.</param>
        /// <param name="keys">The keymap used.</param>
        /// <param name="area">The rect area defining the playfield.</param>
        public void SetUp(Difficulty difficulty, KeyMap keys, Rect area)
        {
            this.difficulty = difficulty;
            _keys = keys;
            _playarea = area;
        }

        /// <summary>
        /// Decreases the score by 1-3 = difficulty.
        /// </summary>
        protected void ScoreDown() =>
            ScoreDown((int)difficulty);

        /// <summary>
        /// Decreases the score by a given value.
        /// </summary>
        /// <param name="value">The value to decrease.</param>
        protected void ScoreDown(int value) =>
            OnScoreUpdate?.Invoke(-Mathf.Abs(value));

        /// <summary>
        /// Increases the score by 1-3 = difficulty.
        /// </summary>
        protected void ScoreUp() =>
            ScoreUp((int)difficulty);

        /// <summary>
        /// Increases the score by a given value.
        /// </summary>
        /// <param name="value">The value to increase.</param>
        protected void ScoreUp(int value) =>
            OnScoreUpdate?.Invoke(Mathf.Abs(value));

        /// <summary>
        /// Trigger ths when you achieved a success.
        /// It counts and manages everything else.
        /// </summary>
        protected void Success() =>
            Success((int)difficulty);

        /// <summary>
        /// Overload method.
        /// Trigger ths when you achieved a success.
        /// It counts and manages everything else.
        /// </summary>
        /// <param name="score">Pass a different score.</param>
        protected void Success(int score)
        {
            _successes++;
            ScoreUp(score);
            if (_successes >= successesToWin)
            {
                Win();
                return;
            }
            OnPlayAnimations?.Invoke(gameObject.transform.parent.name, AnimType.Win);
        }

        /// <summary>
        /// Use this when you made a mistake.
        /// It counts and manages everything else.
        /// </summary>
        protected void Fail() => Fail((int)difficulty);

        protected void Fail(int score)
        {
            _fails++;
            ScoreDown();
            if (_fails >= failsToLose)
            {
                Lose();
                return;
            }
            OnPlayAnimations?.Invoke(gameObject.transform.parent.name, AnimType.Lose);
        }

        /// <summary>
        /// Informs the BaseGame Controller, that the game triggered a win condition
        /// </summary>
        protected void Win() =>
            OnWin?.Invoke(gameObject);

        /// <summary>
        /// Informs the BaseGame Controller, that the game triggered a lose condition
        /// </summary>
        protected void Lose() =>
            OnLose?.Invoke(gameObject);

        /// <summary>
        /// Makes the current game easier next time it's played.
        /// </summary>
        protected void Easier() =>
            OnUpdateDifficulty?.Invoke(gameObject, difficulty - 1);

        /// <summary>
        /// Makes the current game harder next time it's played.
        /// </summary>
        protected void Harder() =>
            OnUpdateDifficulty?.Invoke(gameObject, difficulty + 1);

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
            get => _keys;
            set => _keys = value;
        }

        /// <summary>
        ///  The minigame's inner bounds, set by the manager.
        /// </summary>
        public Rect Playarea
        {
            get => _playarea;
            set => _playarea = value;
        }

        /// <summary>
        /// The minigame's genre;
        /// </summary>
        public Genre Genre
        {
            get => _genre;
            set => _genre = value;
        }

        #endregion GetSets
    }
}