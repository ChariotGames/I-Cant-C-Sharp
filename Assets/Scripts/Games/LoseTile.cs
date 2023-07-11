//using System;
using UnityEngine;
using Scripts.Models;

namespace Scripts.Games
{
    public class LoseTile : BaseGame
    {
        #region Serialized Fields

        [SerializeField][Range(3,5)] private int gridSize = 3;
        [SerializeField][Range(1,2)] private float space = 1;
        [SerializeField] private GameObject tile, player;
        [SerializeField] private Transform container;

        #endregion Serialized Fields

        #region Fields

        [SerializeField]private int remainingTiles = 0, ammountOfTiles = 0, lives = 3;
        #endregion Fields

        #region Built-Ins / MonoBehaviours

        void Start()
        {
            successesToWin = 3;
            failsToLose = 1;
            ammountOfTiles = gridSize * gridSize;
            
            SetGrid();

        }

        #endregion Built-Ins / MonoBehaviours

        #region GetSets / Properties



        #endregion GetSets / Properties

        #region Game Mechanics / Methods



        public void PlayerTouched(GameObject obj)
        {
            ElementType type = obj.GetComponent<LoseTileField>().type;

            switch (type)
            {
                case ElementType.CHECKPOINT:
                    //Destroy(obj);
                    remainingTiles--;
                    if (remainingTiles == 1)
                    {
                        successesToWin--;
                        clearGrid();
                        SetGrid();

                        //if (successesToWin == 0)
                        //{
                        //    Debug.Log("you win");
                        //    base.Win();
                        //}
                    }
                    break;
                case ElementType.ENEMY:
                   
                        failsToLose--;
                        if (failsToLose == 0)
                        {
                            Debug.Log("you lose");
                            base.Lose();
                        }

                        
                    
                    break;
                case ElementType.GOAL:
                    //if (remainingTiles == ammountOfTiles -1)
                    //{
                    //    base.Win();
                    //}
                    break;
            }
        }

        private void clearGrid()
        {
            for (int i = 0; i < container.childCount; i++)
            {
                Destroy(container.GetChild(i).gameObject);
            }
        }

        #endregion Game Mechanics / Methods

        #region Overarching Methods / Helpers

        private void SetGrid()
        {
            int gridHalf = gridSize / 2;

            remainingTiles = ammountOfTiles;
            
            for (int x = -gridHalf; x <= gridHalf; x++)
            {
                for (int y = -gridHalf; y <= gridHalf; y++)
                {
                    GameObject obj = Instantiate(tile, container);
                    obj.transform.Translate(x * space, y * space, 0);
                
                }
                //GameObject obj = Instantiate(tile, container);
                //obj.transform.SetParent(container, true);
                

            }
            int PlayerX = Random.Range(-gridHalf, +gridHalf);
            int PlayerY = Random.Range(-gridHalf, +gridHalf);
            player.transform.position = new Vector3(PlayerX * space, PlayerY * space, 0);
        }
        

        #endregion Overarching Methods / Helpers
    }
}