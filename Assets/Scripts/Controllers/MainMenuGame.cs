using Scripts.Models;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.Controllers
{
    public class MainMenuGame : MonoBehaviour
    {
        #region Serialized Fields

        [SerializeField] private Settings settings;
        [SerializeField] private Image preview;
        [SerializeField] private TMP_Text title;

        #endregion Serialized Fields

        #region Fields

        private Minigame game;

        #endregion Fields

        #region Game Mechanics / Methods

        /// <summary>
        /// Sets the chosen game to play at start.
        /// </summary>
        public void SetGame()
        {
            settings.SelectedGame = game;
        }

        /// <summary>
        /// Sets up the button's properties at creation.
        /// </summary>
        /// <param name="input">The game asset to get the relevant data from.</param>
        public void SetupButton(Minigame input)
        {
            game = input;
            title.text = ConvertIDtoName(game.AssetID);
            if (game.Preview != null) preview.sprite = game.Preview;
        }

        #endregion Game Mechanics / Methods

        #region Overarching Methods / Helpers

        /// <summary>
        /// Converts an AssetID enum to a propper game name.
        /// </summary>
        /// <param name="id">The ID to convert.</param>
        /// <returns>Name with spaces.</returns>
        private string ConvertIDtoName(AssetID id)
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