using System;
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
            ammountOfTiles = gridSize * gridSize;
            remainingTiles = ammountOfTiles;
            SetGrid();
        }

        #endregion Built-Ins / MonoBehaviours

        #region GetSets / Properties



        #endregion GetSets / Properties

        #region Game Mechanics / Methods



        public void PlayerTouched(GameObject obj)
        {
            ElementType type = obj.GetComponent<LoseTileTile>().type;

            switch (type)
            {
                case ElementType.CHECKPOINT:
                    //Destroy(obj);
                    remainingTiles--;
                    if (remainingTiles == ammountOfTiles - 1)
                    {
                        base.Win();
                    }
                    break;
                case ElementType.ENEMY:
                    lives--;
                    if (lives == 0)
                    {
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

        #endregion Game Mechanics / Methods

        #region Overarching Methods / Helpers

        private void SetGrid()
        {
            for (int x = -gridSize/2; x <= gridSize/2; x++)

            {
                for (int y = -gridSize/2; y <= gridSize/2; y++)
                {
                    GameObject obj = Instantiate(tile, container);
                    obj.transform.Translate(x * space, y * space, 0);
                }
                //GameObject obj = Instantiate(tile, container);
                //obj.transform.SetParent(container, true); 
            }
        }

        

        #endregion Overarching Methods / Helpers
    }
}