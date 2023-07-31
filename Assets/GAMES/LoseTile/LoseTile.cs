//using System;
using UnityEngine;
using Scripts.Models;
using System.Collections;

namespace Scripts.Games
{
    public class LoseTile : BaseGame
    {
        #region Serialized Fields

        [Space]
        [Header("Game Specific Stuff")]
        [SerializeField] [Range(3, 5)] private int gridSize = 3;
        [SerializeField] [Range(1, 2)] private float space = 1.1f;
        [SerializeField] private GameObject tile, dangerzone;
        [SerializeField] private LoseTilePlayer player;
        [SerializeField] private Transform container;

        #endregion Serialized Fields

        #region Fields

        [SerializeField] private int remainingTiles = 0, ammountOfTiles = 0, lives = 3;
        private int gridHalf;
        #endregion Fields

        #region Built-Ins / MonoBehaviours

        private void Awake()
        {
            //successesToWin = 3;
            //failsToLose = 1;
            ammountOfTiles = gridSize * gridSize;
            player.Stick = _keys.One.Input;
            gridHalf = gridSize / 2;
        }

        IEnumerator Start()
        {
            yield return StartCoroutine(AnimateInstruction());

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

        private protected override void SetDifficulty()
        {
            //switch (Difficulty)
            //{
            //    case Difficulty.EASY:


            //        break;
            //    case Difficulty.MEDIUM:


            //        break;
            //    case Difficulty.HARD:


            //        break;
            //    default:


            //        break;
            //}
            if (difficulty == Difficulty.MEDIUM)
            {
                
            }
        }


        private void SetGrid()
        {
            int count = ((int)difficulty);
            if (difficulty == Difficulty.HARD)
            {
                count = Random.Range(4, ((int)difficulty)) - 1;
            }
            if (difficulty == Difficulty.EASY) 
            {
                count = 0;
            }

            for (int x = -gridHalf; x <= gridHalf; x++)
            {
                for (int y = -gridHalf; y <= gridHalf; y++)
                {

                    GameObject obj = Instantiate(tile, container);
                    obj.transform.Translate(x * space, y * space, 0);
                    obj.SetActive(true);
                    int Chance = Random.Range(0, 2);
                    if (Chance == 1 && count > 0)
                    {
                        obj.GetComponent<LoseTileField>().setInVisible();
                        obj.GetComponent<LoseTileField>().visited = true;
                        obj.GetComponent<LoseTileField>().type = ElementType.ENEMY;

                        count--;
                    }

                }
                //GameObject obj = Instantiate(tile, container);
                //obj.transform.SetParent(container, true);
            }
            SetPlayer();
        }

        private void SetPlayer()
        {
            int PlayerX = Random.Range(-gridHalf, +gridHalf);
            int PlayerY = Random.Range(-gridHalf, +gridHalf);
            LoseTileField Tile = container.GetChild((PlayerY + gridHalf) * gridSize + (PlayerX + gridHalf)).GetComponent<LoseTileField>();
            while (!Tile.isVisable)
            {
                PlayerX = Random.Range(-gridHalf, +gridHalf);
                PlayerY = Random.Range(-gridHalf, +gridHalf);
                Tile = container.GetChild((PlayerY +gridHalf) * gridSize + (PlayerX + gridHalf)).GetComponent<LoseTileField>();
            }
            player.transform.position = new Vector3(PlayerX * space, PlayerY * space, 0);
        }

        private void ResetGame()
        {
            remainingTiles = ammountOfTiles;
            if (container.childCount > 0) ClearGrid();
            dangerzone.SetActive(true);
            player.gameObject.SetActive(true);
            SetGrid();
        }

        #endregion Overarching Methods / Helpers
    }
}