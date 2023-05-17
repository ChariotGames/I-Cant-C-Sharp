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

        private GameObject game1, game2;
        private (int min, int max) spawnRange;

        #endregion

        void Awake()
        {
            /*spawnRange = (0, games.Count - 1);
            game1 = SpawnGame();
            game2 = SpawnGame();*/

            Debug.Log(games[4].Prefab.GetInstanceID());
            GameObject g1 = Instantiate(games[4].Prefab);
            GameObject g2 = Instantiate(games[4].Prefab);
            Debug.Log(g1.GetInstanceID());
            Debug.Log(g2.GetInstanceID());
        }

        private GameObject SpawnGame()
        {
            Direction direction = FindFreeSpawnPoint();
            if (direction == Direction.NONE)
            {
                // TODO: Add a proper exception
                Debug.Log("All Spawnpoints are busy");
                return null;
            }

            GameAsset game = games[UnityEngine.Random.Range(2, 4)];
            while (AlreadySpawned(game.Prefab) && CheckOrientation(direction, game.Orientation))
            {
                game = games[UnityEngine.Random.Range(spawnRange.min, spawnRange.max)];
            }

            return Instantiate(game.Prefab, spawnPoints[(int)direction].transform);
        }
        private Direction FindFreeSpawnPoint()
        {
            for (int i = 0; i < spawnPoints.Length; i++)
            {
                if (spawnPoints[i].transform.childCount == 0)
                {
                    return (Direction)i;
                }
            }

            return (Direction)(-1);
        }
        private bool AlreadySpawned(GameObject game)
        {
            if (game1 == null && game2 == null) return false;
            return game1 == game || game2 == game;
        }
        private bool CheckOrientation(Direction spawnPosition, Orientation orientation)
        {


            if (orientation == Orientation.FULLSCREEN && (spawnPosition == Direction.CENTER))
            {
                return true;
            }

            if (orientation == Orientation.HORIZONTAL && (spawnPosition == Direction.UP || spawnPosition == Direction.DOWN))
            {
                return true;
            }

            if (orientation == Orientation.VERTICAL && (spawnPosition == Direction.LEFT || spawnPosition == Direction.RIGHT))
            {
                return true;
            }

            if (orientation == Orientation.QUARTER)
            {
                return true;
            }

            return false;
        }


        private void WinCondition(GameObject game)
        {
            Debug.Log("Win from " + game.name);
        }

        private void LoseCondition(GameObject game)
        {
            Debug.Log("Lose from " + game.name);
        }

        


    }
}
