using System;
using System.Collections;
using System.Collections.Generic;
using Scripts._Input;
using Scripts.Models;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

namespace Scripts.Controllers
{
    public class PauseMenu : MonoBehaviour
    {

        #region Serialized Fields

        [SerializeField] private Settings settings;
        [SerializeField] private InputActionAsset playerInput;
        [SerializeField] private GameObject pauseMenu, resumeButton;
        [SerializeField] private InputActionReference button;

        #endregion Serialized Fields


        #region Fields
            
            private InputActionMap playerMap, uiMap;
            
            // TODO: maybe not static?
            private static bool _isPaused;

        #endregion Fields
        
        
        #region GetSets
            // TODO: maybe not required?
            /// <summary>
            /// Getter to read isPaused value in all Classes
            /// </summary>
            public static bool IsPaused
            {
                get => _isPaused;
            }

        #endregion GetSets


        #region Built-Ins / MonoBehaviours

        private void Awake()
        {
            playerMap = playerInput.actionMaps[0];
            uiMap = playerInput.actionMaps[1];
            uiMap.Enable();
        }

        void Start()
        {
            _isPaused = false;
        }
        private void OnEnable()
        {
            button.action.performed += PauseButtonPressed;
        }

        private void OnDisable()
        {
            button.action.performed -= PauseButtonPressed;
        }

        #endregion Built-Ins / MonoBehaviours


        #region Game Mechanics / Methods

        private void PauseButtonPressed(InputAction.CallbackContext ctx)
        {
            if (_isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }

        private void PauseGame()
            {
                _isPaused = true;
                pauseMenu.SetActive(true);
                EventSystem.current.SetSelectedGameObject(resumeButton);
                Time.timeScale = 0;
                playerMap.Disable();
            }
        
            public void ResumeGame()
            {
                _isPaused = false;
                pauseMenu.SetActive(false);
                Time.timeScale = 1;
                playerMap.Enable();
            }
            
            public void EndRun()
            {
                uiMap.Disable();
                playerMap.Enable();
                settings.Lives = 0;
                Time.timeScale = 1;
            }

            public void GoToMenu()
            {
                Time.timeScale = 1;
                uiMap.Disable();
                playerMap.Enable();
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
