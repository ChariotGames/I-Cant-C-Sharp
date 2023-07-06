using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Scripts.Models;

namespace Scripts.Games
{
    public class Evade : BaseGame
    {
        [SerializeField] private GameObject indLeft, indMid, indRight, laneLeft, laneMid, laneRight, player, obstacle;

        private bool active = false;
        private GameObject[] Indicators;
        private GameObject[] Lanes;
        private int firstRandom;
        private int secondRandom;
        private int thirdRandom;
        private float chance;
        private int winCounter = 0;

        // Start is called before the first frame update
        private void Awake()
        {
            player.GetComponent<EvadePlayer>().stick = keys.One.Input;
        }
        void Start()
        {
            active = true;
            Indicators = new GameObject[] { indLeft, indMid, indRight };
            Lanes = new GameObject[] { laneLeft, laneMid, laneRight };
            Invoke(nameof(ActivateIndicators), 3);
        }

        private void ActivateIndicators()
        {
            GenerateRandoms();
            chance = Random.value;

            Indicators[firstRandom].GetComponent<EvadeIndicator>().SetUp();

            if(Difficulty != Difficulty.EASY && chance >= 0.33f)
            {
                Indicators[secondRandom].GetComponent<EvadeIndicator>().SetUp();
            }
            if (Difficulty == Difficulty.HARD && chance >= 0.66f)
            {
                Indicators[thirdRandom].GetComponent<EvadeIndicator>().SetUp();
                obstacle.SetActive(true);
                obstacle.transform.position = Lanes[thirdRandom].transform.position + Vector3.back + Vector3.up;
            }

            if (active) Invoke(nameof(SetOfLane), 3);
        }

        private void SetOfLane()
        {
            BoxCollider2D playerCollider = player.GetComponent<BoxCollider2D>();

            StartCoroutine(AnimateColor(Lanes[firstRandom].GetComponent<SpriteRenderer>(), Color.white, Color.red, 0.4f));
            Indicators[firstRandom].GetComponent<EvadeIndicator>().SetDefault();
                
            if (Difficulty != Difficulty.EASY && chance >= 0.33f)
            {
                StartCoroutine(AnimateColor(Lanes[secondRandom].GetComponent<SpriteRenderer>(), Color.white, Color.red, 0.4f));
                Indicators[secondRandom].GetComponent<EvadeIndicator>().SetDefault();
                Indicators[thirdRandom].GetComponent<EvadeIndicator>().SetDefault();
            }

            CheckWinCondition(playerCollider, Lanes[firstRandom].GetComponent<BoxCollider2D>(), Lanes[secondRandom].GetComponent<BoxCollider2D>());
            obstacle.SetActive(false);
            if (active) Invoke(nameof(ActivateIndicators), 3);
        }

        private void CheckWinCondition(Collider2D col1, Collider2D col2, Collider2D col3)
        {
            if (col1.IsTouching(col2) || col1.IsTouching(col3) && Difficulty != Difficulty.EASY && chance >= 0.33f)
            {
                Debug.Log("Chuckles... I'm in danger.");
                //active = false;
                base.Lose();
            }
            else if (col1.IsTouching(col2))
            {
                Debug.Log("Chuckles... I'm in danger.");
                //active = false;
                base.Lose();
            }
            else
            {
                winCounter++;
            }

            if (winCounter == 5)
            {
                Debug.Log("You passed this quest my son. Now go forth into the world and prove them that you are a real hero of the people! Aka get some Pizza.");
                winCounter = 0;
                //active = false;
                base.Win();
            }
        }

        private void GenerateRandoms()
        {
            List<int> vc = new() { 0, 1, 2 };
            firstRandom = vc[Random.Range(0, vc.Count)];
            vc.Remove(firstRandom);
            secondRandom = vc[Random.Range(0, vc.Count)];
            vc.Remove(secondRandom);
            thirdRandom = vc[0]; 
        }

        private IEnumerator AnimateColor(SpriteRenderer sprite, Color original, Color target, float duration)
        {
            float elapsedTime = 0f;
            while (elapsedTime < duration)
            {
                sprite.color = Color.Lerp(original, target, elapsedTime / duration);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            elapsedTime = 0f;
            while (elapsedTime < duration)
            {
                sprite.color = Color.Lerp(target, original, elapsedTime / duration);
                elapsedTime += Time.deltaTime;
                yield return null;
            }
            sprite.color = original;
        }

    }
}
