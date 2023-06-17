using _Scripts.Games;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

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

        private Camera mainCamera;
        private List<Minigame> loadedGames = new();

        #endregion

        #region Built-Ins

        void Awake()
        {
            if (settings.SelectedGame == null || settings.Mode == Mode.ENDLESS)
            {
                PickGame(new List<Minigame>(settings.Games));
                PickGame(new List<Minigame>(settings.Games));
            }
            else
            {
                loadedGames.Add(LoadGame(settings.SelectedGame, SetParent(Orientation.FULLSCREEN)));
            }

            if (mainCamera) mainCamera = Camera.main;
        }

        #endregion

        #region Instance Management

        /// <summary>
        /// Picks a random game from the BaseGame list.
        /// </summary>
        /// <param name="gameList">A game list to pick from.</param>
        private void PickGame(List<Minigame> gameList)
        {
            // Remove games before loading a new one
            gameList.RemoveAll(item => loadedGames.Contains(item));

            Minigame game = gameList[Random.Range(0, gameList.Count)];

            if (loadedGames.Count >= 2 || containers.Center.childCount == 1)
            {
                RemoveGame(containers.Center.GetChild(0).gameObject);
            }

            if (game.Orientation == Orientation.FULLSCREEN || game.Complexity == Complexity.SOLO)
            {
                loadedGames.Add(LoadGame(game, SetParent(Orientation.FULLSCREEN)));
                return;
            }

            // Find the fitting game
            while (loadedGames.Count != 0 && !CheckFit(game))
            {
                if (gameList.Count == 0) break;

                gameList.Remove(game);
                game = gameList[Random.Range(0, gameList.Count)];
            }

            loadedGames.Add(LoadGame(game, SetParent(game.Orientation)));
        }

        /// <summary>
        /// Goes over each property of a game asset and checks if
        /// it would fit with the currently loaded games on screen.
        /// </summary>
        /// <param name="game">The game to check.</param>
        /// <returns>True if no overlaps are found.</returns>
        private bool CheckFit(Minigame game)
        {
            if (game.Prefab == null) return false;

            if (!CheckProperty((int)game.Orientation, (int)loadedGames[0].Orientation)) return false;

            if (CheckProperty((int)game.KeysFirst, (int)loadedGames[0].KeysFirst)) return false;

            if (!CheckProperty((int)game.Complexity, (int)loadedGames[0].Complexity)) return false;

            return true;
        }

        /// <summary>
        /// Checks enum property values by bitwise XOR comparison.
        /// </summary>
        /// <param name="one">Value of game one.</param>
        /// <param name="two">Value of game two.</param>
        /// <returns>True if a match was found.</returns>
        private bool CheckProperty(int one, int two)
        {
            return (one ^ two) == 0;
        }

        /// <summary>
        /// Instantiates a chosen game asset.
        /// </summary>
        /// <param name="game">The game asset to load from.</param>
        /// <param name="parent">The parent to load it into.</param>
        /// <returns>The successfully loaded game.</returns>
        private Minigame LoadGame(Minigame game, Transform parent)
        {
            GameObject obj = Instantiate(game.Prefab, parent);
            BaseGame prefab = obj.GetComponent<BaseGame>();
            prefab.ID = game.AssetID;
            prefab.Difficulty = game.Difficulty;
            //prefab.KeyMap = game.KeysFirst;
            return game;
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
                    loadedGames.Remove(miniGame);
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
            foreach (Minigame game in loadedGames)
            {
                Destroy(FindObjectOfType<GameObject>(game.Prefab));
            }
            loadedGames.Clear();
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
        private Transform SetParent(Orientation orientation)
        {
            if (orientation == Orientation.FULLSCREEN && containers.Center.childCount == 0)
            {
                return containers.Center;
            }

            if (orientation == Orientation.HORIZONTAL)
            {
                if (containers.Up.childCount == 0) return containers.Up;
                if (containers.Down.childCount == 0) return containers.Down;
            }

            if (orientation == Orientation.VERTICAL || orientation == Orientation.QUARTER)
            {
                if (containers.Left.childCount == 0) return containers.Left;
                if (containers.Right.childCount == 0) return containers.Right;
            }

            return null;
        }

        #endregion

        #region Game Mechanics

        public void WinCondition(AssetID id)
        {
            Debug.Log("Win from " + id);
            for (int i = 0; i < containers.All.Length; i++)
            {
                if (containers.All[i].childCount == 0) continue;

                GameObject obj = containers.All[i].GetChild(0).gameObject;

                if (obj.GetComponent<BaseGame>().ID == id) RemoveGame(obj);
            }
        }

        public void LoseCondition(AssetID id)
        {
            Debug.Log("Lose from " + id);
            settings.Lives--;
            Debug.Log(settings.Lives);
            if (settings.Lives <= 0)
            {
                //PickGame(new List<Minigame>(games));
                gameObject.GetComponent<SceneChanger>().ChangeScene(0);
            }
        }

        public void SetDifficulty(AssetID id, Difficulty difficulty)
        {
            settings.Games.Find(obj => obj.AssetID == id).Difficulty = difficulty;
        }

        #endregion
    }
}
