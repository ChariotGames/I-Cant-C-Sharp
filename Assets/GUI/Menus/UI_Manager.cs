using Scripts.Games;
using Scripts.Models;
using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Scripts.Controllers
{
    public class UI_Manager : MonoBehaviour
    {
        [SerializeField] private Settings settings;
        [SerializeField] private Image[] hearts;
        [SerializeField] private Color heartColor;
        [SerializeField] private Image characterImage;
        [SerializeField] private Sprite fullHeart, emptyHeart;
        [SerializeField] private TMP_Text heartCounter, scoreCounter, timeCounter;
        [SerializeField] private Transform leftKeys, rightKeys, centerKeys;
        [SerializeField] private GameObject scoreNStuff, keysNTimes, minigameManager, templateKeys;
        [SerializeField] private Timer leftTimer, rightTimer;
        [SerializeField] private Transform leftAnim, rightAnim;
        [SerializeField] private AnimationPack tempWin, tempLose;
        [SerializeField] private Animator bigBook, centerPaper, camAnimator;

        private int _score = 0;
        private float _time = 0;
        private bool _timerOn;


        private void Start()
        {
            StartCoroutine(DisplayUI(1.05f, true));

            _timerOn = true;
            characterImage.sprite = settings.SelectedCharacter.Icon;
            if (settings.Lives > 3)
            {
                hearts[0].sprite = fullHeart;
                hearts[1].gameObject.SetActive(false);
                hearts[2].gameObject.SetActive(false);
                heartCounter.text = settings.Lives.ToString();
                heartCounter.gameObject.SetActive(true);
            }
            else
            {
                heartCounter.gameObject.SetActive(false);
                //display three empty hearts
                foreach (Image img in hearts)
                {
                    img.gameObject.SetActive(true);
                    img.color = Color.clear;
                }

                //depending on the health size, the amount of hearts will be filled
                for (int i = 0; i < settings.Lives; i++)
                    hearts[i].color = heartColor;
            }
        }
        private void OnEnable()
        {
            BaseGame.OnPlayAnimations += PlayAnimations;
            BaseGame.OnScoreUpdate += ScoreDisplay;
            BaseGame.OnTimerUpdate += PlayTimer;
            BaseGame.OnTimerStop += StopTimer;
            MinigameManager.OnLoseLife += UpdateHearts;
            MinigameManager.OnSetKeys += DisplayKeys;
            MinigameManager.OnClearKeys += ClearKeys;
            MinigameManager.OnCenterDisplay += DisplayCenter;
            PauseMenu.OnToMenu += OnToMenu;
        }

        private void OnDisable()
        {
            BaseGame.OnPlayAnimations -= PlayAnimations;
            BaseGame.OnScoreUpdate -= ScoreDisplay;
            BaseGame.OnTimerUpdate -= PlayTimer;
            BaseGame.OnTimerStop -= StopTimer;
            MinigameManager.OnLoseLife -= UpdateHearts;
            MinigameManager.OnSetKeys -= DisplayKeys;
            MinigameManager.OnClearKeys -= ClearKeys;
            MinigameManager.OnCenterDisplay -= DisplayCenter;
            PauseMenu.OnToMenu -= OnToMenu;
        }

        // Update is called once per frame
        void Update()
        {
            if (settings.Lives <= 0)
            {
                _timerOn = false;
                //PlayerPrefs.SetString("Score", _score.ToString("D3"));
                

                //if (_score > settings.Highscore)
                //{
                //    settings.Highscore = _score;
                //    PlayerPrefs.SetInt("Highscore", _score);
                //}

                //TimeSpan timePlaying = TimeSpan.FromSeconds(_time);
                //PlayerPrefs.SetString("Time", timePlaying.ToString("mm':'ss"));
                
                //if(settings.BaseDifficulty != Difficulty.TUTORIAL)
                //{
                    settings.Score = _score;
                    settings.Time = (int)_time;
                //}
                StartCoroutine(ToGameOver());
            }

            if (_timerOn)
            {
                _time += Time.deltaTime;
                TimeSpan timePlaying = TimeSpan.FromSeconds(_time);
                timeCounter.text = timePlaying.ToString("mm':'ss");
            }
        }

        private IEnumerator ToGameOver()
        {
            PrepareScreenChange();
            camAnimator.SetTrigger("PlayToEnd");
            yield return new WaitForSeconds(1.05f);
            SceneManager.LoadScene((int)SceneNr.GameOver);
        }

        private void OnToMenu()
        {
            StartCoroutine(ToMainScreen());
        }
        
        private IEnumerator ToMainScreen()
        {
            PrepareScreenChange();
            camAnimator.SetTrigger("PlayToMenu");
            yield return new WaitForSeconds(1.05f);
            SceneManager.LoadScene((int)SceneNr.MainMenu);
        }

        private void PrepareScreenChange()
        {
            minigameManager.SetActive(false);
            scoreNStuff.SetActive(false);
            keysNTimes.SetActive(false);
            bigBook.SetTrigger("BigBookOut");
            centerPaper.SetTrigger("CenterOut");
        }

        public IEnumerator DisplayUI(float delay, bool state)
        {
            yield return new WaitForSeconds(delay);
            scoreNStuff.SetActive(state);
        }

        private void UpdateHearts()
        {
            //if the lives are higher than three,
            //there is only one heart with the number of lives next to it displayed
            if (settings.Lives > 3)
                heartCounter.text = settings.Lives.ToString();

            else if (settings.Lives == 3)
            {
                heartCounter.gameObject.SetActive(false);
                hearts[1].gameObject.SetActive(true);
                hearts[2].gameObject.SetActive(true);
            }
            else
                hearts[settings.Lives].color = Color.clear;
        }

        public void ScoreDisplay(int change)
        {
            _score = Mathf.Clamp(_score += change, 0, 999);
            scoreCounter.text = _score.ToString("D3");
        }

        private void DisplayKeys(string side, KeyMap keys, ActionNames actions)
        {
            Transform parent = leftKeys;
            if (side.Contains("Right")) parent = rightKeys;
            if (side.Contains("Center")) parent = centerKeys;

            for (int i = 0; i < 4; i++)
            {
                if (keys.All[i] == null || actions.All[i].Equals("")) return;

                GameObject key = Instantiate(templateKeys, parent);
                TMP_Text[] texts = key.transform.GetComponentsInChildren<TMP_Text>();
                texts[0].text = keys.All[i].Icon;
                texts[1].text = actions.All[i];
                key.SetActive(true);
            }
        }

        private void DisplayCenter(bool centerLoaded)
        {
            if (centerLoaded)
            {
                centerPaper.ResetTrigger("CenterOut");
                bigBook.ResetTrigger("BigBookIn");
                centerPaper.SetTrigger("CenterIn");
                bigBook.SetTrigger("BigBookOut");
            }
            else
            {
                centerPaper.ResetTrigger("CenterIn");
                bigBook.ResetTrigger("BigBookOut");
                centerPaper.SetTrigger("CenterOut");
                bigBook.SetTrigger("BigBookIn");
            }
        }

        private void ClearKeys(string side)
        {
            if (side.Contains("Left")) RemoveKeys(leftKeys);
            if (side.Contains("Right")) RemoveKeys(rightKeys);
            if (side.Contains("Center")) RemoveKeys(centerKeys);
        }

        /// <summary>
        /// Plays an animation of a certain type and in a certain position.
        /// </summary>
        /// <param name="parent">Where to spawn the animation.</param>
        /// <param name="anim">The animation type.</param>
        /// <param name="count">The number of stars.</param>
        /// <param name="numerator">The portion to fill.</param>
        /// <param name="denominator">The number of portions total.</param>
        private void PlayAnimations(Transform parent, AnimType anim, int count, float numerator, float denominator)
        {
            AnimationPack pack = tempWin;
            if (anim == AnimType.Lose) pack = tempLose;

            Transform pos = parent;
            if (parent.name.Contains("Right")) pos = rightAnim;
            if (parent.name.Contains("Left") || parent.name.Contains("Center")) pos = leftAnim;

            pack = Instantiate(pack, pos);
            pack.Run(count, numerator, denominator, anim.ToString());
        }

        private void PlayTimer(string side, float duration)
        {
            Timer timer = leftTimer;
            if (side.Contains("Right")) timer = rightTimer;
            timer.Duration = duration;
            timer.Run();
        }

        private void StopTimer(string side)
        {
            Timer timer = leftTimer;
            if (side.Contains("Right")) timer = rightTimer;
            timer.Stop();
        }

        private void RemoveKeys(Transform parent)
        {
            for (int i = 0; i < parent.childCount; i++)
                Destroy(parent.GetChild(i).gameObject);
        }
    }
}
