using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Models
{
    [CreateAssetMenu(fileName = "Settings", menuName = "ScriptableObjects/Settings")]
    public class Settings : ScriptableObject
    {
        #region Fields

        [SerializeField] private int lives = 3, players = 1;
        [SerializeField] private Difficulty baseDifficulty;
        [SerializeField] private Minigame selectedGame = null;
        [SerializeField] private Character selectedCharacter = null;
        [SerializeField] private List<Minigame> games, soloGames;
        [SerializeField] private List<Character> characters;

        [SerializeField] private int _highscore, _score, _time;
        

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

        /// <summary>
        /// The list of Characters to pick.
        /// </summary>
        public List<Character> Characters
        {
            get => characters;
            set => characters = value;
        }

        /// <summary>
        /// The selected character to play as.
        /// </summary>
        public Character SelectedCharacter
        {
            get => selectedCharacter;
            set => selectedCharacter = value;
        }

        // The base difficulty of the whole game.
        public Difficulty BaseDifficulty
        {
            get => baseDifficulty;
            set => baseDifficulty = value;
        }

        /// <summary>
        /// The play time.
        /// </summary>
        public int Time
        {
            get => _time;
            set => _time = value;
        }

        /// <summary>
        /// The score of the game.
        /// </summary>
        public int Score
        {
            get => _score;
            set => _score = value;
        }

        /// <summary>
        /// The score of the game.
        /// </summary>
        public int Highscore
        {
            get => _highscore;
            set => _highscore = value;
        }

        #endregion
    }
}
