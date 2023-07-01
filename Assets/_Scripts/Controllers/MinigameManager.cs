using _Scripts.Games;
using _Scripts.Models;
using System.Collections.Generic;
using UnityEngine;

namespace _Scripts.Controllers
{
    /// <summary>
    /// Manages matching, spawning and deletion of game assets.
    /// </summary>
    public class MinigameManager : MonoBehaviour
    {
        #region Serialized Fields

        [SerializeField] private Settings settings;
        [SerializeField] private SpawnPoints containers;

        #endregion

        #region Fields

        private Camera _mainCamera;
        private List<Minigame> _mixGames, _soloGames, _loaded;
        private Queue<Minigame> _previous;
        private const int MAX_LOADED = 2, MAX_QUE = 5;


        private int totalScore = 0;
        #endregion

        #region Built-Ins

        void Awake()
        {
            if (_mainCamera == null) _mainCamera = Camera.main;

            _mixGames = new List<Minigame>(settings.Games);
            _soloGames = new List<Minigame>(settings.SoloGames);
            _loaded = new(MAX_LOADED);
            _previous = new(MAX_QUE);
        }

        void Start()
        {
            if (LoadGame(settings.SelectedGame, containers.Center)) return;

            while (_loaded.Count != MAX_LOADED)
            {
                PickGame(new List<Minigame>(_mixGames));
            }
        }

        #endregion

        #region Instance Management

        /// <summary>
        /// Picks a random game from the BaseGame list.
        /// </summary>
        /// <param name="gameList">A game list to pick from.</param>
        private void PickGame(List<Minigame> gameList)
        {
            Minigame game = gameList[Random.Range(0, gameList.Count)];

            Transform load = null;
            foreach (Transform container in containers.All)
            {
                if (container.childCount == 0) continue;

                load = container;
                break;
            }

            // Find the fitting game
            while (!CheckFit(game, load))
            {
                if (gameList.Count == 0) return;

                gameList.Remove(game);
                game = gameList[Random.Range(0, gameList.Count)];
            }

            //LoadGame(game, SetParent(game.Orientation));
        }

        /// <summary>
        /// Instantiates a chosen game asset.
        /// </summary>
        /// <param name="game">The game asset to load from.</param>
        /// <param name="parent">The parent to load it into.</param>
        /// <returns>The successfully loaded game object.</returns>
        private bool LoadGame(Minigame game, Transform parent)
        {
            if (game == null) return false;

            _loaded.Add(game);

            _mixGames.Remove(game);
            if (_previous.Count == MAX_QUE) _mixGames.Add(_previous.Dequeue());
            _previous.Enqueue(game);

            GameObject obj = Instantiate(game.Prefab, parent);
            BaseGame prefab = obj.GetComponent<BaseGame>();
            prefab.ID = game.AssetID;
            prefab.Difficulty = game.Difficulty;
            //prefab.KeyMap = game.mainKeys;
            return true;
        }

        /// <summary>
        /// Removes the game instance from the scene.
        /// </summary>
        private void RemoveGame(GameObject game)
        {
            GameObject instance = FindObjectOfType<GameObject>(game);
            foreach (Minigame miniGame in settings.Games)
            {
                if (miniGame.Prefab == game)
                {
                    //loaded.Remove(miniGame);
                    break;
                }
            }
            Destroy(instance);
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

        /// <summary>
        /// Finds a fitting spawn container according to game specs.
        /// </summary>
        /// <param name="orientation">The game's orientation settings</param>
        /// <returns>A Transform to contain the game.</returns>
        //private Transform SetParent(Orientation orientation)
        //{
        //    if (orientation == Orientation.FULLSCREEN) return containers.Center;

        //    if (orientation == Orientation.HORIZONTAL)
        //    {
        //        if (containers.Up.childCount == 0) return containers.Up;
        //        if (containers.Down.childCount == 0) return containers.Down;
        //    }

        //    if (orientation == Orientation.VERTICAL || orientation == Orientation.ANY)
        //    {
        //        if (containers.Left.childCount == 0) return containers.Left;
        //        if (containers.Right.childCount == 0) return containers.Right;
        //    }

        //    return null;
        //}

        /// <summary>
        /// Sets the game's main keyMap depending on the set orientation.
        /// </summary>
        /// <param name="orientation">The game's orientation setting.</param>
        /// <returns>Main or auxilary key map.</returns>
        private KeyMap SetKeyMap(Minigame game)
        {
            //if (orientation == Orientation.FULLSCREEN) return containers.Center;

            //if (orientation == Orientation.HORIZONTAL)
            //{
            //    if (containers.Up.childCount == 0) return containers.Up;
            //    if (containers.Down.childCount == 0) return containers.Down;
            //}

            //if (orientation == Orientation.VERTICAL || orientation == Orientation.ANY)
            //{
            //    if (containers.Left.childCount == 0) return containers.Left;
            //    if (containers.Right.childCount == 0) return containers.Right;
            //}

            return new KeyMap();
        }

        #endregion Instance Management

        #region Checks

        /// <summary>
        /// Goes over each property of a game asset and checks if
        /// it would fit with the currently loaded games on screen.
        /// </summary>
        /// <param name="game">The game to check.</param>
        /// <param name="load">The occupied position to compare against.</param>
        /// <returns>True if no overlaps are found.</returns>
        private bool CheckFit(Minigame game, Transform load)
        {
            if (game.Prefab == null) return false;

            if (load == null) return true;

            //if (!CheckOrientation(game.Orientation, load)) return false;
            //if (!CheckGenre(game, load)) return false;
            if (!CheckKeys(game, load.GetChild(0).gameObject)) return false;

            return true;
        }

        //private bool CheckOrientation(Orientation orientation, Transform load)
        //{
        //    if (orientation == Orientation.ANY) return true;

        //    if (orientation == Orientation.HORIZONTAL &&
        //       (load == containers.Up || load == containers.Down)) return true;

        //    if (orientation == Orientation.VERTICAL &&
        //       (load == containers.Left || load == containers.Right)) return true;

        //    return false;
        //}

        private bool CheckOrientation(Minigame game, Minigame load)
        {
            //if (game.Orientation != Orientation.ANY ||
            //    load.Orientation != Orientation.ANY) return false;

            //if (((int)game.Orientation ^ (int)load.Orientation) != 0) return false;

            return true;
        }

        private bool CheckGenre(Minigame game, Minigame load)
        {
            throw new System.NotImplementedException();
        }

        private bool CheckKeys(Minigame game, GameObject load)
        {
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

            return true;
        }

        #endregion Checks

        #region Game Mechanics

        public void WinCondition(AssetID id, GameObject game)
        {
            Debug.Log($"Win from {id}");
            
            totalScore++;
            Debug.Log(totalScore);
            //for (int i = 0; i < containers.All.Length; i++)
            //{
            //    if (containers.All[i].childCount == 0) continue;

            //    GameObject obj = containers.All[i].GetChild(0).gameObject;

            //    if (obj.GetComponent<BaseGame>().ID == id) RemoveGame(obj);
            //}



            //TODO instantiate text mesh pro with score

            RemoveGame(game);
        }

        public void LoseCondition(AssetID id, GameObject game)
        {
            Debug.Log($"Lose from {id}");
            settings.Lives--;
            Debug.Log($"Lives left: {settings.Lives}");
            if (settings.Lives <= 0)
            {
                //PickGame(new List<Minigame>(games));
                //gameObject.GetComponent<SceneChanger>().ChangeScene(0);
                SceneChanger.ChangeScene(0);
            }
        }

        public void SetDifficulty(AssetID id, Difficulty difficulty)
        {
            settings.Games.Find(obj => obj.AssetID == id).Difficulty = difficulty;
        }

        #endregion
    }
}
