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

        //[SerializeField] AudioSource WinSound;


        #endregion Serialized Fields

        #region Fields

        [SerializeField] private int remainingTiles = 0, ammountOfTiles = 0, lives = 3;
        private int gridHalf;
        private bool[,] grid;

        #endregion Fields

        #region Built-Ins / MonoBehaviours

        private void Awake()
        {
            grid = new bool[gridSize, gridSize];

            failsToLose = 3;
            if (Difficulty.EASY == difficulty)
            {
                failsToLose--;
            }
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
            switch (type)
            {
                case ElementType.CHECKPOINT:
                    remainingTiles--;

                    if (remainingTiles == 0)
                    {
                        base.Success();
                        //WinSound.Play();
                        Invoke(nameof(ResetGame), 1);
                    }
                    break;
                case ElementType.ENEMY:
                    Fail();
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
            for (int x = -gridHalf; x <= gridHalf; x++)
            {
                for (int y = -gridHalf; y <= gridHalf; y++)
                {
                    GameObject obj = Instantiate(tile, container);
                    obj.transform.Translate(x * space, y * space, 0);
                    obj.SetActive(true);
                    grid[x + gridHalf, y + gridHalf] = true;
                }
            }
            
            if (difficulty == Difficulty.MEDIUM) RemoveTiles(Random.Range(2, 4));
            if (difficulty == Difficulty.HARD) RemoveTiles(Random.Range(4, 6));
            SetPlayer();
        }

        private void RemoveTiles(int removeCount)
        {
            remainingTiles -= removeCount;
            while (removeCount > 0)
            {
                int x = Random.Range(0, gridSize);
                int y = Random.Range(0, gridSize);

                if (CheckDiagonals(x, y) && CheckVertical(x, y) && CheckHorizontal(x, y))
                {
                    int pos = y * gridSize + x;
                    GameObject obj = container.GetChild(pos).gameObject;
                    LoseTileField LTF = obj.GetComponent<LoseTileField>();
                    LTF.setInVisible();
                    LTF.visited = true;
                    LTF.type = ElementType.ENEMY;
                    grid[x, y] = false; 
                    removeCount--;
                }
            }
        }

        private bool CheckHorizontal(int x, int y)
        {
            bool result = false;
            if ((x - 1) >= 0) result |= grid[(x - 1), y];
            if ((x + 1) < gridSize) result |= grid[(x + 1), y];
            return result;
        }

        private bool CheckVertical(int x, int y)
        {
            bool result = false;
            if ((y - 1) >= 0) result |= grid[x, (y - 1)];
            if ((y + 1) < gridSize) result |= grid[x, (y + 1)];
            return result;
        }

        
        private bool CheckDiagonals(int x, int y)
        {
            bool result = true;
            if ((x - 1) >= 0 && (y - 1) >= 0) result &= grid[(x - 1), (y - 1)];
            if ((x + 1) < gridSize && (y - 1) >= 0) result &= grid[(x + 1), (y - 1)];
            if ((x - 1) >= 0 && (y + 1) < gridSize) result &= grid[(x - 1), (y + 1)];
            if ((x + 1) < gridSize && (y + 1) < gridSize) result &= grid[(x + 1), (y + 1)];
            return result;
        }

        private void SetPlayer()
        {
            int startTile = Random.Range(0, container.childCount);
            LoseTileField LTF = container.GetChild(startTile).gameObject.GetComponent<LoseTileField>();
            while (!LTF.isVisable)
            {
                startTile = Random.Range(0, container.childCount);
                LTF = container.GetChild(startTile).gameObject.GetComponent<LoseTileField>();
            }

            player.transform.localPosition = LTF.gameObject.transform.localPosition;
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