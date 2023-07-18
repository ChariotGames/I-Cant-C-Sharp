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
        [SerializeField] private GameObject restartButton;
        [SerializeField] private Settings defaultSettings, settings;

        [SerializeField] private int score;
        [SerializeField] private int timer;
        [SerializeField] private TMP_Text scoreText;
        [SerializeField] private TMP_Text timerText;


        #endregion Serialized Fields

        #region Fields

        private InputActionMap playerMap, uiMap;

        #endregion Fields

        #region Built-Ins / MonoBehaviours

        private void Awake()
        {
            scoreText.text += score.ToString("D3");
            timerText.text +=  timer.ToString("mm':'ss");

            playerMap = playerInput.actionMaps[0];
            uiMap = playerInput.actionMaps[1];
            uiMap.Enable();

        }

        private void OnEnable()
        {
            playerMap.Disable();
            EventSystem.current.SetSelectedGameObject(restartButton);
        }

        #endregion Built-Ins / MonoBehaviours

        #region Game Mechanics / Methods

        public void Restart()
        {
            settings.Lives = defaultSettings.Lives;
            string t = SceneManager.GetActiveScene().name;
            SceneManager.LoadScene((int)SceneNr.PlayField);
        }

        public void GoToMenu()
        {
            uiMap.Disable();
            SceneManager.LoadScene((int)SceneNr.MainMenu);
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
