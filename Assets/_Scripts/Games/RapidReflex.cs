using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using _Scripts._Input;
using Unity.VisualScripting;
using UnityEngine;
using Debug = System.Diagnostics.Debug;

namespace _Scripts.Games
{
    public class RapidReflex : Game
    {
        #region Serialized Fields
        
        [SerializeField] private int timeToAnswer;
        [SerializeField] private float lightTimer;
        [SerializeField] private GameObject lightsTop, lightsBottom, light;
        [SerializeField] private Color darkRed, lightRed, darkGreen, lightGreen;

        #endregion Serialized Fields

        #region Fields

        
        private const int NUMBER_LIGHTS = 5;
        private SpriteRenderer[] bulbsSpriteTop = new SpriteRenderer[NUMBER_LIGHTS];
        private SpriteRenderer[] bulbsSpriteBottom = new SpriteRenderer[NUMBER_LIGHTS];
        private float timeElapsed = 0;
        private bool isButtonPressed = false;

        #endregion Fields

        #region Built-Ins / MonoBehaviours

        void Start()
        {
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
                yield return StartCoroutine(lightAnimation());
                
                yield return StartCoroutine(measureTime());
            }
        }

        #endregion Game Mechanics / Methods

        #region Overarching Methods / Helpers

        private void spawnLights()
        {
            for (int i = -NUMBER_LIGHTS / 2; i <= NUMBER_LIGHTS / 2; i++)
            {
                GameObject newLight = Instantiate(light, lightsTop.transform);
                newLight.transform.Translate(i*1.1f, 0,0,Space.Self);
                GameObject bulb = newLight.transform.GetChild(0).gameObject;
                SpriteRenderer bulbSprite = bulb.GetComponent<SpriteRenderer>();
                bulbSprite.color = darkRed;
                bulbsSpriteTop[i+NUMBER_LIGHTS / 2] = bulbSprite;
                newLight.SetActive(true);
            }
            for (int i = -NUMBER_LIGHTS / 2; i <= NUMBER_LIGHTS / 2; i++)
            {
                GameObject newLight = Instantiate(light, lightsBottom.transform);
                newLight.transform.Translate(i*1.1f, 0,0,Space.Self);
                GameObject bulb = newLight.transform.GetChild(0).gameObject;
                SpriteRenderer bulbSprite = bulb.GetComponent<SpriteRenderer>();
                bulbSprite.color = darkGreen;
                bulbsSpriteBottom[i+NUMBER_LIGHTS / 2] = bulbSprite;
                newLight.SetActive(true);
            }
        }
        
        private void updateLightColor(SpriteRenderer bulb, Color color)
        {
            bulb.color = color;
        }

        private void turnOffAllLights()
        {
            for (int i = 0; i < NUMBER_LIGHTS; i++)
            {
                updateLightColor(bulbsSpriteTop[i], darkRed);
                updateLightColor(bulbsSpriteBottom[i], darkGreen);
            }
        }

        private IEnumerator lightAnimation()
        {
            for (int i = 0; i < NUMBER_LIGHTS; i++)
            {
                updateLightColor(bulbsSpriteTop[i], lightRed);
                yield return new WaitForSeconds(lightTimer);
            }
            isButtonPressed = false;
            for (int i = 0; i < NUMBER_LIGHTS; i++)
            {
                updateLightColor(bulbsSpriteBottom[i], lightGreen);
            }
        }

        private IEnumerator measureTime()
        {
            timeElapsed = 0;
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            yield return new WaitUntil(() => isButtonPressed);
            stopwatch.Stop();
            timeElapsed = stopwatch.ElapsedMilliseconds;
            turnOffAllLights();
            yield return new WaitForSeconds(2);
        }
        
        
        private void OnEnable()
        {
            InputHandler.EastBtnAction += EastButtonPressed;
        }

        public void EastButtonPressed()
        {
            isButtonPressed = true;
        }
        
        private void OnDisable()
        {
            InputHandler.EastBtnAction -= EastButtonPressed;
        }

        #endregion Overarching Methods / Helpers
    }
}