using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using _Scripts._Input;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using Debug = System.Diagnostics.Debug;
using Random = UnityEngine.Random;

namespace _Scripts.Games
{
    public class RapidReflex : Game
    {
    #region Serialized Fields
        
        [SerializeField] private Difficulty difficulty;
        [SerializeField] private int timeToAnswerInMs;
        [SerializeField] private float lightTimer;
        [SerializeField] private GameObject lightsTop, lightsBottom, light, overlayContainer, background;
        [SerializeField] private Color darkRed, lightRed, darkGreen, lightGreen;
        [SerializeField] private List<Color> flashColor;
        [SerializeField] private TMP_Text gameStateText;

    #endregion Serialized Fields

    #region Fields
    
        private const int NUMBER_LIGHTS_ROW = 5;
        private SpriteRenderer[] bulbsSpriteTop = new SpriteRenderer[NUMBER_LIGHTS_ROW];
        private SpriteRenderer[] bulbsSpriteBottom = new SpriteRenderer[NUMBER_LIGHTS_ROW];
        private SpriteRenderer backroundSprite;
        private float timeElapsed = 0;
        private bool isButtonPressed = false;
        private float randomDelay = 0;

    #endregion Fields

    #region Built-Ins / MonoBehaviours

        void Start()
        {
            flashColor.Add(lightRed);
            flashColor.Add(lightGreen);
            backroundSprite = background.GetComponent<SpriteRenderer>();
            spawnLights();
            StartCoroutine(GameCoroutine());
        }

    #endregion Built-Ins / MonoBehaviours

    #region GetSets / Properties

            

    #endregion GetSets / Properties

    #region Game Mechanics / Methods

        private IEnumerator GameCoroutine()
        {
            yield return new WaitForSeconds(1);
            while (true)
            {
                updateRandomDelay();
                yield return StartCoroutine(lightAnimation());
                
                yield return StartCoroutine(measureTime());
                
                turnOffAllLights();
                yield return StartCoroutine(determineGamestate());
            }
        }

    #endregion Game Mechanics / Methods

    #region Overarching Methods / Helpers

        private void spawnLights()
        {
            for (int i = -NUMBER_LIGHTS_ROW / 2; i <= NUMBER_LIGHTS_ROW / 2; i++)
            {
                GameObject newLight = Instantiate(light, lightsTop.transform);
                newLight.transform.Translate(i*1.1f, 0,0,Space.Self);
                GameObject bulb = newLight.transform.GetChild(0).gameObject;
                SpriteRenderer bulbSprite = bulb.GetComponent<SpriteRenderer>();
                bulbSprite.color = darkRed;
                bulbsSpriteTop[i+NUMBER_LIGHTS_ROW / 2] = bulbSprite;
                newLight.SetActive(true);
            }
            for (int i = -NUMBER_LIGHTS_ROW / 2; i <= NUMBER_LIGHTS_ROW / 2; i++)
            {
                GameObject newLight = Instantiate(light, lightsBottom.transform);
                newLight.transform.Translate(i*1.1f, 0,0,Space.Self);
                GameObject bulb = newLight.transform.GetChild(0).gameObject;
                SpriteRenderer bulbSprite = bulb.GetComponent<SpriteRenderer>();
                bulbSprite.color = darkGreen;
                bulbsSpriteBottom[i+NUMBER_LIGHTS_ROW / 2] = bulbSprite;
                newLight.SetActive(true);
            }
        }
        
        private void updateLightColor(SpriteRenderer bulb, Color color)
        {
            bulb.color = color;
        }

        private void turnOffAllLights()
        {
            for (int i = 0; i < NUMBER_LIGHTS_ROW; i++)
            {
                updateLightColor(bulbsSpriteTop[i], darkRed);
                updateLightColor(bulbsSpriteBottom[i], darkGreen);
            }
        }

        private IEnumerator lightAnimation()
        {
            isButtonPressed = false;
            for (int i = 0; i < NUMBER_LIGHTS_ROW; i++)
            {
                if (checkForEarlyLose()) yield break; 
                updateLightColor(bulbsSpriteTop[i], lightRed);
                if (difficulty == Difficulty.LVL3 && i == NUMBER_LIGHTS_ROW-1) StartCoroutine(randomDistraction());
                yield return new WaitForSeconds(lightTimer + (i == NUMBER_LIGHTS_ROW-1 && difficulty != Difficulty.LVL1 ? randomDelay : 0));
                if (checkForEarlyLose()) yield break; 
            }
            UnityEngine.Debug.Log("Delay: " + (lightTimer + randomDelay) + " s");
            for (int i = 0; i < NUMBER_LIGHTS_ROW; i++)
            {
                updateLightColor(bulbsSpriteBottom[i], lightGreen);
            }
        }

        private IEnumerator measureTime()
        {
            timeElapsed = 0;
            if (checkForEarlyLose()) yield break; 
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            yield return new WaitUntil(() => isButtonPressed || stopwatch.ElapsedMilliseconds > timeToAnswerInMs);
            stopwatch.Stop();
            timeElapsed = stopwatch.ElapsedMilliseconds;
        }

        private IEnumerator determineGamestate()
        {
            overlayContainer.SetActive(true);
            if (timeElapsed < timeToAnswerInMs && timeElapsed >= 0)
            {
                gameWon();
            }
            else
            {
                gameLost();
            }
            yield return new WaitForSeconds(2);
            overlayContainer.SetActive(false);
        }

        private IEnumerator randomDistraction()
        {
            if (Random.Range(0, 1.1f) > 0.5f)
            {
                UnityEngine.Debug.Log("No Distraction");
                yield break;
            }

            float delay = Random.Range(0.5f, lightTimer + randomDelay / 3);
            yield return new WaitForSeconds(delay);
            UnityEngine.Debug.Log("Flashdelay: " + delay);
            StartCoroutine(flashBackground());
        }

        private IEnumerator flashBackground()
        {
            backroundSprite.color = flashColor[Random.Range(0, flashColor.Count)];
            background.SetActive(true);
            yield return new WaitForSeconds(0.2f);
            background.SetActive(false);
        }

        private void gameWon()
        {
            gameStateText.text = "Rapid Reflex: " + timeElapsed + " ms";
            base.Win();
        }
        
        private void gameLost()
        {
            gameStateText.text = timeElapsed > 0 ? "Too slow!" : "Too Early!";
            base.Lose();
        }

        private void updateRandomDelay()
        {
            randomDelay = Random.Range(0, 1.5f);
        }

        private Boolean checkForEarlyLose()
        {
            if (isButtonPressed)
            {
                timeElapsed = -1;
                return true;
            }
            return false;
        }

        private void OnEnable()
        {
            InputHandler.EastBtnAction += EastButtonPressed;
        }

        public void EastButtonPressed()
        {
            isButtonPressed = true;
            UnityEngine.Debug.Log("Button Pressed!");
        }
        
        private void OnDisable()
        {
            InputHandler.EastBtnAction -= EastButtonPressed;
        }

    #endregion Overarching Methods / Helpers
    }
}