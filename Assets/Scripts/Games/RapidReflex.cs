using Scripts.Models;
using Scripts._Input;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Scripts.Controllers;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;
using Debug = UnityEngine.Debug;
using Random = UnityEngine.Random;

namespace Scripts.Games
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
        
        [SerializeField] private GameObject lightsTop, lightsBottom, lightTemplate, overlayContainer, background;
        [SerializeField] private SpriteRenderer[] _bulbsSpriteTop, _bulbsSpriteBottom;
        [SerializeField] private Color darkRed, lightRed, darkGreen, lightGreen;
        [SerializeField] private List<Color> flashColor;
        [SerializeField] private TMP_Text gameState;
        [SerializeField] private float lightTimer;
        [SerializeField] [Range(0.25f, 1)] private float timeToAnswer;

        #endregion Serialized Fields

    #region Fields

        private const int NUMBER_LIGHTS = 5;
        private float _timeElapsed = 0, _randomDelay = 0;
        private bool _isButtonPressed = false;
        private SpriteRenderer _backgroundSprite;
        

    #endregion Fields

    #region Built-Ins / MonoBehaviours
        
        //private void Awake()
        //{
        //    SetUp();
        //}
    
    void Start()
    {
        flashColor.Add(lightRed);
        flashColor.Add(lightGreen);
        _backgroundSprite = background.GetComponent<SpriteRenderer>();
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
                if (Difficulty == Difficulty.HARD && i == NUMBER_LIGHTS-1) StartCoroutine(RandomDistraction());
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
            _timeElapsed = -1;
            if (CheckForEarlyLose()) yield break; 
            float timer = Time.time;
            yield return new WaitUntil(() => _isButtonPressed || Time.time - timer > timeToAnswer);
            _timeElapsed = Time.time - timer;
        }

        private IEnumerator DetermineGamestate()
        {
            overlayContainer.SetActive(true);
            if (_timeElapsed < timeToAnswer && _timeElapsed >= 0)
            {
                gameState.text = "Rapid Reflex: " + (int)(_timeElapsed * 1000) + " ms";
                yield return new WaitForSeconds(1);
                GameWon();
            }
            else
            {
                gameState.text = _timeElapsed > 0 ? "Too slow!" : "Too Early!";
                yield return new WaitForSeconds(1);
                GameLost();
            }
            yield return new WaitForSeconds(2);
            Debug.Log("Test");
            overlayContainer.SetActive(false);
        }
        
        private IEnumerator RandomDistraction()
        {
            if (Random.Range(0f, 1f) > 0.5f)
            {
                UnityEngine.Debug.Log("No Distraction");
                yield break;
            }

            float delay = Random.Range(0.5f, lightTimer + _randomDelay / 3);
            yield return new WaitForSeconds(delay);
            UnityEngine.Debug.Log("Flashdelay: " + delay);
            StartCoroutine(FlashBackground());
        }

        private IEnumerator FlashBackground()
        {
            _backgroundSprite.color = flashColor[Random.Range(0, flashColor.Count)];
            background.SetActive(true);
            yield return new WaitForSeconds(0.2f);
            background.SetActive(false);
        }

        private void GameWon()
        {
            base.Win();
        }
        
        private void GameLost()
        {
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
                return true;
            }
            return false;
        }

        private void OnEnable()
        {
            keys.One.Input.action.started += EastButtonPressed;
        }

        public void EastButtonPressed(InputAction.CallbackContext ctx)
        {
            _isButtonPressed = true;
            UnityEngine.Debug.Log(ctx.canceled);
        }
        
        private void OnDisable()
        {
            keys.One.Input.action.started -= EastButtonPressed;
        }

    #endregion Overarching Methods / Helpers
    }
}