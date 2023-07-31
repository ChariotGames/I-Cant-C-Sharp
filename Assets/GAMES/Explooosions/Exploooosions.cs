using System;
using System.Collections;
using Scripts.Controllers;
using Scripts.Models;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Scripts.Games
{
    public class Exploooosions : BaseGame
    {
        [Space]
        [Header("Game Specific Stuff")]
        public GameObject player;
        [SerializeField] private GameObject bombBase, bombDonut, bombCross, bombContainer, mainContainer;


        private bool active = false;
        private GameObject[] bombs;
        private int chance;
        private int timer;
        //private int _successes = 0;
        //private int _fails = 0;

        private void Awake()
        {
            player.GetComponent<ExpPlayer>().stick = _keys.One.Input;
        }

        IEnumerator Start()
        {
            yield return StartCoroutine(AnimateInstruction());
            mainContainer.SetActive(true);
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
                //_fails++;
                base.Fail();
                //base.AnimateFail(1, 3);
                StartCoroutine(player.GetComponent<ExpPlayer>().AnimateColor(player.GetComponent<SpriteRenderer>(), Color.white, new Color(0.3f, 0.3f, 0.3f), 0.5f));
                player.GetComponent<ExpPlayer>().knockback = player.GetComponent<Rigidbody2D>().position - new Vector2(obj.transform.position.x, obj.transform.position.y);
                // Does not work why?
            }
            else
            {
                base.Success();
                //base.AnimateSuccess(1, 10);
                //base.ScoreUp();
                //_successes++;
            }

            active = false;
            
            /*if (_successes == successesToWin)
            {
                Debug.Log("You passed this quest my son. Now go forth into the world and prove them that you are a real hero of the people! Aka get some Pizza.");
                //_successes = 0;
                active = false;
                base.Harder();
                //base.Win();
            }
            if (_fails == 0)
            {
                Debug.Log("Snake? SNAKE? SNAAAAAACKE!!!");
                //loseCounter = 0;
                active = false;
                base.Easier();
                //base.Lose();
            }*/
        }
    }
}
