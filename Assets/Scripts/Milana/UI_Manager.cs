using Scripts.Models;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.Controllers
{
    public class UI_Manager : MonoBehaviour
    {
        [SerializeField] private Settings settings;
        [SerializeField] private Image[] hearts;
        [SerializeField] private Sprite fullHeart, emptyHeart;
        [SerializeField] private TMP_Text heartCounter, scoreCounter, timeCounter;
        [SerializeField] private GameObject gameOverPanel;


        private int _score = 0;
        private float _time = 0;
        private bool _timerOn;


        private void Start()
        {
            _timerOn = true;
            MinigameManager.OnUpdateUIScore += ScoreDisplay;
        }

        private void OnDisable()
        {
            MinigameManager.OnUpdateUIScore -= ScoreDisplay;
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
                gameOverPanel.SetActive(true);
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
    }

}

