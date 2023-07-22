using Scripts.Models;
using System;
using System.Collections;
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
        public static event Action<string, float> OnTimerUpdate;
        //public static event Action<(string side, int score, float timer, int toWin, int toLose)> OnSetVariables;
        public static event Action<string, AnimType, int, float, float> OnPlayAnimations;

        protected KeyMap _keys;
        protected Rect _playarea;
        protected Genre _genre;
        protected int _successes, _fails;
        protected float _timer;

        private string _parent;

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
            _parent = transform.parent.name;
        }

        /// <summary>
        /// Decreases the score by 1-3 = difficulty.
        /// </summary>
        protected void ScoreDown() =>
            ScoreUp(-(int)difficulty);

        /// <summary>
        /// Decreases the score by a given value.
        /// </summary>
        /// <param name="value">The value to decrease.</param>
        protected void ScoreDown(int value) =>
            ScoreUp(-Mathf.Abs(value));

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
            float from = 1.0f * _successes / successesToWin;
            _successes++;
            float to = 1.0f * _successes / successesToWin;
            ScoreUp(score);
            OnPlayAnimations?.Invoke(_parent, AnimType.Win, (int)difficulty, from, to);
            if (_successes >= successesToWin)
            {
                _successes = 0;
                Win();
                return;
            }
        }

        protected void AnimateWin(float successes, float successesToWin)
        {
            float from = (successes - 1) / successesToWin;
            float to = successes / successesToWin;
            OnPlayAnimations?.Invoke(_parent, AnimType.Win, (int)difficulty, from, to);
        }
        
        protected void AnimateLose(float fails, float failsToLose)
        {
            float from = (failsToLose - fails - 1) / failsToLose;
            float to = (failsToLose - fails) / failsToLose;
            OnPlayAnimations?.Invoke(_parent, AnimType.Lose, (int)difficulty, from, to); 
        }

        /// <summary>
        /// Use this when you made a mistake.
        /// It counts and manages everything else.
        /// </summary>
        protected void Fail() => Fail((int)difficulty);

        /// <summary>
        /// Overload Method.
        /// Use this when you made a mistake.
        /// It counts and manages everything else.
        /// </summary>
        /// <param name="score">The score to reduce on fail.</param>
        protected void Fail(int score)
        {
            float from = 1.0f * _fails / failsToLose;
            _fails++;
            float to = 1.0f * _fails / failsToLose;
            ScoreUp(score);
            OnPlayAnimations?.Invoke(_parent, AnimType.Lose, (int)difficulty, from, to);
            if (_fails >= failsToLose)
            {
                _fails = 0;
                Lose();
                return;
            }
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

        /// <summary>
        /// Runs the timer and updates the UI.
        /// </summary>
        protected void RunTimer() => RunTimer(_timer);

        /// <summary>
        /// Overload method.
        /// Runs the timer and updates the UI.
        /// </summary>
        /// <param name="time">The time of the timer.</param>
        protected void RunTimer(float time)
        {
            if (time <= 0) return;

            StartCoroutine(TimerAnimation(time));
        }

        protected void StopTimer()
        {
            // TODO : Implement TimerOff Action
        }

        /// <summary>
        /// Runs the timer as a coroutine.
        /// </summary>
        /// <param name="duration">The duration of the timer.</param>
        /// <returns></returns>
        private IEnumerator TimerAnimation(float duration)
        {
            float elapsedTime = 0f;
            while (elapsedTime < duration)
            {
                float progress = elapsedTime / duration;
                OnTimerUpdate?.Invoke(transform.parent.name, Mathf.Clamp01(progress));
                yield return null;
            }
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