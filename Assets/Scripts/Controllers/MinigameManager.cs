using Scripts.Games;
using Scripts.Models;
using System.Collections.Generic;
using UnityEngine;

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

        private Camera _mainCamera;
        private List<Minigame> _mixGames, _soloGames;
        private Queue<Minigame> _previous;
        private Minigame loadedLeft, loadedRight, currentGame, otherGame;
        private KeyMap keys, otherKeys;
        private Transform parent;
        private const int MAX_QUE = 5;

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
            if (settings.SelectedGame != null)
            {
                LoadGame(settings.SelectedGame, settings.SelectedGame.KeysLeft, spawnCenter);
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

            GameObject obj = Instantiate(game.Prefab, parent);
            BaseGame prefab = obj.GetComponent<BaseGame>();
            //prefab.ID = game.AssetID;
            prefab.Difficulty = game.Difficulty;
            prefab.Keys = keys;

            return true; // Successfully loaded
        }

        /// <summary>
        /// Removes the game instance from the scene.
        /// </summary>
        private void RemoveGame(GameObject game)
        {
            /*GameObject instance = FindObjectOfType<GameObject>(game);
            foreach (Minigame miniGame in settings.Games)
            {
                if (miniGame.Prefab == game)
                {
                    //loaded.Remove(miniGame);
                    break;
                }
            }*/

            if (spawnLeft.GetChild(0).gameObject == game)
            {
                Destroy(spawnLeft.GetChild(0).gameObject);
                loadedLeft = null;
                loadedLeft = PickGame(new List<Minigame>(_mixGames));
                return;
            }

            if (spawnRight.GetChild(0).gameObject == game)
            {
                Destroy(spawnRight.GetChild(0).gameObject);
                loadedRight = null;
                loadedRight = PickGame(new List<Minigame>(_mixGames));
                return;
            }

            if (spawnCenter.GetChild(0).gameObject == game)
            {
                Destroy(spawnCenter.GetChild(0).gameObject);
                loadedLeft = PickGame(new List<Minigame>(_mixGames));
                loadedRight = PickGame(new List<Minigame>(_mixGames));
                return;
            }
        }

        /// <summary>
        /// Removes all game instance from the scene.
        /// </summary>
        private void RemoveAllGames()
        {
            //foreach (Minigame game in loaded)
            //{
            //    Destroy(FindObjectOfType<GameObject>(game.Prefab));
            //}
            //loaded.Clear();
        }

        /// <summary>
        /// Resizes the game, to fit the current spawn position
        /// </summary>
        private void ResizeGame()
        {
            // TODO: implement
        }

        #endregion Instance Management

        #region Checks

        /// <summary>
        /// Checks if the two games may have an overlap in genres.
        /// </summary>
        /// <param name="game">The game to load.</param>
        /// <param name="other">The still running game on screen.</param>
        /// <returns></returns>
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
            //InputActionReference[] keys = load.GetComponent<BaseGame>().Keys.All;

            //foreach(InputActionReference key in keys)
            //{
            //    if (key == null) continue;

            //    foreach (InputActionReference button in game.KeysMain.All)
            //    {
            //        if (button == null) continue;

            //        if (key == button) return false;
            //    }

            //    foreach (InputActionReference button in game.KeysAux.All)
            //    {
            //        if (button == null) continue;

            //        if (key == button) return false;
            //    }
            //}

            return false;
        }

        #endregion Checks

        #region Game Mechanics

        public void WinCondition(/*AssetID id,*/ GameObject game)
        {
            Debug.Log($"Win from {game.name}");
            //for (int i = 0; i < containers.All.Length; i++)
            //{
            //    if (containers.All[i].childCount == 0) continue;

            //    GameObject obj = containers.All[i].GetChild(0).gameObject;

            //    if (obj.GetComponent<BaseGame>().ID == id) RemoveGame(obj);
            //}
            RemoveGame(game);
        }

        public void LoseCondition(/*AssetID id,*/ GameObject game)
        {
            Debug.Log($"Lose from {game.name}");
            settings.Lives--;
            Debug.Log($"Lives left: {settings.Lives}");
            RemoveGame(game);
            if (settings.Lives <= 0)
            {
                gameObject.GetComponent<SceneChanger>().ChangeScene(0);
            }
        }

        public void SetDifficulty(/*AssetID id,*/ GameObject game, Difficulty difficulty)
        {
            settings.Games.Find(obj => /*obj.AssetID == id*/obj.Prefab == game).Difficulty = difficulty;
        }

        #endregion
    }
}
