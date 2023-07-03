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

        [SerializeField] private TMP_Text score;
        [SerializeField] private TMP_Text timer;
        [SerializeField] private TMP_Text scoreText;
        [SerializeField] private TMP_Text timerText;


        #endregion Serialized Fields

        #region Fields

        private InputActionMap playerMap, uiMap;

        #endregion Fields

        #region Built-Ins / MonoBehaviours

        private void Awake()
        {
            scoreText.text = "Score: " + score.text;
            timerText.text = "Time: " + timer.text;

            playerMap = playerInput.actionMaps[0];
            uiMap = playerInput.actionMaps[1];
            uiMap.Enable();

        }

        void Start()
        {

        }

        #endregion Built-Ins / MonoBehaviours

        #region Game Mechanics / Methods

        public void Restart()
        {
            settings.Lives = defaultSettings.Lives;
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        public void GoToMenu()
        {
            SceneChanger.ChangeScene(0);
        }

        public void QuitGame()
        {
            #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
            #endif
                Application.Quit();
        }

        #endregion Game Mechanics / Methods

        #region Overarching Methods / Helpers

        private void OnEnable()
        {
            playerMap.Disable();
            EventSystem.current.SetSelectedGameObject(restartButton);
        }

        private void OnDisable()
        {

        }

        #endregion Overarching Methods / Helpers
    }
}
