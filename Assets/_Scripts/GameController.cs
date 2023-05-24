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

        private GameAsset[] loadedGames = new GameAsset[2];

        #endregion

        #region Built-Ins

        void Awake()
        {
            SetOriginIDs();

            loadedGames[0] = SpawnGame(new List<GameAsset>(games));
            loadedGames[1] = SpawnGame(new List<GameAsset>(games));
            //if (loadedGames[0] == null) loadedGames[0] = SpawnGame();
            //if (loadedGames[1] == null) loadedGames[1] = SpawnGame();
        }

        #endregion

        #region Instance Management

        /// <summary>
        /// Sets the Game Assets' origin IDs.
        /// Based on the Instance IDs of the Prefabs.
        /// </summary>
        private void SetOriginIDs()
        {
            foreach (GameAsset game in games)
            {
                game.Origin = game.Prefab.GetInstanceID();
            }
        }

        /// <summary>
        /// Spawns a random game from the Game list.
        /// </summary>
        /// <param name="gameList">A game list to pick from.</param>
        /// <returns>A GameAsset reference.</returns>
        private GameAsset SpawnGame(List<GameAsset> gameList)
        {
            if (loadedGames[0] != null) gameList.Remove(loadedGames[0]);
            if (loadedGames[1] != null) gameList.Remove(loadedGames[1]);

            GameAsset game = gameList[Random.Range(0, gameList.Count)];

            // TODO: Check for fullscreen?

            while (game.Prefab == null || AlreadySpawned(game.Origin) || SameOrientation(game.Orientation))
            {
                gameList.Remove(game);
                game = gameList[Random.Range(0, gameList.Count)];
            }

            Transform parent = SetParent(game.Orientation);

            while (parent == null)
            {
                parent = SetParent(game.Orientation);
            }

            Instantiate(game.Prefab, parent);

            return game;
        }

        /// <summary>
        /// Checks if picked game is already spawned.
        /// </summary>
        /// <param name="gameID">The game's ID</param>
        /// <returns>True or false if it exists.</returns>
        private bool AlreadySpawned(int gameID)
        {
            bool one = false;
            bool two = false;
            if (loadedGames[0] != null)
            {
                one = loadedGames[0].Origin == gameID;
            }

            if (loadedGames[1] != null)
            {
                two = loadedGames[1].Origin == gameID;
            }

            return one || two;
            //if ((loadedGames[0] != null && loadedGames[0].Origin == gameID) ||
            //    (loadedGames[1] != null && loadedGames[1].Origin == gameID)) return true;

            //return false;
        }

        /// <summary>
        /// Checks if a game would overlap with already spawned ones
        /// </summary>
        /// <param name="orientation">The game's orionatation flags.</param>
        /// <returns>True when overlap is found.</returns>
        private bool SameOrientation(Orientation orientation)
        {
            bool one = false;
            bool two = false;

            if (loadedGames[0] != null)
            {
                one = (loadedGames[0].Orientation & orientation) != orientation;
            }

            if (loadedGames[1] != null)
            {
                two = (loadedGames[1].Orientation & orientation) != orientation;
            }

            return one || two;
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
        private void RemoveGame()
        {
            // TODO: implement
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

        private void WinCondition(int origin)
        {
            Debug.Log("Win from " + games.Find(obj => obj.Origin == origin).Asset);
        }

        private void LoseCondition(int origin)
        {
            Debug.Log("Lose from " + games.Find(obj => obj.Origin == origin).Asset);
        }

        private void SetDifficulty(int origin, Difficulty difficulty)
        {
            // TODO: Implement
        }

        #endregion
    }
}
