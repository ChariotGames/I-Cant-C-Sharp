using System;
using System.Collections.Generic;
using UnityEngine;

namespace _Scripts
{
    public class GameController : MonoBehaviour
    {
        [SerializeField] private Data gameVariables;
        [SerializeField] private List<Game> games;
        [SerializeField] private GameObject[] spawnPoints;
        private (int min, int max) spawnRange;

        void Awake()
        {
            spawnRange = (0, games.Count - 1);
            //SpawnGame();
        }

        private void SpawnGame()
        {
            Game game = games[UnityEngine.Random.Range(spawnRange.min, spawnRange.max)];

            while (AlreadySpawned(game.Prefab))
            {
                game = games[UnityEngine.Random.Range(spawnRange.min, spawnRange.max)];
            }

            int index = FindFreeSpawnPoint(game.Orientation);
            _ = spawnPoints[0].transform.childCount;
        }

        private bool AlreadySpawned(GameObject game)
        {
            for (int i = 0; i < spawnPoints.Length; i++)
            {
                if (spawnPoints[i].transform.GetChild(0).Equals(game)) return true;
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

        private int FindFreeSpawnPoint(Orientation orientation)
        {
            for (int i = 0; i < spawnPoints.Length; i++)
            {
                if (orientation.HasFlag(Orientation.HORIZONTAL))
                {

                }
            }
            // TODO: implement the correct code.
            return -1;
        }


    }
}
