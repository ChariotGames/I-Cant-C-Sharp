using System;
using System.Collections;
using Scripts.Models;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace Scripts.Controllers
{
    public class EndScreen : MonoBehaviour
    {
        #region Serialized Fields

        [SerializeField] private InputActionAsset playerInput;
        [SerializeField] private HighscoreDisplay highscorePanel;
        [SerializeField] private Settings settings;
        [SerializeField] private Animator animator;
        [SerializeField] private TMP_Text scoreText, timerText;


        #endregion Serialized Fields

        #region Fields

        private InputActionMap playerMap, uiMap;

        #endregion Fields

        #region Built-Ins / MonoBehaviours

        private void Awake()
        {
            scoreText.text = settings.Score.ToString("D3");
            timerText.text = TimeSpan.FromSeconds(settings.Time).ToString("mm':'ss");
            highscorePanel.DisplayScores();
            animator.ResetTrigger("CameraIn");
        }

        #endregion Built-Ins / MonoBehaviours

        #region Game Mechanics / Methods

        public void Restart()
        {
            settings.Lives = settings.MaxLives;
            //string t = SceneManager.GetActiveScene().name;
            SceneManager.LoadScene((int)SceneNr.PlayField);
        }

        public void GoToMenu()
        {
            uiMap.Disable();
            StartCoroutine(AnimateToMenu());
            animator.SetTrigger("CameraIn");
        }

        public void QuitGame()
        {
            #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
            #endif
                Application.Quit();
        }

        private IEnumerator AnimateToMenu()
        {
            yield return new WaitForSeconds(1.0f);
            SceneManager.LoadScene((int)SceneNr.MainMenu);
        }

        #endregion Game Mechanics / Methods
    }
}
