using Scripts.Models;
using System.Collections;
using System.Collections.Generic;
using Scripts.Controllers;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;
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

        [Space]
        [Header("Game Specific Stuff")]
        [SerializeField] private TMP_Text gameState;
        [SerializeField] private GameObject lightTemplate, overlayContainer, lightContainer;
        [SerializeField] private Transform lightsTop, lightsBottom;
        [SerializeField] private SpriteRenderer[] bulbsSpriteTop, bulbsSpriteBottom;
        [SerializeField] private SpriteRenderer backgroundSprite;
        [SerializeField] private Color darkRed, lightRed, darkGreen, lightGreen;
        [SerializeField] private Color backgroundColor;
        [SerializeField] private List<Color> flashColor;
        [SerializeField] private float lightTimer;

    #endregion Serialized Fields

    #region Fields

        private AudioSource audioSource;
        private const int NUMBER_LIGHTS = 5;
        private float _timeElapsed = 0, _randomDelay = 0, _timeToAnswer;
        private bool _isButtonPressed = false;

    #endregion Fields

    #region Built-Ins / MonoBehaviours
    
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        backgroundSprite.color = backgroundColor;
        flashColor.Add(lightRed);
        flashColor.Add(lightGreen);
        //backgroundSprite = background.GetComponent<SpriteRenderer>();
        bulbsSpriteTop = SpawnLights(NUMBER_LIGHTS, darkRed, lightsTop);
        bulbsSpriteBottom = SpawnLights(NUMBER_LIGHTS, darkGreen, lightsBottom);
        StartCoroutine(GameCoroutine());
    }

    #endregion Built-Ins / MonoBehaviours

    #region GetSets / Properties

            

    #endregion GetSets / Properties

    #region Game Mechanics / Methods

        private IEnumerator GameCoroutine()
        {
            yield return StartCoroutine(AnimateInstruction());
            lightContainer.SetActive(true);
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
                UpdateLightColor(bulbsSpriteTop[i], darkRed);
                UpdateLightColor(bulbsSpriteBottom[i], darkGreen);
            }
        }

        private IEnumerator LightAnimation()
        {
            _isButtonPressed = false;
            audioSource.pitch = 0.75f;
            for (int i = 0; i < NUMBER_LIGHTS; i++)
            {
                if (CheckForEarlyLose()) yield break;
                audioSource.Play();
                UpdateLightColor(bulbsSpriteTop[i], lightRed);
                if (Difficulty == Difficulty.HARD && i == NUMBER_LIGHTS-1) StartCoroutine(RandomDistraction());
                yield return new WaitForSeconds(lightTimer + (i == NUMBER_LIGHTS-1 && Difficulty != Difficulty.EASY ? _randomDelay : 0));
                if (CheckForEarlyLose()) yield break; 
            }
            //Debug.Log("Delay: " + (_randomDelay + _randomDelay) + " s");
            audioSource.pitch = 1.5f;
            audioSource.Play();
            for (int i = 0; i < NUMBER_LIGHTS; i++)
            {
                UpdateLightColor(bulbsSpriteBottom[i], lightGreen);
            }
        }

        private IEnumerator MeasureTime()
        {
            _timeElapsed = -1;
            if (CheckForEarlyLose()) yield break; 
            float timer = Time.time;
            //Debug.Log(_timeToAnswer);
            //RunTimer(_timeToAnswer);
            yield return new WaitUntil(() => _isButtonPressed || Time.time - timer > _timeToAnswer);
            _timeElapsed = Time.time - timer;
        }

        private IEnumerator DetermineGamestate()
        {
            overlayContainer.SetActive(true);
            if (_timeElapsed < _timeToAnswer && _timeElapsed >= 0)
            {
                gameState.text = "rapid reflex: " + (int)(_timeElapsed * 1000) + " ms";
                Success();
            }
            else
            {
                gameState.text = _timeElapsed > 0 ? "too slow!" : "too early!";
                Fail();
            }
            yield return new WaitForSeconds(1);
            overlayContainer.SetActive(false);
            yield return new WaitForSeconds(0.5f);
        }
        
        private IEnumerator RandomDistraction()
        {
            if (Random.Range(0f, 1f) > 0.5f)
            {
                //Debug.Log("No Distraction");
                yield break;
            }

            float delay = Random.Range(0.5f, lightTimer + _randomDelay / 3);
            yield return new WaitForSeconds(delay);
            StartCoroutine(FlashBackground());
        }

        private IEnumerator FlashBackground()
        {
            backgroundSprite.color = flashColor[Random.Range(0, flashColor.Count)];
            //background.SetActive(true);
            yield return new WaitForSeconds(0.2f);
            //background.SetActive(false);
            backgroundSprite.color = backgroundColor;
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
        
        private protected override void SetDifficulty()
        {
            _timeToAnswer = 1.33f - (int)difficulty * 0.33f;
        }

        private void OnEnable()
        {
            SetDifficulty();
            _keys.One.Input.action.started += EastButtonPressed;
        }

        public void EastButtonPressed(InputAction.CallbackContext ctx)
        {
            _isButtonPressed = true;
        }
        
        private void OnDisable()
        {
            _keys.One.Input.action.started -= EastButtonPressed;
        }

    #endregion Overarching Methods / Helpers
    }
}