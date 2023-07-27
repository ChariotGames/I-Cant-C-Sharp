//using System;
using UnityEngine;
using Scripts.Models;

namespace Scripts.Games
{
    public class LoseTile : BaseGame
    {
        #region Serialized Fields

        [Space]
        [Header("Game Specific Stuff")]
        [SerializeField] [Range(3, 5)] private int gridSize = 3;
        [SerializeField] [Range(1, 2)] private float space = 1.1f;
        [SerializeField] private GameObject tile;
        [SerializeField] private LoseTilePlayer player;
        [SerializeField] private Transform container;

        #endregion Serialized Fields

        #region Fields

        [SerializeField] private int remainingTiles = 0, ammountOfTiles = 0, lives = 3;
        #endregion Fields

        #region Built-Ins / MonoBehaviours

        private void Awake()
        {
            successesToWin = 3;
            failsToLose = 1;
            ammountOfTiles = gridSize * gridSize;
            player.Stick = _keys.One.Input;
        }

        void Start()
        {
            ResetGame();
        }

        #endregion Built-Ins / MonoBehaviours

        #region Game Mechanics / Methods



        public void PlayerTouched(ElementType type)
        {
            //ElementType type = obj.GetComponent<LoseTileField>().type;

            switch (type)
            {
                case ElementType.CHECKPOINT:
                    //Destroy(obj);
                    remainingTiles--;

                    if (remainingTiles == 0)
                    {
                        //successesToWin--;
                        Success();
                        ResetGame();

                        //if (successesToWin == 0)
                        //{
                        //    Debug.Log("you win");
                        //    base.Win();
                        //}
                    }
                    break;
                case ElementType.ENEMY:

                    //failsToLose--;
                    //if (failsToLose == 0)
                    //{
                    //    Debug.Log("you lose");
                    //    base.Lose();
                    //}

                    Fail();
                    ResetGame();

                    break;
                case ElementType.GOAL:
                    //if (remainingTiles == ammountOfTiles -1)
                    //{
                    //    base.Win();
                    //}
                    break;
            }
        }

        private void ClearGrid()
        {
            for (int i = 0; i < container.childCount; i++)
            {
                GameObject obj = container.GetChild(i).gameObject;
                Destroy(obj);
            }
        }

        #endregion Game Mechanics / Methods

        #region Overarching Methods / Helpers

        private void SetGrid()
        {
            int gridHalf = gridSize / 2;

            for (int x = -gridHalf; x <= gridHalf; x++)
            {
                for (int y = -gridHalf; y <= gridHalf; y++)
                {

                    GameObject obj = Instantiate(tile, container);
                    obj.transform.Translate(x * space, y * space, 0);
                    obj.SetActive(true);
                }
                //GameObject obj = Instantiate(tile, container);
                //obj.transform.SetParent(container, true);
            }

            int PlayerX = Random.Range(-gridHalf, +gridHalf);
            int PlayerY = Random.Range(-gridHalf, +gridHalf);
            player.transform.position = new Vector3(PlayerX * space, PlayerY * space, 0);
        }

        private void ResetGame()
        {
            remainingTiles = ammountOfTiles;
            if (container.childCount > 0) ClearGrid();
            SetGrid();
        }

        #endregion Overarching Methods / Helpers
    }
}