using System;
using System.Collections;
using System.Collections.Generic;
using _Scripts._Input;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

namespace _Scripts.Controllers
{
    public class PauseMenu : MonoBehaviour
    {

        #region Serialized Fields

        [SerializeField] private InputActionAsset playerInput;
            [SerializeField] private GameObject pauseMenu;
            [SerializeField] private GameObject button;

        #endregion Serialized Fields


        #region Fields
            
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
            playerInput.actionMaps[1].Enable();
        }

        void Start()
            {
                _isPaused = false;
            }

        #endregion Built-Ins / MonoBehaviours


        #region Game Mechanics / Methods

            private void PauseGame()
            {
                _isPaused = true;
                pauseMenu.SetActive(true);
                EventSystem.current.SetSelectedGameObject(button);
                Time.timeScale = 0;
                playerInput.actionMaps[0].Disable();
            }
        
            public void ResumeGame()
            {
                _isPaused = false;
                pauseMenu.SetActive(false);
                Time.timeScale = 1;
                playerInput.actionMaps[0].Enable();
            }

            public void GoToMenu()
            {
                Time.timeScale = 1;
                playerInput.actionMaps[0].Enable();
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
                InputHandler.OptionButtonStart += PauseButtonPressed;
            }

            private void PauseButtonPressed()
            {
                Debug.Log("Input Registered");
                if (_isPaused)
                {
                    ResumeGame();
                }
                else
                {
                    PauseGame();
                }
            }
            
            private void OnDisable()
            {
                InputHandler.OptionButtonStart -= PauseButtonPressed;
            }
        
        #endregion Overarching Methods / Helpers
    }
}
