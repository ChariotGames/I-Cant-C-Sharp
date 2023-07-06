using System.Collections;
using TMPro;
using Scripts.Games;
using Scripts.Models;
using System.Collections.Generic;
using UnityEngine;
using System;
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
        [SerializeField] private UI_Manager uiManager;

        #endregion

        #region Fields

        private List<Minigame> _mixGames, _soloGames;
        private Queue<Minigame> _previous;
        private Minigame loadedLeft, loadedRight, currentGame, otherGame;
        private KeyMap keys, otherKeys;
        private Transform parent;
        private const int MAX_QUE = 5;
        private int loadedTimes = 0;

        //private int score = 0;
        //private float _time = 0;
        //private bool timerOn;

        #endregion

        #region Built-Ins

        void Awake()
        {
            _mixGames = new List<Minigame>(settings.Games);
            _soloGames = new List<Minigame>(settings.SoloGames);
            _previous = new(MAX_QUE);
        }

        void Start()
        {
            if (settings.SelectedGame != null)
            {
                LoadGame(settings.SelectedGame, settings.SelectedGame.KeysRight, spawnCenter);
                return;
            }

            loadedLeft = PickGame(new List<Minigame>(_mixGames));
            loadedRight = PickGame(new List<Minigame>(_mixGames));
        }


        #endregion

        #region Instance Management

        /// <summary>
        /// Picks a random game from the BaseGame list.
        /// </summary>
        /// <param name="gameList">A game list to pick from.</param>
        /// <returns>The picked game asset.</returns>
        private Minigame PickGame(List<Minigame> gameList)
        {
            GetNext(gameList);

            // Find the fitting game
            while (CheckGenre(currentGame, otherGame) && CheckKeys(keys, otherKeys))
            {
                if (gameList.Count == 0) return null;

                gameList.Remove(currentGame);
                GetNext(gameList);
            }

            if (LoadGame(currentGame, keys, parent)) return currentGame;

            return null;
        }

        /// <summary>
        /// Gets the next game in a list to set checking properties.
        /// </summary>
        /// <param name="gameList">A game list to pick from.</param>
        private void GetNext(List<Minigame> gameList)
        {
            currentGame = gameList[Random.Range(0, gameList.Count)];
            if (spawnLeft.childCount == 0)
            {
                parent = spawnLeft;
                keys = currentGame.KeysLeft;
                if (spawnRight.childCount != 0)
                {
                    otherGame = loadedRight;
                    otherKeys = spawnRight.GetChild(0).GetComponent<BaseGame>().Keys;
                }
            }
            else
            {
                parent = spawnRight;
                keys = currentGame.KeysRight;
                if (spawnLeft.childCount == 0)
                {
                    otherGame = loadedLeft;
                    otherKeys = spawnLeft.GetChild(0).GetComponent<BaseGame>().Keys;
                }
            }
        }

        /// <summary>
        /// Instantiates a chosen game asset with all settings.
        /// </summary>
        /// <param name="game">The game asset to load from.</param>
        /// <param name="keys">The keymap to use.</param>
        /// <param name="parent">The parent to load it into.</param>
        /// <returns>The successfully loaded game object.</returns>
        private bool LoadGame(Minigame game, KeyMap keys, Transform parent)
        {
            if (game == null) return false;

            _mixGames.Remove(game);
            if (_previous.Count == MAX_QUE) _mixGames.Add(_previous.Dequeue());
            _previous.Enqueue(game);

            game.Prefab.SetActive(false);
            GameObject obj = Instantiate(game.Prefab, parent);
            BaseGame baseGame = obj.GetComponent<BaseGame>();
            baseGame.SetUp(this, game.Difficulty, keys, parent.GetComponent<RectTransform>().rect);
            obj.SetActive(true);
            loadedTimes++;
            return true; // Successfully loaded
        }

        /// <summary>
        /// Removes the game instance from the scene.
        /// Messy code...
        /// </summary>
        private void RemoveGame(GameObject game)
        {
            if (settings.SelectedGame != null)
            {
                return;
            }
            
            if(loadedTimes == MAX_QUE)
            {
                RemoveAllGames();
                List<Minigame> gameList = new(_soloGames);
                Minigame bossGame = gameList[Random.Range(0, gameList.Count)];
                LoadGame(bossGame, bossGame.KeysLeft, spawnCenter);
                return;
            }

            if (spawnLeft.childCount != 0 && spawnLeft.GetChild(0).gameObject == game)
            {
                Destroy(spawnLeft.GetChild(0).gameObject);
                loadedLeft = null;
                loadedLeft = PickGame(new List<Minigame>(_mixGames));
                return;
            }

            if (spawnRight.childCount != 0 && spawnRight.GetChild(0).gameObject == game)
            {
                Destroy(spawnRight.GetChild(0).gameObject);
                loadedRight = null;
                loadedRight = PickGame(new List<Minigame>(_mixGames));
                return;
            }

            if (spawnCenter.childCount != 0 && spawnCenter.GetChild(0).gameObject == game)
            {
                Destroy(spawnCenter.GetChild(0).gameObject);
                loadedLeft = PickGame(new List<Minigame>(_mixGames));
                loadedRight = PickGame(new List<Minigame>(_mixGames));
                return;
            }
        }

        /// <summary>
        /// Removes all game instances from the scene.
        /// </summary>
        private void RemoveAllGames()
        {
            if (spawnLeft.childCount != 0)
            {
                Destroy(spawnLeft.GetChild(0).gameObject);
                loadedLeft = null;
            }

            if (spawnRight.childCount != 0)
            {
                Destroy(spawnRight.GetChild(0).gameObject);
                loadedRight = null;
            }

            if (spawnCenter.childCount != 0)
            {
                Destroy(spawnCenter.GetChild(0).gameObject);
            }
            loadedTimes = 0;
        }

        #endregion Instance Management

        #region Checks

        /// <summary>
        /// Checks if the two games may have an overlap in genres.
        /// </summary>
        /// <param name="game">The game to load.</param>
        /// <param name="other">The still running game on screen.</param>
        /// <returns>False if no overlaps are found.</returns>
        private bool CheckGenre(Minigame game, Minigame other)
        {
            if (other == null) return false;
            return (game.Genre & other.Genre) != 0;
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

        public void ScoreUpdate(int score)
        {
            uiManager.ScoreUpdate(score);
        }

        public void WinCondition(GameObject game)
        {
            Debug.Log($"Win from {game.name}");

            //score++;
            //scoreCounter.text = score.ToString("D3");

            RemoveGame(game);
        }

        public void LoseCondition(GameObject game)
        {
            Debug.Log($"Lose from {game.name}");
            settings.Lives--;
            Debug.Log($"Lives left: {settings.Lives}");
            RemoveGame(game);
        }

        public void SetDifficulty(GameObject game, Difficulty difficulty)
        {
            settings.Games.Find(obj => obj.Prefab == game).Difficulty = difficulty;
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
