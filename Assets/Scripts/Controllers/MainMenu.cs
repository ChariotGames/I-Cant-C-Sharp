using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Scripts.Models;

namespace Scripts.Controllers
{
    /// <summary>
    /// A basic main menu control.
    /// </summary>
    public class MainMenu : MonoBehaviour
    {
        #region Serialized Fields

        [SerializeField] private Camera mainCamera;
        [SerializeField] private CanvasScaler canvasScaler;
        [SerializeField] private GameObject gamesContainer, templateGameButton, optionLives, startButton;
        [SerializeField] private Settings defaultSettings, settings;
        [SerializeField] private TMP_Text livesText, difficultyText;

        #endregion Serialized Fields

        #region Fields

        private List<GameObject> gameButtons;
        private const float REFERENCE_WIDTH = 1920f;

        #endregion Fields

        #region Built-Ins / MonoBehaviours

        void Start()
        {
            if (mainCamera) mainCamera = Camera.main;

            ResetSettings();
            SetSelected(startButton);
            canvasScaler.scaleFactor = mainCamera.pixelWidth / REFERENCE_WIDTH;
            gameButtons = new();
            livesText.text = settings.Lives.ToString();
            difficultyText.text = settings.BaseDifficulty.ToString();
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
                gameButtons.Add(button);
            }

            foreach (Minigame game in settings.SoloGames)
            {
                GameObject button = Instantiate(templateGameButton, gamesContainer.transform);
                button.GetComponent<MainMenuGame>().SetupButton(game);
                gameButtons.Add(button);
            }

            EventSystem.current.SetSelectedGameObject(gamesContainer.transform.GetChild(0).gameObject);
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
            settings.Lives = defaultSettings.Lives;
            SetLives(0); // Updates the menu text
            settings.Players = defaultSettings.Players;
            settings.SelectedGame = defaultSettings.SelectedGame;
            settings.Games = defaultSettings.Games;
            settings.SoloGames = defaultSettings.SoloGames;
        }

        /// <summary>
        /// Updates the life counter.
        /// </summary>
        /// <param name="change">The difference to set to.</param>
        public void SetLives(int change)
        {
            settings.Lives += change;
            livesText.text = settings.Lives.ToString();
        }

        /// <summary>
        /// Sets the base difficulty for all games.
        /// </summary>
        /// <param name="change">The difference to set to.</param>
        public void SetDifficulty(int change)
        {
            int current = (int)settings.BaseDifficulty + change;
            settings.BaseDifficulty = (Difficulty)Mathf.Clamp(current, (int)Difficulty.EASY, (int)Difficulty.VARYING);
            difficultyText.text = settings.BaseDifficulty.ToString();
        }

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