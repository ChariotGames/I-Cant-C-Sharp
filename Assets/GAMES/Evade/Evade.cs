using System;
using System.Collections;
using System.Collections.Generic;
using Scripts.Controllers;
using UnityEngine;
using Scripts.Models;
using Random = UnityEngine.Random;

namespace Scripts.Games
{
    public class Evade : BaseGame
    {
        [Space]
        [Header("Game Specific Stuff")]
        [SerializeField] private GameObject player;
        [SerializeField] private GameObject indLeft, indMid, indRight, laneLeft, laneMid, laneRight, obstacle, MainContainer;
        [SerializeField] private AudioClip[] clips;

        private bool active = false;
        private GameObject[] Indicators;
        private GameObject[] Lanes;
        private int firstRandom;
        private int secondRandom;
        private int thirdRandom;
        private float chance;
        //private int _successes = 0;
        private Color color;

        // Start is called before the first frame update
        private void Awake()
        {
            player.GetComponent<EvadePlayer>().stick = _keys.One.Input;
            color = laneLeft.GetComponent<SpriteRenderer>().color;
        }

        IEnumerator Start()
        {
            yield return StartCoroutine(AnimateInstruction());
            MainContainer.SetActive(true);
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
            Indicators[secondRandom].transform.GetChild(1).gameObject.SetActive(true);
            Indicators[thirdRandom].transform.GetChild(1).gameObject.SetActive(true);

            if (Difficulty != Difficulty.EASY && chance >= 0.33f)
            {
                Indicators[secondRandom].GetComponent<EvadeIndicator>().SetUp();
                Indicators[secondRandom].transform.GetChild(1).gameObject.SetActive(false);
            }
            if (Difficulty == Difficulty.HARD && chance >= 0.66f)
            {
                Indicators[thirdRandom].GetComponent<EvadeIndicator>().SetUp();
                Indicators[thirdRandom].transform.GetChild(1).gameObject.SetActive(false);
                obstacle.SetActive(true);
                obstacle.transform.position = Lanes[thirdRandom].transform.position + Vector3.back;
            }

            if (active) Invoke(nameof(SetOfLane), 3);
        }

        private void SetOfLane()
        {
            BoxCollider2D playerCollider = player.GetComponent<BoxCollider2D>();

            StartCoroutine(AnimateColor(Lanes[firstRandom].GetComponent<SpriteRenderer>(), color, Color.red, 0.4f));
            Indicators[firstRandom].GetComponent<EvadeIndicator>().SetDefault();
            Indicators[secondRandom].transform.GetChild(1).gameObject.SetActive(false);
            Indicators[thirdRandom].transform.GetChild(1).gameObject.SetActive(false);

            if (Difficulty != Difficulty.EASY && chance >= 0.33f)
            {
                StartCoroutine(AnimateColor(Lanes[secondRandom].GetComponent<SpriteRenderer>(), color, Color.red, 0.4f));
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
                player.transform.GetChild(1).gameObject.SetActive(true);
                Invoke(nameof(calm), 0.5f);
                //base.AnimateFail(player.transform, 1, 1);
                //base.Lose();
                base.Fail();
            }
            else if (col1.IsTouching(col2))
            {
                Debug.Log("Chuckles... I'm in danger.");
                //active = false;
                //base.AnimateFail(player.transform, 1, 1);
                //base.Lose();
                base.Fail();
            }
            else
            {
                //_successes++;
                //base.AnimateSuccess(player.transform, 1, 5);
                //base.ScoreUp();
                base.Success();
            }

            /*if (_successes == successesToWin)
            {
                Debug.Log("You passed this quest my son. Now go forth into the world and prove them that you are a real hero of the people! Aka get some Pizza.");
                //_successes = 0;
                //active = false;
                base.Harder();
                //base.Win();
            }*/
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

        private void calm()
        {
            player.transform.GetChild(1).gameObject.SetActive(false);
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
