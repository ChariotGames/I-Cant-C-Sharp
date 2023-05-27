using _Scripts.Games;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _Scripts
{
    public class GameController : MonoBehaviour
    {
        #region Serialized Fields

        [SerializeField] private Data gameVariables;
        [SerializeField] private List<GameAsset> games;
        [SerializeField] private GameObject[] spawnPoints;

        #endregion

        #region Fields

        private List<GameAsset> loadedGames = new();

        #endregion

        #region Built-Ins

        void Awake()
        {
            PickGame(new List<GameAsset>(games));
            PickGame(new List<GameAsset>(games));
        }

        #endregion

        #region Instance Management

        /// <summary>
        /// Picks a random game from the Game list.
        /// </summary>
        /// <param name="gameList">A game list to pick from.</param>
        private void PickGame(List<GameAsset> gameList)
        {
            gameList.RemoveAll(item => loadedGames.Contains(item));

            GameAsset game = gameList[Random.Range(0, gameList.Count)];

            // Remove games before loading a new one
            if (loadedGames.Count >= 2 || spawnPoints[^1].transform.childCount == 1)
            {
                RemoveGame(game.Orientation.HasFlag(Orientation.FULLSCREEN));
            }

            if (game.Orientation == Orientation.FULLSCREEN)
            {
                loadedGames.Add(LoadGame(game, SetParent(game.Orientation)));
                return;
            }

            // Find the fitting game
            while (game.Prefab == null || loadedGames.Count != 0 && !CheckFit(game))
            {
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
        private bool CheckFit(GameAsset game)
        {
            if (game.Prefab == null) return false;

            if (!CheckProperty((int)game.Orientation, (int)loadedGames[0].Orientation)) return false;

            if (CheckProperty((int)game.Keys, (int)loadedGames[0].Keys)) return false;

            if (CheckProperty((int)game.Genre, (int)loadedGames[0].Genre)) return false;

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
        private GameAsset LoadGame(GameAsset game, Transform parent)
        {
            Instantiate(game.Prefab, parent);
            Game prefab = game.Prefab.GetComponent<Game>();
            prefab.ID = game.AssetID;
            prefab.Difficulty = game.Difficulty;
            return game;
        }

        /// <summary>
        /// Finds a fitting spawn container according to game specs.
        /// </summary>
        /// <param name="orientation">The game's orientation settings</param>
        /// <returns>A Transform to contain the game.</returns>
        private Transform SetParent(Orientation orientation)
        {
            if (orientation.HasFlag(Orientation.FULLSCREEN) && spawnPoints[^1].transform.childCount == 0)
            {
                return spawnPoints[(int)Direction.CENTER].transform;
            }

            if (orientation.HasFlag(Orientation.HORIZONTAL))
            {
                for (int i = 0; i < spawnPoints.Length - 1; i += 2)
                {
                    Transform trans = spawnPoints[i].transform;
                    if (trans.childCount == 0) return trans;
                }
            }

            if (orientation.HasFlag(Orientation.VERTICAL))
            {
                for (int i = 1; i < spawnPoints.Length - 1; i += 2)
                {
                    Transform trans = spawnPoints[i].transform;
                    if (trans.childCount == 0) return trans;
                }
            }

            if (orientation.HasFlag(Orientation.QUARTER))
            {
                for (int i = 0; i < spawnPoints.Length - 1; i++)
                {
                    Transform trans = spawnPoints[i].transform;
                    if (trans.childCount == 0) return trans;
                }
            }

            return null;
        }

        /// <summary>
        /// Removes the game instance from the scene.
        /// </summary>
        /// <param name="removeAllFlag">Removes all games if true.</param>
        private void RemoveGame(bool removeAllFlag)
        {
            if (removeAllFlag)
            {
                foreach (GameAsset game in loadedGames)
                {
                    Destroy(GameObject.FindObjectOfType<GameObject>(game.Prefab));
                }
                loadedGames.Clear();
            }

            int index = Random.Range(0, loadedGames.Count);
            GameObject instance = GameObject.FindObjectOfType<GameObject>(loadedGames[index].Prefab);
            Destroy(instance);
            loadedGames.Remove(loadedGames[index]);
        }

        /// <summary>
        /// Resizes the game, to fit the current spawn position
        /// </summary>
        private void ResizeGame()
        {
            // TODO: implement
        }

        #endregion

        #region Game Mechanics

        private void WinCondition(AssetID id)
        {
            Debug.Log("Win from " + id);
        }

        private void LoseCondition(AssetID id)
        {
            Debug.Log("Lose from " + id);
            gameVariables.Lives--;
            Debug.Log(gameVariables.Lives);
            if(gameVariables.Lives <= 0)
            {
                //PickGame(new List<GameAsset>(games));
                gameObject.GetComponent<SceneChanger>().ChangeScene(0);
            }
        }

        private void SetDifficulty(AssetID id, Difficulty difficulty)
        {
            games.Find(obj => obj.AssetID == id).Difficulty = difficulty;
        }

        #endregion
    }
}
