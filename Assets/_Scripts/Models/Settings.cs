using System.Collections.Generic;
using UnityEngine;

namespace _Scripts.Models
{
    [CreateAssetMenu(fileName = "Settings", menuName = "ScriptableObjects/Settings", order = 2)]
    public class Settings : ScriptableObject
    {
        #region Fields

        [SerializeField] private int lives = 3, players = 1;
        [SerializeField] private Minigame selectedGame = null;
        [SerializeField] private List<Minigame> games = new(), soloGames = new();

        #endregion

        #region GetSets

        /// <summary>
        /// The Lives a player has to play the game.
        /// Ranges from 0-9.
        /// </summary>
        public int Lives
        {
            get => lives;
            set => lives = Mathf.Clamp(value, 0, 9);
        }

        /// <summary>
        /// The number of players playing the game together.
        /// Ranges from 1-4.
        /// </summary>
        public int Players
        {
            get => players;
            set => players = Mathf.Clamp(value, 1, 4);
        }

        /// <summary>
        /// The selected game to play once in single or tutorial mode.
        /// </summary>
        public Minigame SelectedGame
        {
            get => selectedGame;
            set => selectedGame = value;
        }

        /// <summary>
        /// The list of Minigames able to play in combination with others.
        /// </summary>
        public List<Minigame> Games
        {
            get => games;
            set => games = value;
        }

        /// <summary>
        /// The list of Minigames able to play alone.
        /// Either due to size or complexity.
        /// </summary>
        public List<Minigame> SoloGames
        {
            get => soloGames;
            set => soloGames = value;
        }

        #endregion
    }
}
