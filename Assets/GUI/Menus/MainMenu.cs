using Scripts.Games;
using Scripts.Models;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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
        [SerializeField] private AudioMixer audioMixer;
        [SerializeField] private Button parallelButton;
        [SerializeField] private Slider mainVolume, musicVolume, soundVolume;

        #endregion Serialized Fields

        #region Fields

        private const float REFERENCE_WIDTH = 1920f;

        #endregion Fields

        #region Built-Ins / MonoBehaviours

        void Awake()
        {
            if (mainCamera) mainCamera = Camera.main;
            canvasScaler.scaleFactor = mainCamera.pixelWidth / REFERENCE_WIDTH;

            LoadPrefs();
            startAnimator.SetTrigger("BookIn");
            ClearSettings();
            ResetAudio(false);
            FillCharacterContainer();
            FillGamesContainer();
            if (settings.BaseDifficulty == Difficulty.TUTORIAL) parallelButton.interactable = false;
        }

        #endregion Built-Ins / MonoBehaviours

        #region Game Mechanics / Methods

        /// <summary>
        /// Loads settings from PlayerPrefs
        /// </summary>
        public void LoadPrefs()
        {
            if (PlayerPrefs.HasKey("Difficulty")) SetDifficulty(PlayerPrefs.GetInt("Difficulty"));

            if (PlayerPrefs.HasKey("MaxLives")) SetLives(PlayerPrefs.GetInt("MaxLives"));

            if (PlayerPrefs.HasKey("MainVolume")) SetMainVolume(PlayerPrefs.GetFloat("MainVolume"));

            if (PlayerPrefs.HasKey("MusicVolume")) SetMusicVolume(PlayerPrefs.GetFloat("MusicVolume"));

            if (PlayerPrefs.HasKey("SoundVolume")) SetSoundVolume(PlayerPrefs.GetFloat("SoundVolume"));

            if (PlayerPrefs.HasKey("CharacterName"))
            {
                string name = PlayerPrefs.GetString("CharacterName");
                foreach (Character character in settings.Characters)
                {
                    if (character.Name.Equals(name)) settings.SelectedCharacter = character;
                }  
            }
        }

        /// <summary>
        /// Saves settings to PlayerPrefs
        /// </summary>
        public void SavePrefs()
        {
            PlayerPrefs.SetInt("Difficulty", (int)settings.BaseDifficulty);

            PlayerPrefs.SetInt("MaxLives", settings.MaxLives);

            PlayerPrefs.SetFloat("MainVolume", settings.MainVolume);

            PlayerPrefs.SetFloat("MusicVolume", settings.MusicVolume);

            PlayerPrefs.SetFloat("SoundVolume", settings.SoundVolume);

            PlayerPrefs.SetString("CharacterName", settings.SelectedCharacter.Name);
        }

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
        }

        /// <summary>
        /// On menu change, set the given button as selected.
        /// </summary>
        /// <param name="button">The buton to select.</param>
        public void SetSelected(GameObject button)
        {
            EventSystem.current.SetSelectedGameObject(button.transform.GetChild(0).gameObject);
        }

        public void RunEndless()
        {
            StartCoroutine(ToPlayfield());
        }

        private IEnumerator ToPlayfield()
        {
            yield return new WaitForSeconds(1.05f);
            SceneManager.LoadScene((int)SceneNr.PlayField);
        }

        /// <summary>
        /// Quits the application or editor.
        /// </summary>
        public void Quit()
        {
            SavePrefs();
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #endif
            Application.Quit();
        }

        /// <summary>
        /// Clears the primary game relevant setting after a run.
        /// </summary>
        private void ClearSettings()
        {
            settings.Lives = settings.MaxLives;
            livesText.text = settings.Lives.ToString();
            difficulty.value = (int)settings.BaseDifficulty;
            settings.SelectedGame = null;
            difficulty.RefreshShownValue();
            settings.Time = 0;
            settings.Score = 0;
        }

        /// <summary>
        /// Resets all settings back to default.
        /// </summary>
        public void RestoreDefaults()
        {
            settings.MaxLives = defaultSettings.MaxLives;
            settings.Games = new List<Minigame>(defaultSettings.Games);
            settings.SoloGames = new List<Minigame>(defaultSettings.SoloGames);
            settings.Characters = new List<Character>(defaultSettings.Characters);
            
            ClearSettings();
            ClearHighscores();
            ResetAudio(true);
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
            if (settings.BaseDifficulty == Difficulty.TUTORIAL) parallelButton.interactable = false;
        }

        /// <summary>
        /// Resets the difficulty of all games back to easy.
        /// Just in case! But you lose all progress!
        /// </summary>
        public void ResetDifficulties()
        {
            settings.BaseDifficulty = defaultSettings.BaseDifficulty;

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

        /// <summary>
        /// Clears the highscoretable.
        /// </summary>
        private void ClearHighscores()
        {
            PlayerPrefs.DeleteKey("HighscoreList");
        }

        /// <summary>
        /// Resets audio settings back to user values or default.
        /// </summary>
        /// <param name="clear">If true, back to default: all zeroes.</param>
        private void ResetAudio(bool clear)
        {
            if (clear)
            {
                settings.MainVolume = defaultSettings.MainVolume;
                settings.MusicVolume = defaultSettings.MusicVolume;
                settings.SoundVolume = defaultSettings.SoundVolume;
            }

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

        /// <summary>
        /// Sets the main volume with the slider.
        /// </summary>
        /// <param name="volume">The value from the inspector or dynamic from slider.</param>
        public void SetMainVolume(float volume)
        {
            audioMixer.SetFloat("MasterVolume", volume);
            settings.MainVolume = volume;
        }

        /// <summary>
        /// Sets the music volume with the slider.
        /// </summary>
        /// <param name="volume">The value from the inspector or dynamic from slider.</param>
        public void SetMusicVolume(float volume)
        {
            audioMixer.SetFloat("MusicVolume", volume);
            settings.MusicVolume = volume;
        }

        /// <summary>
        /// Sets the sound volume with the slider.
        /// </summary>
        /// <param name="volume">The value from the inspector or dynamic from slider.</param>
        public void SetSoundVolume(float volume)
        {
            audioMixer.SetFloat("SoundVolume", volume);
            settings.SoundVolume = volume;
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