using Scripts.Models;
using System;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Scripts.Controllers
{
    public class PauseMenu : MonoBehaviour
    {

        #region Serialized Fields

            [SerializeField] private Settings settings;
            [SerializeField] private InputActionAsset playerInput;
            [SerializeField] private GameObject resumeButton;
            [SerializeField] private InputActionReference button;
            [SerializeField] private AudioMixer audioMixer;
            [SerializeField] private Slider mainVolume, musicVolume, soundVolume;
            [SerializeField] private Animator pauseAnimator;

        #endregion Serialized Fields


        #region Fields

            private InputActionMap playerMap, uiMap;
            public static event Action OnToMenu;

        // TODO: maybe not static?
        private bool _isPaused;

        #endregion Fields
        
        
        #region GetSets
            /// <summary>
            /// Getter to read isPaused value in all Classes
            /// </summary>
            public bool IsPaused
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
            ResetAudio();
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
                pauseAnimator.SetTrigger("PauseIn");
                EventSystem.current.SetSelectedGameObject(resumeButton);
                Time.timeScale = 0;
                playerMap.Disable();
            }
        
            public void ResumeGame()
            {
                _isPaused = false;
                pauseAnimator.SetTrigger("PauseOut");
                Time.timeScale = 1;
                playerMap.Enable();
            }

            public void EndRun()
            {
                pauseAnimator.SetTrigger("PauseOut");
                
                uiMap.Disable();
                //playerMap.Enable();
                ResumeGame();
                settings.Lives = 0;
                //Time.timeScale = 1;
            }

            public void GoToMenu()
            {
                pauseAnimator.SetTrigger("PauseOut");
                
                Time.timeScale = 1;
                uiMap.Disable();
                playerMap.Enable();
                OnToMenu?.Invoke();
            }

            public void QuitGame()
            {
                #if UNITY_EDITOR
                    UnityEditor.EditorApplication.isPlaying = false;
                #endif
                    Application.Quit();
            }

            private void ResetAudio()
            {
                float main = settings.MainVolume;
                float music = settings.MusicVolume;
                float sound = settings.SoundVolume;

                SetMainVolume(main);
                SetMusicVolume(music);
                SetSoundVolume(sound);

                mainVolume.value = main;
                musicVolume.value = music;
                soundVolume.value = sound;
            }

            public void SetMainVolume(float volume)
            {
                audioMixer.SetFloat("MasterVolume", volume);
                settings.MainVolume = volume;
            }

            public void SetMusicVolume(float volume)
            {
                audioMixer.SetFloat("MusicVolume", volume);
                settings.MusicVolume = volume;
            }

            public void SetSoundVolume(float volume)
            {
                audioMixer.SetFloat("SoundVolume", volume);
                settings.SoundVolume = volume;
            }

        #endregion Game Mechanics / Methods
    }
}
