//using Scripts.Games;
//using Scripts.Models;
//using System.Collections.Generic;
//using TMPro;
//using UnityEngine;
//using System;
//using System.Collections;


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

        [SerializeField] private GameObject GameOverPanel;
        [SerializeField] private TMP_Text scoreCounter;
        [SerializeField] private TMP_Text timeCounter;
        #endregion

        #region Fields

        private Camera _mainCamera;
        private List<Minigame> _mixGames, _soloGames;
        private Queue<Minigame> _previous;
        private Minigame loadedLeft, loadedRight, currentGame, otherGame;
        private KeyMap keys, otherKeys;
        private Transform parent;
        private const int MAX_QUE = 5;
        private int loadedTimes = 0;

        private int score = 0;
        private float _time = 0;
        private bool timerOn;

        #endregion

        #region Built-Ins

        void Awake()
        {
            if (_mainCamera == null) _mainCamera = Camera.main;

            _mixGames = new List<Minigame>(settings.Games);
            _soloGames = new List<Minigame>(settings.SoloGames);
            _previous = new(MAX_QUE);
        }

        void Start()
        {
            BeginTimer();

            
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
            baseGame.Difficulty = game.Difficulty;
            baseGame.Keys = keys;
            baseGame.Bounds = SetBounds(parent.position, _soloGames.Contains(game));
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
                Destroy(spawnCenter.GetChild(0).gameObject, 1);
                LoadGame(settings.SelectedGame, settings.SelectedGame.KeysRight, spawnCenter);
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

        public void WinCondition(/*AssetID id,*/ GameObject game)
        {
            Debug.Log($"Win from {game.name}");
            score++;

            //TODO: check if score can get over 100? Maybe take different approach
            if(score <10)
            {
                scoreCounter.text = "00" + score.ToString();
            } else if(score <100)
            {
                scoreCounter.text = "0" + score.ToString();
            } else scoreCounter.text = score.ToString();
            
            //TODO: temporary
            RemoveGame(game);
        }

        public void LoseCondition(/*AssetID id,*/ GameObject game)
        {
            Debug.Log($"Lose from {game.name}");
            settings.Lives--;
            Debug.Log($"Lives left: {settings.Lives}");
            if (settings.Lives <= 0)
            {
                //TODO: temporary fix for RemoveGame()
                EndTimer();
                GameOverPanel.SetActive(true);
                //SceneChanger.ChangeScene(0);

            }
            RemoveGame(game);
        }

        public void SetDifficulty(/*AssetID id,*/ GameObject game, Difficulty difficulty)
        {
            settings.Games.Find(obj => /*obj.AssetID == id*/obj.Prefab == game).Difficulty = difficulty;
        }

        /// <summary>
        /// Sets the game's bounds, to fit the current spawn position.
        /// <param name="centerPoint">The point to set to.</param>
        /// <param name="isFullscreen">Only needed for fullscreen games.</param>
        /// </summary>
        private Bounds SetBounds(Vector3 centerPoint, bool isFullscreen)
        {
            Vector3 size = _mainCamera.ViewportToWorldPoint(new Vector3(1f, 1f, 0f)) - _mainCamera.ViewportToWorldPoint(Vector3.zero);

            if (isFullscreen) return new Bounds(_mainCamera.transform.position, size);

            return new Bounds(centerPoint, size);
        }

        private void BeginTimer()
        {
            timerOn = true;
            StartCoroutine(RunTimer());
            
        }

        private void EndTimer()
        {
            timerOn = false;
        }

        private IEnumerator RunTimer()
        {
            while(timerOn)
            {
                _time += Time.deltaTime;
                TimeSpan timePlaying = TimeSpan.FromSeconds(_time);
                timeCounter.text = timePlaying.ToString("mm':'ss");
                yield return null;
            }
        }

        #endregion
    }
}
