using _Scripts.Models;
using _Scripts._Input;
using System.Collections;
using System.Diagnostics;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _Scripts.Games
{
    /// <summary>
    /// Two rows of lights are turned on one bit by bit
    /// Player has to press a button as fast as possible once all are on.
    /// 
    /// Easy: each light turns on after the same time
    /// Medium: last light has a random delay
    /// Hard: distracting popups appear
    /// </summary>
    public class RapidReflex : BaseGame
    {
    #region Serialized Fields
        
        [SerializeField] private GameObject lightsTop, lightsBottom, lightTemplate, overlayContainer;
        [SerializeField] private SpriteRenderer[] _bulbsSpriteTop, _bulbsSpriteBottom;
        [SerializeField] private Color darkRed, lightRed, darkGreen, lightGreen;
        [SerializeField] private TMP_Text gameState;
        [SerializeField] private float lightTimer;
        [SerializeField] [Range(100,1000)] private int timeToAnswerInMs;

        #endregion Serialized Fields

        #region Fields

        private const int NUMBER_LIGHTS = 5;
        private float _timeElapsed = 0, _randomDelay = 0;
        private bool _isButtonPressed = false;

    #endregion Fields

    #region Built-Ins / MonoBehaviours

        void Start()
        {
            _bulbsSpriteTop = SpawnLights(NUMBER_LIGHTS, darkRed, lightsTop.transform);
            _bulbsSpriteBottom = SpawnLights(NUMBER_LIGHTS, darkGreen, lightsBottom.transform);
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
                UpdateRandomDelay();
                yield return StartCoroutine(LightAnimation());
                
                yield return StartCoroutine(MeasureTime());
                
                TurnOffAllLights();
                yield return StartCoroutine(DetermineGamestate());
            }
        }

    #endregion Game Mechanics / Methods

    #region Overarching Methods / Helpers

        private SpriteRenderer[] SpawnLights(int amount, Color color, Transform parent)
        {
            SpriteRenderer[] renderers = new SpriteRenderer[amount];
            for (int i = -amount / 2; i <= amount / 2; i++)
            {
                GameObject newLight = Instantiate(lightTemplate, parent);
                newLight.transform.Translate(i * 1.1f, 0, 0, Space.Self);
                GameObject bulb = newLight.transform.GetChild(0).gameObject;
                SpriteRenderer bulbSprite = bulb.GetComponent<SpriteRenderer>();
                bulbSprite.color = color;
                renderers[i + amount / 2] = bulbSprite;
                newLight.SetActive(true);
            }
            return renderers;
        }
        
        private void UpdateLightColor(SpriteRenderer bulb, Color color)
        {
            bulb.color = color;
        }

        private void TurnOffAllLights()
        {
            for (int i = 0; i < NUMBER_LIGHTS; i++)
            {
                UpdateLightColor(_bulbsSpriteTop[i], darkRed);
                UpdateLightColor(_bulbsSpriteBottom[i], darkGreen);
            }
        }

        private IEnumerator LightAnimation()
        {
            _isButtonPressed = false;
            for (int i = 0; i < NUMBER_LIGHTS; i++)
            {
                if (CheckForEarlyLose()) yield break; 
                UpdateLightColor(_bulbsSpriteTop[i], lightRed);
                yield return new WaitForSeconds(lightTimer + (i == NUMBER_LIGHTS-1 && Difficulty != Difficulty.EASY ? _randomDelay : 0));
                if (CheckForEarlyLose()) yield break; 
            }
            UnityEngine.Debug.Log("Delay: " + (_randomDelay + _randomDelay) + " s");
            for (int i = 0; i < NUMBER_LIGHTS; i++)
            {
                UpdateLightColor(_bulbsSpriteBottom[i], lightGreen);
            }
        }

        private IEnumerator MeasureTime()
        {
            _timeElapsed = 0;
            if (CheckForEarlyLose()) yield break; 
            Stopwatch stopwatch = new();
            stopwatch.Start();
            yield return new WaitUntil(() => _isButtonPressed || stopwatch.ElapsedMilliseconds > timeToAnswerInMs);
            stopwatch.Stop();
            _timeElapsed = stopwatch.ElapsedMilliseconds;
        }

        private IEnumerator DetermineGamestate()
        {
            overlayContainer.SetActive(true);
            if (_timeElapsed < timeToAnswerInMs && _timeElapsed >= 0)
            {
                GameWon();
            }
            else
            {
                GameLost();
            }
            yield return new WaitForSeconds(2);
            overlayContainer.SetActive(false);
        }

        private void GameWon()
        {
            gameState.text = "Rapid Reflex: " + _timeElapsed + " ms";
            base.Win();
        }
        
        private void GameLost()
        {
            gameState.text = _timeElapsed > 0 ? "Too slow!" : "Too Early!";
            base.Lose();
        }

        private void UpdateRandomDelay()
        {
            _randomDelay = Random.Range(0, 1.5f);
        }

        private bool CheckForEarlyLose()
        {
            if (_isButtonPressed)
            {
                _timeElapsed = -1;
                return true;
            }
            return false;
        }

        private void OnEnable()
        {
            InputHandler.ButtonEast += EastButtonPressed;
            InputHandler.ShoulderRight += EastButtonPressed;
        }

        public void EastButtonPressed()
        {
            _isButtonPressed = true;
            UnityEngine.Debug.Log("Button Pressed!");
        }
        
        private void OnDisable()
        {
            InputHandler.ButtonEast -= EastButtonPressed;
            InputHandler.ShoulderRight -= EastButtonPressed;
        }

    #endregion Overarching Methods / Helpers
    }
}