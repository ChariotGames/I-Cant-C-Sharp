using UnityEngine;

namespace Scripts.Games
{
    public class Exploooosions : BaseGame
    {
        [SerializeField] private GameObject border, bombBase, bombDonut, bombCross, bombContainer;

        public GameObject player;

        private bool active = false;
        private GameObject[] bombs;
        private int chance;
        private int timer;
        private int winCounter = 0;
        private int loseCounter = 0;

        private void Awake()
        {
            player.GetComponent<ExpPlayer>().stick = _keys.One.Input;
            Vector2[] borders = border.GetComponent<EdgeCollider2D>().points;
            borders[0] = new Vector2(_playarea.xMin, _playarea.yMin);
            borders[1] = new Vector2(_playarea.xMin, _playarea.yMax);
            borders[2] = new Vector2(_playarea.xMax, _playarea.yMax);
            borders[3] = new Vector2(_playarea.xMax, _playarea.yMin);
            borders[4] = new Vector2(_playarea.xMin, _playarea.yMin);
            for (int i = 0; i < borders.Length; i++)
            {
                border.GetComponent<EdgeCollider2D>().points = borders;
            }
        }

        void Start()
        {
            active = true;
            bombs = new GameObject[] { bombBase, bombDonut, bombCross };
            Invoke(nameof(SpawnBombs), 3);
        }

        private void SpawnBombs()
        {
            if (Difficulty == Models.Difficulty.EASY)
            {
                chance = Random.Range(0, 1);
            }
            if (Difficulty == Models.Difficulty.MEDIUM)
            {
                chance = Random.Range(0, 2);
            }
            if (Difficulty == Models.Difficulty.HARD)
            {
                chance = Random.Range(0, 3);
            }

            GameObject bomb = Instantiate(bombs[chance], gameObject.transform.position, Quaternion.identity, bombContainer.transform);
            bomb.transform.localPosition = new Vector3(Random.Range(_playarea.xMin, _playarea.xMax), Random.Range(_playarea.yMin, _playarea.yMax), transform.localPosition.z);
            bomb.SetActive(true);

            timer = Random.Range(1, 4);
            Invoke(nameof(SpawnBombs), timer);
        }

        public void CheckWinCondition(Collider2D col1, Collider2D col2, GameObject obj)
        {
            if (col1.IsTouching(col2))
            {
                Debug.Log("Chuckles... I'm in danger.");
                loseCounter++;
                //failsToLose--;
                player.GetComponent<ExpPlayer>().knockback = player.GetComponent<Rigidbody2D>().position - new Vector2(obj.transform.position.x, obj.transform.position.y);
                // Does not work why?
            }
            else
            {
                //successesToWin++;
                winCounter++;
            }

            if (winCounter == 20)
            {
                Debug.Log("You passed this quest my son. Now go forth into the world and prove them that you are a real hero of the people! Aka get some Pizza.");
                winCounter = 0;
                active = false;
                base.Harder();
                base.Win();
            }
            if (loseCounter == 3)
            {
                Debug.Log("Snake? SNAKE? SNAAAAAACKE!!!");
                loseCounter = 0;
                active = false;
                base.Easier();
                base.Lose();
            }
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
