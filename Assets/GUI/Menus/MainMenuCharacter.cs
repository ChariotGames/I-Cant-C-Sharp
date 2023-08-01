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
    public class MainMenuCharacter : MonoBehaviour, ISelectHandler
    {
        #region Serialized Fields

            [SerializeField] private Settings settings;
            [SerializeField] private Image preview;
            [SerializeField] private TMP_Text title;

        #endregion Serialized Fields

        #region Fields

            public static event Action selectionChanged;
            private Character _character;

        #endregion Fields

        #region Game Mechanics / Methods

            /// <summary>
            /// Sets the chosen game to play at start.
            /// </summary>
            public void SetCharacter()
            {
                settings.SelectedCharacter = _character;
            }

            /// <summary>
            /// Sets up the button's properties at creation.
            /// </summary>
            /// <param name="input">The character asset to get the relevant data from.</param>
            public void SetupButton(Character input)
            {
                _character = input;

                string sTemp = Regex.Replace(_character.Name, "([A-Z][a-z])", " $1", RegexOptions.Compiled).Trim();

                title.text = Regex.Replace(sTemp, "([A-Z][A-Z])", " $1", RegexOptions.Compiled).Trim().ToLower();
                if (_character.Preview != null) preview.sprite = _character.Preview;
            }

            public void OnSelect(BaseEventData eventData)
            {
                //Debug.Log("new Selection");
                selectionChanged?.Invoke();
            }
        #endregion Game Mechanics / Methods
    }
}