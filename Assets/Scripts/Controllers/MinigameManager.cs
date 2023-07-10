using Scripts.Games;
using Scripts.Models;
using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Scripts.Controllers
{
    /// <summary>
    /// Manages matching, spawning and deletion of game assets.
    /// </summary>
    public class MinigameManager : MonoBehaviour
    {
        #region Serialized Fields

        [SerializeField] private Settings settings;
        [SerializeField] private Transform spawnLeft, spawnRight, spawnCenter;

        #endregion

        #region Fields

        public static event Action<int> OnUpdateUIScore;

        private List<Minigame> _mixGames, _soloGames;
        private Queue<Minigame> _previous;
        private Genre _otherGenre;
        private KeyMap _otherKeys;
        private Transform _parent;
        private const int MAX_QUE = 5;
        private int _winCounter = MAX_QUE;

        #endregion

        #region Built-Ins

        void Awake()
        {
            _previous = new();
            ResetGames();
        }

        void Start()
        {
            BaseGame.OnLose += LoseCondition;
            BaseGame.OnWin += RemoveGame;
            BaseGame.OnUpdateDifficulty += UpdateDifficulty;
            BaseGame.OnScoreUpdate += UpdateScore;

            if (settings.SelectedGame != null)
            {
                LoadGame(settings.SelectedGame, settings.SelectedGame.KeysRight, spawnCenter);
                return;
            }

            SpawnSides();
        }

        void Update()
        {
            if (_mixGames.Count <= 1 || _soloGames.Count <= 1)
                ResetGames();

            if (_winCounter <= 0)
            {
                if (spawnLeft.childCount != 0 || spawnRight.childCount != 0 || spawnCenter.childCount != 0) return;
                List<Minigame> gameList = new(_soloGames);
                Minigame bossGame = gameList[Random.Range(0, gameList.Count)];
                LoadGame(bossGame, bossGame.KeysRight, spawnCenter);
            }
        }

        private void OnDisable()
        {
            BaseGame.OnLose -= LoseCondition;
            BaseGame.OnWin -= RemoveGame;
            BaseGame.OnUpdateDifficulty -= UpdateDifficulty;
            BaseGame.OnScoreUpdate -= UpdateScore;
        }

        #endregion

        #region Instance Management

        private void SpawnSides()
        {
            _parent = spawnLeft;
            PickGame(new List<Minigame>(_mixGames));
            _parent = spawnRight;
            PickGame(new List<Minigame>(_mixGames));
        }

        /// <summary>
        /// Picks a random game from the BaseGame list.
        /// </summary>
        /// <param name="gameList">A game list to pick from.</param>
        /// <returns>The picked game asset.</returns>
        private void PickGame(List<Minigame> gameList)
        {
            Minigame thisGame = GetNext(gameList);

            // Find the fitting game
            while (CheckGenre(thisGame.Genre, _otherGenre) && CheckKeys(thisGame.SelectedKeys, _otherKeys))
            {
                gameList.Remove(thisGame);
                thisGame = GetNext(gameList);
            }

            LoadGame(thisGame, thisGame.SelectedKeys, _parent);
        }

        /// <summary>
        /// Gets the next game in a list to set checking properties.
        /// </summary>
        /// <param name="gameList">A game list to pick from.</param>
        /// <returns>The selected game.</returns>
        private Minigame GetNext(List<Minigame> gameList)
        {
            Minigame thisGame = gameList[Random.Range(0, gameList.Count)];
            BaseGame other = null;

            if (_parent == spawnLeft)
            {
                thisGame.SelectedKeys = thisGame.KeysLeft;
                if (spawnRight.childCount == 0) return thisGame;
                other = spawnRight.GetChild(0).GetComponent<BaseGame>();
            }

            if (_parent == spawnRight)
            {
                thisGame.SelectedKeys = thisGame.KeysRight;
                if (spawnLeft.childCount == 0) return thisGame;
                other = spawnLeft.GetChild(0).GetComponent<BaseGame>();
            }

            if (other == null) return thisGame;

            _otherGenre = other.Genre;
            _otherKeys = other.Keys;

            return thisGame;
        }

        /// <summary>
        /// Instantiates a chosen game asset with all settings.
        /// </summary>
        /// <param name="game">The game asset to load from.</param>
        /// <param name="keys">The keymap to use.</param>
        /// <param name="parent">The parent to load it into.</param>
        private void LoadGame(Minigame game, KeyMap keys, Transform parent)
        {
            if (game == null) return;

            _mixGames.Remove(game);
            if (_previous.Count == MAX_QUE) _mixGames.Add(_previous.Dequeue());
            _previous.Enqueue(game);

            game.Prefab.SetActive(false);
            GameObject obj = Instantiate(game.Prefab, parent);
            BaseGame baseGame = obj.GetComponent<BaseGame>();
            Difficulty difficulty = settings.BaseDifficulty;

            if (settings.BaseDifficulty == Difficulty.VARYING)
            {
                difficulty = game.Difficulty;
            }

            baseGame.SetUp(difficulty, keys, parent.GetComponent<RectTransform>().rect);
            obj.SetActive(true);
        }

        /// <summary>
        /// Removes the game instance from the scene.
        /// Messy code...
        /// </summary>
        private void RemoveGame(GameObject game)
        {
            if (settings.SelectedGame != null) return;

            _parent = game.transform.parent;

            if (_parent == spawnCenter)
            {
                RemoveAllGames();
                SpawnSides();
                return;
            }

            Destroy(game);
            if (_winCounter > 0)
                PickGame(new List<Minigame>(_mixGames));
        }

        /// <summary>
        /// Removes all game instances from the scene.
        /// </summary>
        private void RemoveAllGames()
        {
            if (spawnLeft.childCount != 0)
                Destroy(spawnLeft.GetChild(0).gameObject);

            if (spawnRight.childCount != 0)
                Destroy(spawnRight.GetChild(0).gameObject);

            if (spawnCenter.childCount != 0)
                Destroy(spawnCenter.GetChild(0).gameObject);

            _winCounter = MAX_QUE;
        }

        /// <summary>
        /// Copies the lists of games from settings.
        /// </summary>
        private void ResetGames()
        {
            _mixGames = new List<Minigame>(settings.Games);
            _soloGames = new List<Minigame>(settings.SoloGames);
        }

        #endregion Instance Management

        #region Checks

        /// <summary>
        /// Checks if the two games may have an overlap in genres.
        /// </summary>
        /// <param name="game">The game to load.</param>
        /// <param name="other">The still running game on screen.</param>
        /// <returns>False if no overlaps are found.</returns>
        private bool CheckGenre(Genre game, Genre other)
        {
            return (game & other) != 0;
        }

        private bool CheckKeys(KeyMap game, KeyMap other)
        {
            if (other.Equals(default(KeyMap))) return false;

            foreach (Key key in game.All)
            {
                foreach (Key others in other.All)
                {
                    if (key == others) return true;
                }
            }

            return false;
        }

        #endregion Checks

        #region Game Mechanics

        public void WinCondition(GameObject game)
        {
            _winCounter--;
            RemoveGame(game);
        }

        public void LoseCondition(GameObject game)
        {
            settings.Lives--;
            _winCounter--;
            RemoveGame(game);
        }

        public void UpdateDifficulty(GameObject game, Difficulty difficulty)
        {
            List<Minigame> all = new();
            all.AddRange(settings.Games);
            all.AddRange(settings.SoloGames);

            Minigame found = all.Find(obj => game.name.Contains(obj.Prefab.name));

            if (found != null)
            {
                found.Difficulty = difficulty;
                return;
            }
        }
        public void UpdateScore(int change)
        {
            OnUpdateUIScore?.Invoke(change);
        }

        #endregion

        #region Gizmos

        private void OnDrawGizmos()
        {
            if (spawnLeft.gameObject.activeInHierarchy)
            {
                Gizmos.color = new Color(0, 1, 0, 0.2f);
                Gizmos.DrawCube(spawnLeft.position, spawnLeft.GetComponent<RectTransform>().rect.size);
            }
            if (spawnRight.gameObject.activeInHierarchy)
            {
                Gizmos.color = new Color(1, 0, 0, 0.2f);
                Gizmos.DrawCube(spawnRight.position, spawnRight.GetComponent<RectTransform>().rect.size);
            }
            if (spawnCenter.gameObject.activeInHierarchy)
            {
                Gizmos.color = new Color(1, 0, 1, 0.2f);
                Gizmos.DrawCube(spawnCenter.position, spawnCenter.GetComponent<RectTransform>().rect.size);
            }
        }

        #endregion Gizmos
    }
}
