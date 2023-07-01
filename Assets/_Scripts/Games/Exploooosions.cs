using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _Scripts.Games
{
    public class Exploooosions : BaseGame
    {
        [SerializeField] private GameObject bombBase, bombDonut, bombCross, bombContainer;

        public GameObject player;

        private bool active = false;
        private GameObject[] bombs;
        private int chance;
        private int timer;
        private int winCounter = 0;
        private int loseCounter = 0;

        // Start is called before the first frame update
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
            bomb.transform.position = new Vector3(Random.Range(-5, 6), Random.Range(-5, 6), transform.position.z);
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
                player.GetComponent<ExpPlayer>().knockback = player.GetComponent<Rigidbody2D>().position - new Vector2(obj.transform.position.x, obj.transform.position.y);
                // Does not work why?
            }
            else
            {
                winCounter++;
            }

            if (winCounter == 25)
            {
                Debug.Log("You passed this quest my son. Now go forth into the world and prove them that you are a real hero of the people! Aka get some Pizza.");
                active = false;
                base.Win();
            }
            if (loseCounter == 3)
            {
                Debug.Log("Snake? SNAKE? SNAAAAAACKE!!!");
                active = false;
                base.Lose();
            }
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
