using System;
using Scripts.Models;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Scripts.Controllers
{
    public class MainMenuGame : MonoBehaviour, ISelectHandler
    {
        #region Serialized Fields

            [SerializeField] private Settings settings;
            [SerializeField] private Image preview;
            [SerializeField] private TMP_Text title;

        #endregion Serialized Fields

        #region Fields

            public static event Action selectionChanged;
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

            //string gameName = game.Prefab.name;
            //string sTemp = Regex.Replace(gameName, "([A-Z][a-z])", " $1", RegexOptions.Compiled).Trim();


            title.text = game.Prefab.name;//Regex.Replace(sTemp, "([A-Z][A-Z])", " $1", RegexOptions.Compiled).Trim().ToLower();
                if (game.Preview != null) preview.sprite = game.Preview;
            }

            public void OnSelect(BaseEventData eventData)
            {
                //Debug.Log("new Selection");
                selectionChanged?.Invoke();
            }

        //private String separateCamelCase()
        //{
        //    return "";
        //}

        //private string SplitCamelCase(this string str)
        //{
        //    return Regex.Replace(
        //    Regex.Replace(
        //    str,
        //    @"(\P{Ll})(\P{Ll}\p{Ll})",
        //    "$1 $2"
        //    ),
        //    @"(\p{Ll})(\P{Ll})",
        //    "$1 $2"
        //    );
        //}

        #endregion Game Mechanics / Methods
    }
}