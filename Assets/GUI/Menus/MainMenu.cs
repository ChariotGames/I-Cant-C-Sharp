using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Scripts.Models;
using UnityEngine.SceneManagement;
using Scripts.Games;

namespace Scripts.Controllers
{
    /// <summary>
    /// A basic main menu control.
    /// </summary>
    public class MainMenu : MonoBehaviour
    {
        #region Serialized Fields

        [SerializeField] private Animator startAnimator;
        [SerializeField] private Camera mainCamera;
        [SerializeField] private CanvasScaler canvasScaler;
        [SerializeField] private GameObject templateGameButton, templateCharacterButton, gamesContainer, characterContainer;
        [SerializeField] private Settings defaultSettings, settings;
        [SerializeField] private TMP_Text livesText;
        [SerializeField] private TMP_Dropdown difficulty;

        #endregion Serialized Fields

        #region Fields

        private const float REFERENCE_WIDTH = 1920f;

        #endregion Fields

        #region Built-Ins / MonoBehaviours

        void Start()
        {
            if (mainCamera) mainCamera = Camera.main;

            startAnimator.SetTrigger("BookIn");
            ResetSettings();
            canvasScaler.scaleFactor = mainCamera.pixelWidth / REFERENCE_WIDTH;
        }

        #endregion Built-Ins / MonoBehaviours

        #region Game Mechanics / Methods

        /// <summary>
        /// Fills the games page with game assets on the first run.
        /// </summary>
        public void FillGamesContainer()
        {
            if (gamesContainer.transform.childCount != 0)
            {
                EventSystem.current.SetSelectedGameObject(gamesContainer.transform.GetChild(0).gameObject);
                return;
            }

            foreach (Minigame game in settings.Games)
            {
                GameObject button = Instantiate(templateGameButton, gamesContainer.transform);
                button.GetComponent<MainMenuGame>().SetupButton(game);
                //gameButtons.Add(button);
            }

            foreach (Minigame game in settings.SoloGames)
            {
                GameObject button = Instantiate(templateGameButton, gamesContainer.transform);
                button.GetComponent<MainMenuGame>().SetupButton(game);
                //gameButtons.Add(button);
            }

            EventSystem.current.SetSelectedGameObject(gamesContainer.transform.GetChild(0).gameObject);
        }

        /// <summary>
        /// Fills the character page with char assets on the first run.
        /// </summary>
        public void FillCharacterContainer()
        {
            if (characterContainer.transform.childCount == 0)
            {
                foreach (Character character in settings.Characters)
                {
                    GameObject button = Instantiate(templateCharacterButton, characterContainer.transform);
                    button.GetComponent<MainMenuCharacter>().SetupButton(character);
                    //gameButtons.Add(button);
                }
            }

            EventSystem.current.SetSelectedGameObject(characterContainer.transform.GetChild(0).gameObject);
        }

        public void RunEndless()
        {
            SceneManager.LoadScene((int)SceneNr.PlayField);
        }

        /// <summary>
        /// Quits the application or editor.
        /// </summary>
        public void Quit()
        {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #endif
            Application.Quit();
        }

        /// <summary>
        /// Resets all settings back to default.
        /// </summary>
        public void ResetSettings()
        {
            settings.Lives = settings.MaxLives;
            settings.Games = new List<Minigame>(defaultSettings.Games);
            settings.SoloGames = new List<Minigame>(defaultSettings.SoloGames);
            settings.Characters = new List<Character>(defaultSettings.Characters);
            //settings.BaseDifficulty = defaultSettings.BaseDifficulty;
            settings.SelectedGame = defaultSettings.SelectedGame;
            settings.Time = defaultSettings.Time;
            settings.Score = defaultSettings.Score;
            livesText.text = settings.Lives.ToString();
            difficulty.value = (int)settings.BaseDifficulty;
        }

        /// <summary>
        /// Updates the life counter.
        /// </summary>
        /// <param name="change">The difference to set to.</param>
        public void SetLives(int change)
        {
            settings.Lives = Mathf.Clamp(settings.Lives + change, 1, 9);
            settings.MaxLives = settings.Lives;
            livesText.text = settings.Lives.ToString();
        }

        /// <summary>
        /// Sets the base difficulty for all games.
        /// </summary>
        /// <param name="change">The difference to set to.</param>
        public void SetDifficulty(int value)
        {
            settings.BaseDifficulty = (Difficulty)Mathf.Clamp(value, (int)Difficulty.TUTORIAL, (int)Difficulty.VARYING);
            //difficultyText.text = settings.BaseDifficulty.ToString();
        }

        public void ClearHighscores()
        {
            PlayerPrefs.DeleteKey("HighscoreList");
        }

        /// <summary>
        /// Resets the difficulty of all games back to easy.
        /// Just in case! But you lose all progress!
        /// </summary>
        public void ResetDifficulties()
        {
            foreach (Minigame game in settings.Games)
            {
                game.Difficulty = Difficulty.EASY;
                game.Prefab.GetComponent<BaseGame>().Difficulty = Difficulty.EASY;
            }

            foreach (Minigame game in settings.SoloGames)
            {
                game.Difficulty = Difficulty.EASY;
                game.Prefab.GetComponent<BaseGame>().Difficulty = Difficulty.EASY;
            }
        }

        // Obsolete Codes, just in case.

        /// <summary>
        /// Sets the number of player.
        /// </summary>
        /// <param name="count">The number of players.</param>
        public void SetPlayers(int count)
        {
            settings.Players = count;
        }

        /// <summary>
        /// On menu change, set the given button as selected.
        /// </summary>
        /// <param name="button">The buton to select.</param>
        public void SetSelected(GameObject button)
        {
            EventSystem.current.SetSelectedGameObject(button);
        }

        /// <summary>
        /// Toggles a given menu on or off.
        /// </summary>
        /// <param name="menu">The menu to toggle.</param>
        public void ToggleSubMenu(GameObject menu)
        {
            menu.SetActive(!menu.activeInHierarchy);
        }
        #endregion Game Mechanics / Methods
    }
}