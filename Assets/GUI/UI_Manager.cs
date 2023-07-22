using Scripts.Games;
using Scripts.Models;
using System;
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
        [SerializeField] private Sprite fullHeart, emptyHeart;
        [SerializeField] private TMP_Text heartCounter, scoreCounter, timeCounter;
        [SerializeField] private Transform leftKeys, rightKeys, centerKeys;
        [SerializeField] private GameObject templateKeys;
        [SerializeField] private Image leftTimer, rightTimer;
        [SerializeField] private Transform leftAnim, rightAnim;
        [SerializeField] private AnimationPack tempWin, tempLose;

        private int _score = 0;
        private float _time = 0;
        private bool _timerOn;


        private void Start()
        {
            _timerOn = true;
        }
        private void OnEnable()
        {
            BaseGame.OnPlayAnimations += PlayAnimations;
            BaseGame.OnScoreUpdate += ScoreDisplay;
            BaseGame.OnTimerUpdate += UpdateTimer;
            MinigameManager.OnSetKeys += DisplayKeys;
            MinigameManager.OnClearKeys += ClearKeys;
        }

        private void OnDisable()
        {
            BaseGame.OnPlayAnimations -= PlayAnimations;
            BaseGame.OnScoreUpdate -= ScoreDisplay;
            BaseGame.OnTimerUpdate -= UpdateTimer;
            MinigameManager.OnSetKeys -= DisplayKeys;
            MinigameManager.OnClearKeys -= ClearKeys;
        }

        // Update is called once per frame
        void Update()
        {
            //if the lives are higher than three,
            //there is only one heart with the number of lives next to it displayed
            if(settings.Lives > 3)
            {
                hearts[0].sprite = fullHeart;
                hearts[1].gameObject.SetActive(false);
                hearts[2].gameObject.SetActive(false);
                heartCounter.text = settings.Lives.ToString();
                heartCounter.gameObject.SetActive(true);
            } else
            {
                heartCounter.gameObject.SetActive(false);
                //display three empty hearts
                foreach (Image img in hearts)
                {
                    img.gameObject.SetActive(true);
                    img.sprite = emptyHeart;
                }

                //depending on the health size, the amount of hearts will be filled
                for (int i = 0; i < settings.Lives; i++)
                {
                    hearts[i].sprite = fullHeart;
                }
            }

            if (settings.Lives <= 0)
            {
                _timerOn = false;
                PlayerPrefs.SetString("Score",_score.ToString("D3"));
                TimeSpan timePlaying = TimeSpan.FromSeconds(_time);
                PlayerPrefs.SetString("Time",timePlaying.ToString("mm':'ss"));
                SceneManager.LoadScene((int)SceneNr.GameOver);
            }

            if (_timerOn)
            {
                _time += Time.deltaTime;
                TimeSpan timePlaying = TimeSpan.FromSeconds(_time);
                timeCounter.text = timePlaying.ToString("mm':'ss");
            }
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

            if (!actions.Other.Equals(""))
            {
                GameObject k = Instantiate(templateKeys, parent);
                TMP_Text[] texts = k.transform.GetComponentsInChildren<TMP_Text>();
                texts[1].text = actions.Other;
                k.SetActive(true);
                return;
            }

            for (int i = 0; i < 4; i++)
            {
                if (keys.All[i] == null || actions.All[i].Equals("")) return;

                GameObject k = Instantiate(templateKeys, parent);
                TMP_Text[] texts = k.transform.GetComponentsInChildren<TMP_Text>();
                texts[0].text = keys.All[i].Icon;
                texts[1].text = actions.All[i];
                k.SetActive(true);
            }
        }

        private void ClearKeys(string side)
        {
            if (side.Contains("Left")) RemoveKeys(leftKeys);
            if (side.Contains("Right")) RemoveKeys(rightKeys);
            if (side.Contains("Center")) RemoveKeys(centerKeys);
        }

        //private void SetAnimations((string parent, int score, float timer, int toWin, int toLose) vars)
        //{
        //    if (vars.parent.Contains("Left") || vars.parent.Contains("Center"))
        //    {
        //        leftTimer.Duration = vars.timer;
        //        if (vars.timer <= 0) leftTimer.gameObject.SetActive(false);
        //        leftAnim.Count = vars.score;
        //        leftAnim.WinsMax = vars.toWin;
        //        leftAnim.FailsMax = vars.toLose;
        //    }
        //    else
        //    {
        //        rightTimer.Duration = vars.timer;
        //        if (vars.timer <= 0) rightTimer.gameObject.SetActive(false);
        //        rightAnim.Count = vars.score;
        //        rightAnim.WinsMax = vars.toWin;
        //        rightAnim.FailsMax = vars.toLose;
        //    }
        //}

        /// <summary>
        /// Plays an animation of a certain type and in a certain position.
        /// </summary>
        /// <param name="parent">Where to spawn the animation.</param>
        /// <param name="anim">The animation type.</param>
        /// <param name="count">The number of stars.</param>
        /// <param name="numerator">The portion to fill.</param>
        /// <param name="denominator">The number of portions total.</param>
        private void PlayAnimations(string parent, AnimType anim, int count, float numerator, float denominator)
        {
            AnimationPack pack = tempWin;
            if (anim == AnimType.Lose) pack = tempLose;

            Transform pos = leftAnim;
            if (parent.Contains("Right"))
            {
                pos = rightAnim;
            }

            pack = Instantiate(pack, pos);
            pack.Run(count, numerator, denominator, anim.ToString());
        }

        private void UpdateTimer(string side, float fraction)
        {
            Image timer = leftTimer;
            if (!timer.gameObject.activeInHierarchy) timer.gameObject.SetActive(true);
            if (side.Contains("Right")) timer = rightTimer;

            timer.fillAmount += fraction;
            if(timer.fillAmount >= 1) timer.gameObject.SetActive(false);
        }

        private void RemoveKeys(Transform parent)
        {
            for (int i = 0; i < parent.childCount; i++)
            {
                Destroy(parent.GetChild(i).gameObject);
            }
        }
    }

}

