using System;
using System.Collections;
using System.Collections.Generic;
using Scripts._Input;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

namespace Scripts.Controllers
{
    public class PauseMenu : MonoBehaviour
    {

        #region Serialized Fields

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

        #endregion Built-Ins / MonoBehaviours


        #region Game Mechanics / Methods

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

            public void GoToMenu()
            {
                Time.timeScale = 1;
                uiMap.Disable();
                playerMap.Enable();
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
                button.action.performed += PauseButtonPressed;
            }

            private void PauseButtonPressed(InputAction.CallbackContext ctx)
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
                button.action.performed -= PauseButtonPressed;
            }
        
        #endregion Overarching Methods / Helpers
    }
}
