using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace _Scripts
{
    public class MainMenu : MonoBehaviour
    {
        #region Serialized Fields

        [SerializeField] private Camera mainCamera;
        [SerializeField] private CanvasScaler canvasScaler;
        [SerializeField] private GameObject gamesContainer, templateGameButton;
        [SerializeField] private Settings settings;

        #endregion Serialized Fields

        #region Fields

        private List<GameObject> gameButtons;
        private const float REFERENCE_WIDTH = 1920f;

        #endregion Fields

        #region Built-Ins / MonoBehaviours

        void Start()
        {
            gameButtons = new();
            canvasScaler.scaleFactor = mainCamera.pixelWidth / REFERENCE_WIDTH;
        }

        #endregion Built-Ins / MonoBehaviours

        #region Game Mechanics / Methods

        /// <summary>
        /// Fills the games page with game assets on the first run.
        /// </summary>
        public void FillGamesContainer()
        {
            if (gamesContainer.transform.childCount != 0) return;

            foreach (var game in settings.Games)
            {
                GameObject button = Instantiate(templateGameButton, gamesContainer.transform);
                button.GetComponentInChildren<TMP_Text>().text = CovertIDtoName(game.AssetID);
                gameButtons.Add(button);
            }
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

        #region Overarching Methods / Helpers

        /// <summary>
        /// Sorts the game buttons on screen. Might be obsolete!
        /// </summary>
        private void SortButtons()
        {
            foreach (var button in gameButtons)
            {
                //TODO Button pos anordnen
            }
        }

        /// <summary>
        /// Converts an AssetID enum to a propper game name.
        /// </summary>
        /// <param name="id">The ID to convert.</param>
        /// <returns>Name with spaces.</returns>
        private string CovertIDtoName(AssetID id)
        {
            string name = id.ToString()[4..];
            string result = "";

            foreach (char c in name)
            {
                if (char.IsUpper(c)) result += " ";
                result += c;
            }
            return result.Trim();
        }

        #endregion Overarching Methods / Helpers
    }
}