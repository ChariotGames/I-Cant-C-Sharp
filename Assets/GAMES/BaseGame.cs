using Scripts.Models;
using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Animations;

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

        [Header("Game Values")]
        [SerializeField] protected Difficulty difficulty = Difficulty.EASY;
        [SerializeField] protected int successesToWin = 5, successesToLevelUp;
        [SerializeField] protected int failsToLose = 3;

        #endregion Serialized Fields

        #region Fields

        public static event Action<GameObject> OnWin, OnLose;
        public static event Action<GameObject, Difficulty> OnUpdateDifficulty;
        public static event Action<int> OnScoreUpdate;
        public static event Action<string, float> OnTimerUpdate;
        public static event Action<string> OnTimerStop;
        //public static event Action<(string side, int score, float timer, int toWin, int toLose)> OnSetVariables;
        public static event Action<Transform, AnimType, int, float, float> OnPlayAnimations;

        protected KeyMap _keys;
        protected Rect _playarea;
        protected SpawnSide _spawnSide;

        protected Genre _genre;
        protected int _successes, _fails;
        protected float _timer;
        
        private Transform _parent;
        private GameObject _instructionPrefab;
        private float _instructionSpeed;
        private int difficultyTracker;
        private bool willGetHarder = false;
        private bool isVarying = true;

        #endregion Fields

        #region Methods

        /// <summary>
        /// Called by the game manager to set relevant data.
        /// </summary>
        /// <param name="difficulty">The difficulty loaded with.</param>
        /// <param name="keys">The keymap used.</param>
        /// <param name="area">The rect area defining the playfield.</param>
        public void SetUp(Difficulty difficulty, KeyMap keys, Rect area, SpawnSide type, bool varying)
        {
            this.difficulty = difficulty;
            _keys = keys;
            _playarea = area;
            SpawnSide = type;
            _parent = transform.parent;
            _fails = failsToLose;
            isVarying = varying;
            difficultyTracker = successesToWin < successesToLevelUp ? successesToWin : successesToLevelUp;
        }

        /// <summary>
        /// Called to setup the Instruction text object.
        /// </summary>
        /// <param name="prefab">The Prefab itself.</param>
        /// <param name="message">The Message from the Minigame.</param>
        /// <param name="duration">How far up the object should move.</param>
        public void SetInstructions(GameObject prefab, string message, float duration)
        {
            _instructionPrefab = Instantiate(prefab, transform);
            _instructionPrefab.GetComponent<TMP_Text>().text = message;
            _instructionSpeed = duration;
        }

        /// <summary>
        /// Decreases the score by 1-3 = difficulty.
        /// </summary>
        protected void ScoreDown() =>
            ScoreDown(-(int)difficulty);

        /// <summary>
        /// Decreases the score by a given value.
        /// </summary>
        /// <param name="value">The value to decrease.</param>
        protected void ScoreDown(int value) =>
            ScoreUpdate(-Mathf.Abs(value));

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
            ScoreUpdate(value);

        /// <summary>
        /// The actual method setting the score!
        /// </summary>
        /// <param name="value">The value to increase or decrease.</param>
        private void ScoreUpdate(int value) =>
            OnScoreUpdate?.Invoke(value);

        /// <summary>
        /// Trigger this when you achieved a success.
        /// It counts and manages everything else.
        /// </summary>
        protected void Success() =>
            Success(_parent, (int)difficulty);

        /// <summary>
        /// Overload method.
        /// Trigger this when you achieved a success.
        /// It counts and manages everything else.
        /// <param name="parent">The place to play the animation.</param>
        /// </summary>
        protected void Success(Transform parent) =>
            Success(parent, (int)difficulty);

        /// <summary>
        /// Overload method.
        /// Trigger this when you achieved a success.
        /// It counts and manages everything else.
        /// </summary>
        /// <param name="score">Pass a different score.</param>
        protected void Success(int score) =>
            Success(_parent, score);

        /// <summary>
        /// Overload method.
        /// Trigger this when you achieved a success.
        /// It counts and manages everything else.
        /// </summary>
        /// <param name="parent">The place to play the animation.</param>
        /// <param name="score">Pass a different score.</param>
        protected void Success(Transform parent, int score)
        {
            _successes++;
            ScoreUp(score);
            StopTimer();
            AnimateSuccess(parent, _successes, successesToWin);
            
            difficultyTracker--;
            if (difficultyTracker <= 0)
            {
                difficultyTracker = successesToLevelUp;
                willGetHarder = true;
            }
            
            if (_successes >= successesToWin)
            {
                _successes = 0;
                _fails = failsToLose;
                if (willGetHarder)
                {
                    Harder();
                }
                willGetHarder = false;
                Win();
            }
        }

        /// <summary>
        /// Use this when you made a mistake.
        /// It counts and manages everything else.
        /// </summary>
        protected void Fail() => Fail(_parent, (int)difficulty);

        /// <summary>
        /// Overload Method.
        /// Use this when you made a mistake.
        /// It counts and manages everything else.
        /// </summary>
        /// <param name="score">The score to reduce on fail.</param>
        protected void Fail(int score) =>
            Fail(_parent, score);

        /// <summary>
        /// Overload Method.
        /// Use this when you made a mistake.
        /// It counts and manages everything else.
        /// </summary>
        /// <param name="parent">The place to play the animation.</param>
        protected void Fail(Transform parent) =>
            Fail(parent, (int)difficulty);

        /// <summary>
        /// Overload Method.
        /// Use this when you made a mistake.
        /// It counts and manages everything else.
        /// </summary>
        /// <param name="parent">The place to play the animation.</param>
        /// <param name="score">The score to reduce on fail.</param>
        protected void Fail(Transform parent, int score)
        {
            _fails--;
            //ScoreDown(score);
            StopTimer();
            AnimateFail(parent, _fails, failsToLose);
            
            difficultyTracker++;

            if (_fails <= 0)
            {
                _successes = 0;
                _fails = failsToLose;
                Easier();
                Lose();
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
        /// Overload method.
        /// Runs the Win animation at a given parent position.
        /// </summary>
        /// <param name="parent">The parent object to attatch and play the animation at.</param>
        /// <param name="successes">Current increasing count of successes achieved (or their equivalent in your game).</param>
        /// <param name="successesToWin">The max number of successes to win (or their equivalent in your game).</param>
        protected void AnimateSuccess(Transform parent, int successes, int successesToWin)
        {
            OnPlayAnimations?.Invoke(parent, AnimType.Win, (int)difficulty, (float)successes, (float)successesToWin);
        }

        /// <summary>
        /// Runs the Win animation.
        /// </summary>
        /// <param name="successes">Current increasing count of successes achieved (or their equivalent in your game).</param>
        /// <param name="successesToWin">The max number of successes to win (or their equivalent in your game).</param>
        protected void AnimateSuccess(int successes, int successesToWin)
        {
            AnimateSuccess(_parent, successes, successesToWin);
        }

        /// <summary>
        /// Overload method.
        /// Runs the Lose animation at a given parent position.
        /// </summary>
        /// <param name="parent">The parent object to attatch and play the animation at.</param>
        /// <param name="fails">Current decreasing count of fails left (or their equivalent in your game).</param>
        /// <param name="failsToLose">The max number of fails to lose (or their equivalent in your game).</param>
        protected void AnimateFail(Transform parent, int fails, int failsToLose)
        {
            OnPlayAnimations?.Invoke(parent, AnimType.Lose, (int)difficulty, (float)(failsToLose - fails), (float)failsToLose);
        }

        /// <summary>
        /// Runs the Lose animation.
        /// </summary>
        /// <param name="fails">Current decreasing count of fails left (or their equivalent in your game).</param>
        /// <param name="failsToLose">The max number of fails to lose (or their equivalent in your game).</param>
        protected void AnimateFail(int fails, int failsToLose)
        {
            AnimateFail(_parent, fails, failsToLose);
        }

        /// <summary>
        /// Makes the current game easier next time it's played.
        /// </summary>
        protected void Easier()
        {
            if(!isVarying) return; 
            OnUpdateDifficulty?.Invoke(gameObject, difficulty - 1);
            
            if (SpawnSide == SpawnSide.Side) return;
            
            Difficulty = --difficulty;
            SetDifficulty();
        }
            

        /// <summary>
        /// Makes the current game harder next time it's played.
        /// </summary>
        protected void Harder()
        {
            if(!isVarying) return; 
            OnUpdateDifficulty?.Invoke(gameObject, difficulty + 1);
            
            if (SpawnSide == SpawnSide.Side) return;
            
            Difficulty = ++difficulty;
            SetDifficulty();
        }

        /// <summary>
        /// Override Method, you need to override if you check Difficulty in your Game ONCE.
        /// </summary>
        private protected virtual void SetDifficulty()
        {
            
        }
            

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
            
            OnTimerUpdate?.Invoke(transform.parent.name, time);
        }

        /// <summary>
        /// Stops the currently displayed timer.
        /// </summary>
        protected void StopTimer()
        {
            OnTimerStop?.Invoke(transform.parent.name);
        }

        /// <summary>
        /// Animates the inital game instruction.
        /// </summary>
        protected IEnumerator AnimateInstruction()
        {
            yield return StartCoroutine(AnimateInstruction(_instructionPrefab, _instructionSpeed));
        }

        /// <summary>
        /// Animates the inital game instruction in a GameObject.
        /// </summary>
        /// <param name="container">The TMP_Text to animate.</param>
        protected IEnumerator AnimateInstruction(GameObject container)
        {
            yield return StartCoroutine(AnimateInstruction(container, _instructionSpeed));
        }

        /// <summary>
        /// Animates the inital game instruction in a GameObject.
        /// </summary>
        /// <param name="distance">How far to move.</param>
        protected IEnumerator AnimateInstruction(float distance)
        {
            yield return StartCoroutine(AnimateInstruction(_instructionPrefab, distance));
        }

        /// <summary>
        /// Does the actual animation.
        /// </summary>
        /// <param name="container">The GameObject holding the TMP_Text.</param>
        /// <param name="distance">How far to move.</param>
        /// <returns></returns>
        protected IEnumerator AnimateInstruction(GameObject container, float distance)
        {
            GameObject obj = Instantiate(container, transform.position, Quaternion.identity, transform);
            obj.SetActive(true);
            TMP_Text tmp = obj.GetComponent<TMP_Text>();

            float offset = 0;
            while (offset < distance)
            {
                float delta = Time.deltaTime * _instructionSpeed;
                obj.transform.Translate(0, delta, 0, Space.Self);
                offset += delta;
                yield return new WaitForSeconds(Time.deltaTime);
            }

            float countdown = 3.0f;
            tmp.text = countdown.ToString();
            yield return new WaitForSeconds(Time.deltaTime);
            while (0 < countdown)
            {
                countdown -= Time.deltaTime;
                tmp.text = ((int)countdown+1).ToString();
                yield return new WaitForSeconds(Time.deltaTime);
            }
            obj.SetActive(false);
        }

        #endregion  Methods

        #region GetSets

        /// <summary>
        /// The game's current difficulty.
        /// </summary>
        public Difficulty Difficulty
        {
            get => difficulty;
            set => difficulty = (Difficulty)Mathf.Clamp((int)value, (int)Difficulty.EASY, (int)Difficulty.HARD);
        }

        /// <summary>
        /// The game's current spawn side.
        /// </summary>
        public SpawnSide SpawnSide
        {
            get => _spawnSide;
            set => _spawnSide = value;
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