using System.Collections.Generic;
using UnityEngine;

namespace _Scripts
{
    [CreateAssetMenu(fileName = "Settings", menuName = "ScriptableObjects/Settings", order = 2)]
    public class Settings : ScriptableObject
    {
        #region Fields

        [SerializeField] private int lives = 3, players = 1;
        [SerializeField] private Mode mode = Mode.SINGLE;
        [SerializeField] private Minigame selectedGame = null;
        [SerializeField] private List<Minigame> games = new();

        #endregion

        #region GetSets

        public int Lives
        {
            get => lives;
            set => lives = Mathf.Clamp(value, 1, 9);
        }
        public int Players
        {
            get => players;
            set => players = Mathf.Clamp(value, 1, 4);
        }

        public Mode Mode
        {
            get => mode;
            set => mode = value;
        }

        public Minigame SelectedGame
        {
            get => selectedGame;
            set => selectedGame = value;
        }

        public List<Minigame> Games
        {
            get => games;
            set => games = value;
        }

        #endregion
    }
}
