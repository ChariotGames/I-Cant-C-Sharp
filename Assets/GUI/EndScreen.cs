using System;
using System.Collections;
using System.Collections.Generic;
using Scripts.Models;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace Scripts.Controllers
{
    public class EndScreen : MonoBehaviour
    {
        #region Serialized Fields

        [SerializeField] private InputActionAsset playerInput;
        [SerializeField] private GameObject restartButton, highscorePanel;
        [SerializeField] private Settings settings;

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
            highscorePanel.GetComponent<HighscoreDisplay>().triggerAwake();
            //HighscoreDisplay.AddScore();

            playerMap = playerInput.actionMaps[0];
            uiMap = playerInput.actionMaps[1];
        }

        private void OnEnable()
        {
            playerMap.Disable();
            uiMap.Enable();
            EventSystem.current.SetSelectedGameObject(restartButton);
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
            SceneManager.LoadScene((int)SceneNr.MainMenu);
        }

        public void ShowScoretable()
        {
            // TODO: Implement code
        }

        public void QuitGame()
        {
            #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
            #endif
                Application.Quit();
        }

        #endregion Game Mechanics / Methods
    }
}
