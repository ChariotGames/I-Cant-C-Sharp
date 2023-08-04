using System;
using System.Collections;
using Scripts.Models;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Scripts.Controllers
{
    public class GameOverMenu : MonoBehaviour
    {
        #region Serialized Fields
        
        [SerializeField] private Animator endAnimator, camAnimator;
        [SerializeField] private HighscorePanel highscorePanel;
        [SerializeField] private Settings settings;
        [SerializeField] private TMP_Text scoreText, timerText;

        #endregion Serialized Fields

        #region Fields
        

        #endregion Fields

        #region Built-Ins / MonoBehaviours

        private void Awake()
        {
            endAnimator.SetTrigger("EndIn");
            scoreText.text = settings.Score.ToString("D3");
            timerText.text = TimeSpan.FromSeconds(settings.Time).ToString("mm':'ss");
            highscorePanel.DisplayScores();
        }

        #endregion Built-Ins / MonoBehaviours

        #region Game Mechanics / Methods

        public void Restart()
        {
            settings.Lives = settings.MaxLives;
            StartCoroutine(AnimateToPlay());
            camAnimator.SetTrigger("EndToPlay");
        }

        public void GoToMenu()
        {
            StartCoroutine(AnimateToMenu());
            camAnimator.SetTrigger("EndToMenu");
        }

        public void QuitGame()
        {
            SavePrefs();
            #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
            #endif
                Application.Quit();
        }



        /// <summary>
        /// Saves settings to PlayerPrefs
        /// </summary>
        public void SavePrefs()
        {
            PlayerPrefs.SetInt("Difficulty", (int)settings.BaseDifficulty);

            PlayerPrefs.SetInt("MaxLives", settings.MaxLives);

            PlayerPrefs.SetFloat("MainVolume", settings.MainVolume);

            PlayerPrefs.SetFloat("MusicVolume", settings.MusicVolume);

            PlayerPrefs.SetFloat("SoundVolume", settings.SoundVolume);

            PlayerPrefs.SetString("CharacterName", settings.SelectedCharacter.Name);
        }

        private IEnumerator AnimateToMenu()
        {
            yield return new WaitForSeconds(1.0f);
            SceneManager.LoadScene((int)SceneNr.MainMenu);
        }

        private IEnumerator AnimateToPlay()
        {
            yield return new WaitForSeconds(1.0f);
            SceneManager.LoadScene((int)SceneNr.PlayField);
        }

        #endregion Game Mechanics / Methods
    }
}
