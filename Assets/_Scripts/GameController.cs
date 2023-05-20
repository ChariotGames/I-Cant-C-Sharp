using System;
using System.Collections.Generic;
using UnityEngine;
using _Scripts.Games;

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

        private int[] loadedGames = new int[2];

        #endregion

        void Awake()
        {
            SetOriginIDs();

            loadedGames[0] = SpawnGame();
            loadedGames[1] = SpawnGame();
        }

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
        /// <returns>An instance of a picked Prefab.</returns>
        private int SpawnGame()
        {

            GameAsset game = games[UnityEngine.Random.Range(0, games.Count)];

            while (AlreadySpawned(game.Origin))
            {
                game = games[UnityEngine.Random.Range(0, games.Count)];
            }

            Transform parent = SetParent(game.Orientation);

            while (parent == null)
            {
                parent = SetParent(game.Orientation);
            }

            Instantiate(game.Prefab, parent);

            return game.Origin;
        }

        /// <summary>
        /// Checks if picked game is already spawned.
        /// </summary>
        /// <param name="gameID">The game's ID</param>
        /// <returns>True or false if it exists.</returns>
        private bool AlreadySpawned(int gameID)
        {
            if (loadedGames[0] == gameID || loadedGames[1] == gameID) return true;

            return false;
        }

        /// <summary>
        /// Finds a fitting spawn container according to game specs.
        /// </summary>
        /// <param name="orientation">The game's orientation settings</param>
        /// <returns>A Transform to contain the game.</returns>
        private Transform SetParent(Orientation orientation)
        {
            if (orientation.HasFlag(Orientation.FULLSCREEN) && spawnPoints[^1].transform.childCount == 0){
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

            if (orientation.HasFlag(Orientation.VERTICAL)) {
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

        private void RemoveGame()
        {
            // TODO: implement
        }

        private void ResizeGame()
        {
            // TODO: implement
        }

        #endregion

        #region Game Mechanics

        private void WinCondition(int origin)
        {
            Debug.Log("Win from " + games.Find(obj => obj.Origin == origin).Name);
        }

        private void LoseCondition(int origin)
        {
            Debug.Log("Lose from " + games.Find(obj => obj.Origin == origin).Name);
        }

        #endregion
    }
}
